export PATH :=  home_directory() + "/.dotnet/tools:" + "./node_modules/.bin:" + env_var('PATH')

default:
    @just publish

[confirm("Do you want to clean the project [Y/N]:")]
clean:
    @echo "Cleaing project"
    dotnet clean
    rm -rf node_modules


[unix]
toolchain:
    @echo "Checking toolchain"
    @command -v dotnet >/dev/null 2>&1 || { just install_toolchain; exit 0; } 
    @command -v node >/dev/null 2>&1 || { just install_toolchain; exit 0; }
    @command -v npm >/dev/null 2>&1 || { just install_toolchain; exit 0; }
    @echo "All tools are installed"

build:
    tsc
    dotnet build

publish:
    @just restore
    @just build
    dotnet publish -c Release

restore:
    @just toolchain
    @echo 'Resotring project'
    dotnet tool restore
    dotnet restore
    libman restore
    npm install

watch:
    #!/usr/bin/env bash
    tsc --watch &
    dotnet watch

[linux]
[private]
[confirm("Some tools are not installed do you want to install them? [Y/n]:")]
install_toolchain:
    #!/usr/bin/env bash
    source /etc/os-release
    if [[ "$ID" == "fedora" ]]; then
        echo "On fedora using dnf"
        pkexec dnf install dotnet-sdk-8.0 nodejs
    else
        echo "Toolchain installation is not suported on $ID"
        exit 1
    fi

run:
    dotnet run

kill-server:
    pkill Wikirace