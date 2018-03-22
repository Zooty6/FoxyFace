#!/bin/bash

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
cd $DIR

export ASPNETCORE_ENVIRONMENT=Release
export ASPNETCORE_URLS=http://*:5005

cd FoxyFaceAPI/FoxyFaceAPI
dotnet restore
dotnet run --urls http://*:5005
