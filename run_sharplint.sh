#!/bin/bash
# SPDX-License-Identifier: MIT
# Copyright (C) 2021 Roland Csaszar
#
# Project:  TzolkinWaves
# File:     run_sharplint.sh
# Date:     07.May.2021
###############################################################################
# install dotnet tool install -g dotnet-fsharplint

dotnet fsharplint lint TzolkinWaves.sln
