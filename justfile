#!/usr/bin/env just --justfile

default:
    just --list
    
# Run all services via Aspire
run:
    dotnet build
    cd src/AppHost && dotnet run --no-build

# Describe the producer service
describe-producer:
    cd src/EventProducer && dotnet run -- describe