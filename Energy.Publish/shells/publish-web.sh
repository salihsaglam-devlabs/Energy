#!/bin/zsh
# Publishes the Energy.Web project. The FTP upload is handled by Energy.Publish
# (Program.cs) AFTER this script succeeds, so this script only builds output.
set -euo pipefail

cd /Users/base/Codes/Energy || exit 1

echo "==> Publishing Energy.Web ..."
dotnet publish Energy.Web/Energy.Web.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=false -p:PublishReadyToRun=false

echo "==> Energy.Web publish succeeded."
