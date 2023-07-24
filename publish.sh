#!/bin/sh

# build linux binaries
dotnet publish -r linux-x64 -p:PublishSingleFile=true --self-contained true -o dist/linux-x64 src
dotnet publish -r linux-arm64 -p:PublishSingleFile=true --self-contained true -o dist/linux-arm64 src

# build windows binaries
dotnet publish -r win-x64 -p:PublishSingleFile=true --self-contained true -o dist/win-x64 src
dotnet publish -r win-arm64 -p:PublishSingleFile=true --self-contained true -o dist/win-arm64 src

# build mac binaries
dotnet publish -r osx-x64 -p:PublishSingleFile=true --self-contained true -o dist/osx-x64 src
dotnet publish -r osx-arm64 -p:PublishSingleFile=true --self-contained true -o dist/osx-arm64 src
