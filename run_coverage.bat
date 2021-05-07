:: SPDX-License-Identifier: MIT
:: Copyright (C) 2021 Roland Csaszar
::
:: Project:  TzolkinWaves
:: File:     run_coverage.bat
:: Date:     07.May.2021
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

:: /p:AltCoverReportFormat=�Json�, �NCover� or default �OpenCover�

dotnet test -v n /p:AltCover=true /p:AltCoverReportFormat=OpenCover /p:AltCoverReport=.\coverage.xml
