# CI/CD walkthrough

This project demonstrates continuous integration and container delivery. It does
not deploy a running public service: successful master builds publish a container
image to GitHub Container Registry (GHCR).

## Pipeline flow

`pull request / push -> audited restore -> build -> unit tests -> source scan ->
image build -> image scan -> database + API smoke test -> publish (master push only)`

The container is built once. After it passes scanning and smoke tests, it is saved
as a short-lived workflow artifact. The publication job loads and tags this exact
image; it does not rebuild a potentially different image. Commit tags allow a
consumer to select a specific version rather than relying on the moving `latest` tag.

Actions are pinned to commit SHAs. Dependabot proposes weekly updates for NuGet,
Docker base images, and GitHub Actions. Updates remain subject to CI checks.

## What each check proves

| Check | Purpose | Failure behavior |
| --- | --- | --- |
| NuGet audit | Direct and transitive dependency vulnerabilities | High/critical findings or unavailable audit source fail restore |
| Release build and xUnit | Compilation and existing business logic tests | Build/test failure blocks downstream jobs |
| Trivy source scan | Recognized secret patterns in the checkout | Findings block the container job |
| Trivy image scan | OS and application dependency vulnerabilities in the image | Fixable high/critical findings block publication |
| Compose readiness | PostgreSQL starts, schema migrations apply, API can connect | Startup failure or timeout fails the job |
| Python smoke test | Real database-backed registration and login; invalid password rejection | Unexpected HTTP response or missing token fails the job |

The image scan intentionally ignores vulnerabilities without an available fix.
A passing scan does not prove absence of all vulnerabilities or authorization bugs.
The existing account registration and role-management design should be reviewed
before any production exposure.

## Local reproduction

From the repository root, with Docker Desktop's Linux engine running:

```bash
python scripts/create-dev-env.py
dotnet restore SmartAquapark.Api.slnx -p:NuGetAudit=true -p:NuGetAuditMode=all -p:NuGetAuditLevel=high -warnaserror:NU1900,NU1903,NU1904
dotnet build SmartAquapark.Api.slnx -c Release --no-restore
dotnet test SmartAquapark.Api.slnx -c Release --no-build
docker compose config --quiet
docker compose build --pull api
docker compose up -d --no-build --wait --wait-timeout 120
python scripts/smoke-test.py
```

If Trivy is installed locally, reproduce the scans with:

```bash
trivy fs --scanners secret --skip-files .env --exit-code 1 .
trivy image --scanners vuln --severity HIGH,CRITICAL --ignore-unfixed --exit-code 1 smartaquapark-api:local
```

CI scans source before generating `.env`; local scans should skip that private file.
The smoke test creates a test account on every run. It accepts `SMOKE_BASE_URL` as an
environment variable for a different development URL.

## Demonstrating a failure

On a temporary branch, change an expected result in an existing xUnit test and
open a pull request. The test job should fail, container checks should be skipped,
and there should be no published image. Revert the temporary change afterwards.

To demonstrate readiness locally, stop only the development database:

```bash
docker compose stop db
curl -i http://localhost:8080/health/live
curl -i http://localhost:8080/health/ready
docker compose start db
```

Liveness should remain HTTP 200 while readiness returns HTTP 503. Readiness returns
to HTTP 200 when the database is reachable and migrations are current. Health
responses do not expose connection details.

## Troubleshooting

- **Docker daemon unavailable:** start Docker Desktop and select Linux containers.
- **Compose reports missing credentials:** run the setup script, or fill `.env`.
- **Database authentication fails after editing `.env`:** the named volume retains
  its original password. Restore the matching credential or change the database
  password deliberately; restarting does not reinitialize existing data.
- **Migration failure:** inspect `docker compose logs api` and `docker compose logs db`.
- **Port collision:** change `API_PORT` or `DB_PORT` in `.env`.
- **Image scan fails:** inspect the `container-reports` artifact, update the affected
  dependency or base image, then rerun. Do not bypass the gate to get a green badge.
- **GHCR permission denied:** check package access and repository Actions policies.
  A pre-existing package with the same name may need its Actions access granted to
  this repository. No publication takes place during pull-request validation.

`docker compose down` stops the local demo while preserving data. CI uses a fresh,
isolated Compose project and removes its disposable database volume during cleanup.
