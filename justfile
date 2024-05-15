#!/usr/bin/env just --justfile

default:
    just --list
    
run:
    dotnet build
    cd AppHost && dotnet run