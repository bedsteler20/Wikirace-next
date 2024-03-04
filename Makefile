
PATH := node_modules/.bin:$(PATH)
PATH := $(HOME)/.dotnet/tools:$(PATH) 

watch:
	make -j 4 watch-tailwind watch-tailstrap watch-ts watch-dotnet

watch-ts:
	tsc --watch --preserveWatchOutput

watch-tailwind:
	tailwindcss -i ./src/Styles/index.css -o ./src/wwwroot/css/index.css --watch

watch-tailstrap:
	tailstrap -i ./src/Styles/tailstrap.css -o ./src/wwwroot/css/tailstrap.css --watch

watch-dotnet:
	dotnet watch --project ./src/Wikirace.csproj

build:
	make build-ts build-tailwind build-dotnet build-tailstrap

build-ts:
	tsc

build-tailwind:
	tailwindcss -i ./src/Styles/index.css -o ./src/wwwroot/css/index.css

build-tailstrap:
	tailstrap -i ./src/Styles/tailstrap.css -o ./src/wwwroot/css/tailstrap.css

build-dotnet:
	dotnet build ./src/Wikirace.csproj

update-database:
	dotnet ef database update --project ./src/Wikirace.csproj

migration:
	dotnet ef migrations add $(name) --project ./src/Wikirace.csproj	

restore:
	@echo "Restoring dependencies"
	dotnet restore ./src/Wikirace.csproj
	dotnet tool restore
	npm install