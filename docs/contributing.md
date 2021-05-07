# Contributing

Any help is welcome!

If you encounter a problem using LunaZodiaco, a task it not as easy as you'd like it to be or you'd like something added to it: open an issue at GitHub, see section [Report Issues](#report-issues-bugs-and-feature-requests).

- [Contributing](#contributing)
  - [Report Issues (Bugs and Feature Requests)](#report-issues-bugs-and-feature-requests)
  - [Forking the Repository](#forking-the-repository)
    - [Github Documentation on Collaborating with Issues and Pull Requests](#github-documentation-on-collaborating-with-issues-and-pull-requests)
  - [Common Tasks Developing LunaZodiaco](#common-tasks-developing-LunaZodiaco)
    - [Jupyter Notebook](#jupyter-notebook)
    - [Changing and Generating Documentation](#changing-and-generating-documentation)
      - [Installing Dependencies](#installing-dependencies)
      - [MkDocs Files](#mkdocs-files)
      - [Read the Docs Configuration](#read-the-docs-configuration)
      - [GitHub Documentation](#github-documentation)
  - [Source Code](#source-code)
    - [Source Code and Tests](#source-code-and-tests)
      - [Source](#source)
      - [Test Source](#test-source)
    - [Build Targets](#build-targets)
  - [CodeCov Configuration](#codecov-configuration)
  - [GitHub Workflows](#github-workflows)
  - [GitHub Issue Template](#github-issue-template)

## Report Issues (Bugs and Feature Requests)

File a bug report at [Github](https://github.com/Release-Candidate/LunaZodiaco/issues/new?assignees=&labels=&template=bug_report.md&title=).
Add a feature request at [Github](https://github.com/Release-Candidate/LunaZodiaco/issues/new?assignees=&labels=&template=feature_request.md&title=).
Take a look at the [Issue Tracker at GitHub](https://github.com/Release-Candidate/LunaZodiaco/issues)

## Forking the Repository

If you'd like to contribute directly, e.g. better the documentation, add another language or write some source code: fork LunaZodiaco by clicking the `Fork` button in the upper right corner of the GitHub project website. Check out your fork of LunaZodiaco using the URL from the `Code` button of your fork on Github. The URL should be something like github.com/YOUR_USERNAME/LunaZodiaco.git.

Details about how to fork a repository on Github are [here](https://docs.github.com/en/github/getting-started-with-github/fork-a-repo).

Make your changes, push them to your forked repository and make a pull-request (e.g. using the Pull request-button above and right of GitHubs source file view).

See [GitHub on Pull-Requests](https://docs.github.com/en/github/collaborating-with-issues-and-pull-requests/proposing-changes-to-your-work-with-pull-requests) and another [How-To](https://github.com/MarcDiethelm/contributing/blob/master/README.md).

### Github Documentation on Collaborating with Issues and Pull Requests

See GitHub's documentation about how to contribute for details: [Collaborating with issues and pull requests](https://docs.github.com/en/github/collaborating-with-issues-and-pull-requests).

## Common Tasks Developing LunaZodiaco

### Jupyter Notebook

The Jupyter Notebook is located in the project root directory LunaZodiaco, named `LunaZodiaco.ipynb`. `postBuild` holds the information on how to customize the MyBinder Docker image for the use with F#.

### Changing and Generating Documentation

#### Installing Dependencies

To generate the documentation using MkDocs, a virtual Python environment is needed.
So, first you need to install Python, if you don't
have it installed already - either from your distributions repository, using the XCode or [Homebrew](https://brew.sh/) version, or getting it from [Python.org](https://www.python.org/downloads/).

See

- [Using Python on Windows](https://docs.python.org/3/using/windows.html)
- [Using Python on a Macintosh](https://docs.python.org/3/using/mac.html)
- [Using Python on Unix Platforms](https://docs.python.org/3/using/unix.html)

In the file [`Pipfile`](https://github.com/Release-Candidate/LunaZodiaco/blob/main/Pipfile) there is a stanza saying

```ini
[requires]
python_version = "3.9"
```

That's just because I used 3.9 when generating that
documentation, and Pipenv is picky about the version mentioned
there. Just edit that to match your installed
Python version.

Install `pipenv` using the package manager pip

```shell
pip install pipenv
```

Now you're ready to download and install the needed packages using pipenv

```shell
pipenv install --dev
```

After that you can use MkDocs.

Call

```shell
pipenv run mkdocs serve
```

in the root directory of LunaZodiaco and connect to the running webserver at [http://127.0.0.1:8000](http://127.0.0.1:8000).
This preview shows changes in realtime, so any changes to the markdown files in `docs` you
see as preview as soon as you save the file. The generated HTML files are saved in the directory
`sites`.

#### MkDocs Files

- `mkdocs.yml` the MkDocs configuration, specially
    the configuration of the navigation sidebar in `nav`
    Which you may need to edit

```yml
      nav:
    - Home: index.md
    - Project Links:
        - "GitHub Project Page": "https://github.com/Release-Candidate/LunaZodiaco"
        - "Nuget Package": "https://www.nuget.org/packages/LunaZodiaco/"
        - "Report a Bug or a Feature Request": "https://github.com/Release-Candidate/LunaZodiaco/issues/new/choose"
        - "Issue Tracker at GitHub": "https://github.com/Release-Candidate/LunaZodiaco/issues"
    - "Installation & Usage":
        - "Installation & Usage": usage.md
        - "NuGet Package": nuget_package.md
        - License: license.md
    - Contributing:
        - Contributing: contributing.md
```

- `docs/` the markdown files that are used to generate the
   HTML sites in the directory `sites/`

#### Read the Docs Configuration

- `.readthedocs.yaml` the configuration for Read the Docs
- `docs/requirements.txt` the packages needed by MkDocs
   when generating the documentation at Read the Docs.
   Locally needed packages are configured in `Pipfile`

Read the Docs automatically generates the MkDocs documentation after each `git push`.

#### GitHub Documentation

The Markdown documentation for GitHub are the files [README.md](https://github.com/Release-Candidate/LunaZodiaco-calendar/blob/main/README.md) and [CHANGELOG.md](https://github.com/Release-Candidate/LunaZodiaco-calendar/blob/main/CHANGELOG.md) in the project root directory.

## Source Code

Before you can use the configured Tools of this project, you have to download and install (aka. `restore`) the packages
of the tools.

1. First, download and install the "dotnet tools" using the command

```shell
dotnet tool restore
```

now you have installed Fake, Paket and FSharpLint,
configured in the file [`.config/dotnet-tools.json`](https://github.com/Release-Candidate/LunaZodiaco/blob/main/.config/dotnet-tools.json)

2. Download and install ("restore") the Paket Nuget packages.

```shell
dotnet paket restore
```

3. Delete the file `build.fsx.lock` and run Fake, to download and install (restore) it's nuget packages.

```shell
dotnet fake run build.fsx
```

4. To generate the documentation using MkDocs, a virtual Python environment is needed. A virtual Python
environment is the same as the locally installed Nuget packages above.
So, first you need to install Python, if you don't
have it installed already - either from your distributions repository, using the XCode or [Homebrew](https://brew.sh/) version,
or getting it from [Python.org](https://www.python.org/downloads/).
In the file [`Pipfile`](https://github.com/Release-Candidate/LunaZodiaco/blob/main/Pipfile) there is a stanza saying

```ini
[requires]
python_version = "3.9"
```

  That's just because I used 3.9 when generating that
documentation, and Pipenv is picky about the version mentioned
there. Just edit that to match your installed
Python version.
Install `pipenv` using the package
manager pip

```shell
pip install pipenv
```

Now you're ready to download and install the needed packages using pipenv

```shell
pipenv install --dev
```

### Source Code and Tests

The sources of the projects are located in the directors `src`, the tests in `tests`. [Fabulous](https://fsprojects.github.io/Fabulous/) is used as the F# MVU wrapper of Xamarin forms.

#### Source

There are 4 projects in LunaZodiaco:

- `LunaZodiacoDate` - the F# library and NuGet package that does the actual calculations, see [NUget Package](./nuget_package.md) for usage information.
- `LunaZodiaco` - the LunaZodiaco smartphone app, the platform independent part of the smartphone app.
- `LunaZodiaco.iOS` - the iOS specific part of the smartphone app.
- `LunaZodiaco.Android` - the Android specific part of the app.

#### Test Source

The project `TestLunaZodiaco` in the directory `tests/TestLunaZodiaco`: tests the NuGet package library, the project `LunaZodiacoDate`.
Files:

- `TestMain.fs` - Expecto's main entry point when called as an executable and not by using `dotnet test`.
- `Generics.fs` - generic functions used by the actual test functions.
- `TestLunaZodiacoNumber.fs` - tests the module `LunaZodiacoNumber`
- `TestLunaZodiacoGlyph.fs` - tests the module `LunaZodiacoGlyph`
- `TestLunaZodiacoDate.fs` - tests the module `LunaZodiacoDate`

### Build Targets

The Nuget package LunaZodiaco and the tests can be build using Fake, for anything smartphone related you need to use Visual Studio or Monodevelop, I did not succeed porting these projects to Paket and Fake.

- NuGet packages generated by the target `Packages` are saved to the directory `nuget`, set using the expression
  `packageOutputPath` in [`build.fsx`](https://github.com/Release-Candidate/LunaZodiaco/blob/main/build.fsx).
- The test results of the targets `Tests` and `TestsDeb` are saved as  `.xml` files in the
  directory `test_results`, set using the expression  `testOutputPath` in [`build.fsx`](https://github.com/Release-Candidate/LunaZodiaco/blob/main/build.fsx).
- The coverage report `coverage.xml` of the targets `TestsCoverage` and `TestsCoverageDeb`
  is saved to the directory `test_results`, set using the expression  `testOutputPath`
  in [`build.fsx`](https://github.com/Release-Candidate/LunaZodiaco/blob/main/build.fsx).
- The output of the `Publish` target is copied to the directory `bin`, set using the expression  `exeOutPath` in [`build.fsx`](https://github.com/Release-Candidate/LunaZodiaco/blob/main/build.fsx).

!!! Warning

    All of these directories are cleaned - that means all files in them are deleted - using the target `clean`.
    Which is called as the first build step of all targets. In other words: all generated
    files (including Nuget packages) are deleted when calling any target.

The Fake script [`build.fsx`](https://github.com/Release-Candidate/LunaZodiaco/blob/main/build.fsx) defines the following targets:

- `Clean` deletes all files generated by the build. **This is called first in every other target, all generated files are deleted before running any target!**
- `Distclean` deletes everything that isn't checked in to Git
- `Build` and `BuildDeb` build the configured projects
   using `dotnet build`. Without suffix a release build is made, `BuildDeb` uses the
   debug configuration. Default is to build all projects found in the directories
   `src` and `tests`. The expression `buildProjs` in [`build.fsx`](https://github.com/Release-Candidate/LunaZodiaco/blob/main/build.fsx) defines
   the projects to build.
- `Docs` generates the HTML documentation using MkDocs. The HTML is created in the directory `sites`.
  The output directory is configured in the MkDocs config file [`mkdocs.yml`](https://github.com/Release-Candidate/LunaZodiaco/blob/main/mkdocs.yml)
- `Lint` runs FSharpLint on all configured projects. Default is all projects, `lintProjs` in [`build.fsx`](https://github.com/Release-Candidate/LunaZodiaco/blob/main/build.fsx)
  defines the projects to run `Lint` on.
- `Tests` and `TestsDeb` build and run all test projects in the directory `tests` using `dotnet test`.
  Without suffix a release build is made, `TestsDeb` uses the  debug configuration.
  Default is testing all projects in the directory `tests`, `testProjs` in [`build.fsx`](https://github.com/Release-Candidate/LunaZodiaco/blob/main/build.fsx) defines the projects to run `Tests` and `TestsDeb` on.
- `TestsCoverage` and `TestsCoverageDeb`  build and run all tests projects and generate a coverage report using AltCover.
  With and without suffix a debug configuration is build and tested.
  Default is testing all projects in the directory `tests`, `testProjs` in [`build.fsx`](https://github.com/Release-Candidate/LunaZodiaco/blob/main/build.fsx) defines the projects to run `TestsCoverage` and `TestsCoverageDeb` on.
- `Publish` runs `dotnet publish` on the configured projects, default is all in `src`. The
  build is done with the release configuration, the version **must** be given as argument on the command line
  and be the same as the newest one in [CHANGELOG.md](https://github.com/Release-Candidate/LunaZodiaco/blob/main/CHANGELOG.md).
  `publishProjs` in [`build.fsx`](https://github.com/Release-Candidate/LunaZodiaco/blob/main/build.fsx) defines the projects to run `Publish` on.
  Published binaries are saved to the directory `bin`.
- `Packages` builds the NuGet packages of all projects using a release build. The
  build is done with the release configuration, the version **must** be given as argument on the command line
  and be the same as the newest one in [CHANGELOG.md](https://github.com/Release-Candidate/LunaZodiaco/blob/main/CHANGELOG.md).
  `packageProjs` in [`build.fsx`](https://github.com/Release-Candidate/LunaZodiaco/blob/main/build.fsx) defines the projects to run `Packages` on.
  Packages are saved to the directory `packages`
- `Upload` uploads all packages in the directory `packages` to NuGet.org. The NuGet API key
  needs to be saved to the configuration using

```shell
nuget setApiKey API_KEY
```

- `Release` is a pseudo-target, the same as `Build`, `Docs`, `Publish`, `Packages` and `Upload`. The
  build is done with the release configuration, the version **must** be given as argument on the command line
  and be the same as the newest one in [CHANGELOG.md](https://github.com/Release-Candidate/LunaZodiaco/blob/main/CHANGELOG.md).
- `All` is a pseudo-target, the same as `Build` and `Docs`,

Usage:

```shell
dotnet fake run build.fsx target TARGET VERSION_STRING
```

- `TARGET` is the name of the target to call, see above list for possible targets
- `VERSION_STRING` is the version to use for the target, **must** be the same as the first (newest)
  version in the file [`CHANGELOG.md`](https://github.com/Release-Candidate/LunaZodiaco/blob/main/CHANGELOG.md). Targets that need `VERSION_STRING` are
  `Packages`, `Publish`, `Upload` and `Release`.

Run all default targets (see above):

```shell
dotnet fake run build.fsx
```

Which is the same as

```shell
dotnet fake run build.fsx target All
```

E.g. to build a debug version of all projects:

```shell
dotnet fake run build.fsx target BuildDeb
```

Generate the Nuget packages of version `2.65.93` and upload to [NuGet.org](https://www.nuget.org/)

```shell
dotnet fake run build.fsx target Upload 2.65.93
```

Run `dotnet publish` on the configured projects (`publishProjs` in [`build.fsx`](https://github.com/Release-Candidate/LunaZodiaco/blob/main/build.fsx))  setting the
version to 6.2.53

```shell
dotnet fake run build.fsx target Publish 6.2.53
```

Run the tests - result is in the directory `test_results`:

```shell
dotnet fake run build.fsx target Tests
```

Run the tests with coverage analysis - result is in the directory `test_results`:

```shell
dotnet fake run build.fsx target TestsCoverage
```

## CodeCov Configuration

Generate an account at [CodeCov](https://about.codecov.io/), link your GitHub repositories, add the CodeCov token
as a secret to the GitHub repositories named `CODECOV_SECRET` and upload the coverage result
using a GitHub action. Used in the workflows [linux_test.yml](https://github.com/Release-Candidate/LunaZodiaco/blob/main/.github/workflows/linux_test.yml),
[osx_test.yml](https://github.com/Release-Candidate/LunaZodiaco/blob/main/.github/workflows/osx_test.yml) and [windows_test.yml](https://github.com/Release-Candidate/LunaZodiaco/blob/main/.github/workflows/windows_test.yml)

```yml
      - name: Run Tests
        run: |
          cd $GITHUB_WORKSPACE
          dotnet fake run ./build.fsx target TestsCoverage

      - name: Upload coverage to Codecov
        uses: codecov/codecov-action@v1
        if: ${{ always() }}
        with:
          token: ${{ secrets.CODECOV_SECRET }}
          files: ./test_results/coverage.xml
          directory: ./coverage/reports/
          # flags: unittest
          env_vars: OS,PYTHON
          name: Linux-Test-Src
          fail_ci_if_error: false
          path_to_write_report: ./coverage/codecov_report.txt
          verbose: true
```

## GitHub Workflows

All tests and builds are executed on Linux,
Mac OS X and Windows.

These are the GitHub workflows defined in the directory `.github/workflows`

- `create_packages.yml` creates and uploads the NuGet packages, runs `dotnet publish` and
  generates a new GitHUb release with these files appended. Run automatically after tagging
  the source with a release tag of the form `v?.?.?` (**must be the same version as the newest in [CHANGELOG.md](https://github.com/Release-Candidate/LunaZodiaco/blob/main/CHANGELOG.md)**).
  Appends the newest entry in [CHANGELOG.md](https://github.com/Release-Candidate/LunaZodiaco/blob/main/CHANGELOG.md) to the release - script [`scripts/get_changelog.sh`](https://github.com/Release-Candidate/LunaZodiaco/blob/main/scripts/get_changelog.sh)
  See the [latest release](https://github.com/Release-Candidate/LunaZodiaco/releases/latest) as an example
- `linux_test.yml` runs the tests and coverage tests on Linux, uploads the test results as artifacts,
  uploads the coverage results to CodeCov.
- `osx_test.yml` runs the tests and coverage tests on Mac OS X, uploads the test results as artifacts,
  uploads the coverage results to CodeCov.
- `windows_test.yml` runs the tests and coverage tests on Windows, uploads the test results as artifacts,
  uploads the coverage results to CodeCov.

The badges of the workflows are linked in the section [Badges](https://github.com/Release-Candidate/LunaZodiaco#badges )

## GitHub Issue Template

Issue templates for GitHub in `.github/ISSUE_TEMPLATE/`

- `bug_report.md` Bug report template
- `feature_request.md` Feature request template
