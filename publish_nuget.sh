#!/bin/bash
# SPDX-License-Identifier: MIT
# Copyright (C) 2021 Roland Csaszar
#
# Project:  TzolkinWaves
# File:     publish_nuget.sh
# Date:     07.May.2021
###############################################################################
# The Nuget token must be saved using `nuget setapikey` to not need to input it.

dotnet nuget push ./src/TzolkinWaves/bin/Release/TzolkinWaves.*.nupkg --source https://api.nuget.org/v3/index.json
