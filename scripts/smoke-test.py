"""Exercise the running API and migrated PostgreSQL, using only Python stdlib."""
import json
import os
import secrets
import urllib.error
import urllib.request
import uuid

base = os.environ.get('SMOKE_BASE_URL', 'http://localhost:8080').rstrip('/')


def request(path, payload=None):
    data = json.dumps(payload).encode() if payload is not None else None
    req = urllib.request.Request(base + path, data=data,
                                 headers={'Content-Type': 'application/json'})
    with urllib.request.urlopen(req, timeout=15) as response:
        return response.status, response.read().decode()


for path in ['/health/live', '/health/ready']:
    status, body = request(path)
    if status != 200 or body != 'Healthy':
        raise SystemExit(f'{path}: expected Healthy (200), got {status}')

email = f'ci-{uuid.uuid4().hex}@example.invalid'
password = secrets.token_urlsafe(24)
status, body = request('/api/auth/register', {
    'firstName': 'CI', 'lastName': 'Smoke', 'email': email,
    'password': password, 'role': 1,
})
if status != 200 or not json.loads(body).get('token'):
    raise SystemExit('Registration failed to return a token.')
status, body = request('/api/auth/login', {'email': email, 'password': password})
if status != 200 or not json.loads(body).get('token'):
    raise SystemExit('Login failed to return a token.')
try:
    request('/api/auth/login', {'email': email, 'password': password + '-wrong'})
except urllib.error.HTTPError as error:
    if error.code != 401:
        raise
else:
    raise SystemExit('Invalid password was accepted.')
print('PASS: liveness, database readiness, registration, login and invalid-password rejection.')
