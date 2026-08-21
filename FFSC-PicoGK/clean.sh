#!/bin/bash
# clean.sh - Clean build artifacts

set -e
cd "$(dirname "$0")"

echo "=== Cleaning FFSC_PicoGK ==="
dotnet clean PicoGKExamples.csproj
rm -rf bin/ obj/ output/

echo "=== Clean Complete ==="
