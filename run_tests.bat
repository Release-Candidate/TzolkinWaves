:: SPDX-License-Identifier: MIT
:: Copyright (C) 2021 Roland Csaszar
::
:: Project:  TzolkinWaves
:: File:     run_tests.bat
:: Date:     07.May.2021
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

dotnet fake run build.fsx target publish
.\bin\TzolkinWaves.exe --summary --nunit-summary .\test_results\nresult.xml --junit-summary .\test_results\jresult.xml
