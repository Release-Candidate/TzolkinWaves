#!/bin/bash
# SPDX-License-Identifier: MIT
# Copyright (C) 2021 Roland Csaszar
#
# Project:  TzolkinWaves
# File:     run_tests.sh
# Date:     07.May.2021
###############################################################################
dotnet fake run ./build.fsx target publish
dotnet ./bin/TzolkinWaves.dll --summary --nunit-summary ./test_results/nresult.xml --junit-summary ./test_results/jresult.xml
