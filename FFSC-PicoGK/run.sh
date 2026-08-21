#!/bin/bash
# run.sh - Run the FFSC_PicoGK project

set -e
cd "$(dirname "$0")/.."

echo "=== Running FFSC_PicoGK ==="
dotnet run --project PicoGKExamples.csproj

echo "=== Run Complete ==="
