export PATH :=  home_directory() + "/.dotnet/tools:" + "./node_modules/.bin:" + env_var('PATH')

default:
    @just publish

[confirm("Do you want to clean the project [Y/N]:")]
clean:
    @echo "Cleaing project"
    dotnet clean
    rm -rf node_modules

build-css:
    tailwindcss -i ./src/Styles/index.css -o ./src/wwwroot/css/index.css

build-ts:
    tsc

build-js:
    @just build-ts
    @just build-css

build-server:
    dotnet build

build:
    @just build-js
    @just build-server

publish:
    @just restore
    @just build
    dotnet publish -c Release

restore:
    @echo 'Resotring project'
    dotnet tool restore
    dotnet restore
    libman restore
    npm install

watch:
    #!/usr/bin/env bash
    tsc -w --preserveWatchOutput  &
    tailwindcss -i ./src/Styles/index.css -o ./src/wwwroot/css/index.css --watch &
    dotnet watch --project src/Wikirace.csproj



run:
    dotnet run

kill-server:
    pkill Wikirace

migrate name:
    dotnet ef migrations add --project=src {{uppercamelcase(name)}} -o src/Data/Migrations
    @just update-db

update-db:
    dotnet ef database update --project=src