#!/bin/zsh
cd /Users/base/Codes/Energy || exit
dotnet publish Energy.Api/Energy.Api.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=false -p:PublishReadyToRun=false