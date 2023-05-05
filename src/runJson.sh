#!/bin/sh

COMMAND="dotnet script -c Release Comparison.csx --rowCount=100 --chunkSize=20 --workers=8 --runnerType=Json"
echo "$COMMAND"
eval "$COMMAND"
