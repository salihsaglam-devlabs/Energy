#!/bin/zsh
cd /Users/base/Codes/Energy || exit
dotnet publish Energy.Web/Energy.Web.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=false -p:PublishReadyToRun=false
# Upload the published Web output to the FTP server (overwrites all files).
dotnet run --project Energy.Publish -c Release -- web
