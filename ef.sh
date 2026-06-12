#!/bin/zsh
# ---------------------------------------------------------------------------
# EF Core migration yardımcısı.
# Aktif veritabanı sağlayıcısını Energy.Api/appsettings.json içindeki
# "Environment" + ilgili bölümdeki "Database:Provider" değerinden okur ve
# migration komutlarını O sağlayıcının ayrı migration projesine yönlendirir:
#   SQL Server  -> Energy.Migrations.SqlServer
#   PostgreSQL  -> Energy.Migrations.PostgreSql
#
# Kullanım:
#   ./ef.sh add <MigrationAdı>     # aktif sağlayıcıya migration ekler
#   ./ef.sh update [hedefMigration]# veritabanını günceller
#   ./ef.sh remove                 # son migration'ı geri alır
#   ./ef.sh list                   # migration'ları listeler
#
# Sağlayıcıyı geçici olarak zorlamak için:  ENERGY_DB_PROVIDER=SqlServer ./ef.sh add X
# dotnet yolu farklıysa:                     DOTNET=/usr/local/share/dotnet/dotnet ./ef.sh ...
# ---------------------------------------------------------------------------
set -euo pipefail

ROOT="$(cd "$(dirname "$0")" && pwd)"
APPSETTINGS="$ROOT/Energy.Api/appsettings.json"
DOTNET="${DOTNET:-dotnet}"
STARTUP="$ROOT/Energy.Api/Energy.Api.csproj"
CTX="Energy.Infrastructure.Persistence.AppDbContext"

read_provider() {
    if [ -n "${ENERGY_DB_PROVIDER:-}" ]; then
        echo "$ENERGY_DB_PROVIDER"
        return
    fi
    python3 - "$APPSETTINGS" <<'PY'
import json, sys
try:
    d = json.load(open(sys.argv[1]))
except Exception:
    print("PostgreSql"); raise SystemExit
env = d.get("Environment", "Production")
prov = ((d.get(env, {}) or {}).get("Database", {}) or {}).get("Provider") \
       or (d.get("Database", {}) or {}).get("Provider") \
       or "PostgreSql"
print(prov)
PY
}

NORM="$(read_provider | tr '[:upper:]' '[:lower:]' | tr -d ' ')"
case "$NORM" in
    sqlserver|mssql|sql|mssqlserver)
        PROJ="Energy.Migrations.SqlServer"; LABEL="SQL Server";;
    *)
        PROJ="Energy.Migrations.PostgreSql"; LABEL="PostgreSQL";;
esac
PROJPATH="$ROOT/$PROJ/$PROJ.csproj"

CMD="${1:-}"
shift 2>/dev/null || true

case "$CMD" in
    add)
        NAME="${1:-}"
        if [ -z "$NAME" ]; then echo "Kullanım: ./ef.sh add <MigrationAdı>"; exit 1; fi
        echo "==> [$LABEL] migration ekleniyor: $NAME  ($PROJ)"
        $DOTNET ef migrations add "$NAME" --project "$PROJPATH" --startup-project "$STARTUP" --context "$CTX" --output-dir Migrations
        ;;
    update)
        echo "==> [$LABEL] veritabanı güncelleniyor  ($PROJ)"
        $DOTNET ef database update ${1:+"$1"} --project "$PROJPATH" --startup-project "$STARTUP" --context "$CTX"
        ;;
    remove)
        echo "==> [$LABEL] son migration kaldırılıyor  ($PROJ)"
        $DOTNET ef migrations remove --project "$PROJPATH" --startup-project "$STARTUP" --context "$CTX"
        ;;
    list)
        echo "==> [$LABEL] migration listesi  ($PROJ)"
        $DOTNET ef migrations list --project "$PROJPATH" --startup-project "$STARTUP" --context "$CTX"
        ;;
    *)
        echo "Aktif sağlayıcı: $LABEL ($PROJ)"
        echo "Kullanım: ./ef.sh {add <ad> | update [hedef] | remove | list}"
        exit 1
        ;;
esac

