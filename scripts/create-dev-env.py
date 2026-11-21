"""Generate local demo credentials without printing or overwriting them."""
from pathlib import Path
import secrets

path = Path(__file__).resolve().parents[1] / '.env'
try:
    with path.open('x', encoding='utf-8') as file:
        file.write(f'POSTGRES_PASSWORD={secrets.token_urlsafe(32)}\n')
        file.write(f'JWT_KEY={secrets.token_urlsafe(48)}\n')
        file.write('API_PORT=8080\nDB_PORT=5433\n')
    print('Created .env with unique local credentials. Keep this file private.')
except FileExistsError:
    print('.env already exists; existing credentials were preserved.')
