#!/usr/bin/env bash

set -e

dotnet ef --startup-project src/FigureMath.Data/ "$@"