#!/bin/bash
# build.sh - Build the FFSC_PicoGK project

set -e
cd "$(dirname "$0")/.."

echo "=== Building FFSC_PicoGK ==="
dotnet build PicoGKExamples.csproj

echo "=== Build Complete ==="
