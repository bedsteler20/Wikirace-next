
PATH := node_modules/.bin:$(PATH)
PATH := $(HOME)/.dotnet/tools:$(PATH) 

watch:
	make -j 4 watch-tailwind watch-tailstrap watch-ts watch-dotnet

watch-ts:
	node ./esbuild.config.mjs --watch

watch-tailwind:
	tailwindcss -i ./src/Styles/index.css -o ./src/wwwroot/lib/wikirace/index.css --watch

watch-tailstrap:
	tailwindcss -i ./src/Styles/tailstrap.css -o ./src/wwwroot/lib/wikirace/tailstrap.css --watch

watch-dotnet:
	dotnet watch --project ./src/Wikirace.csproj

build:
	make build-ts build-tailwind build-dotnet build-tailstrap

build-ts:
	node ./esbuild.config.mjs

build-tailwind:
	tailwindcss -i ./src/Styles/index.css -o ./src/wwwroot/lib/wikirace/index.css

build-tailstrap:
	tailwindcss -i ./src/Styles/tailstrap.css -o ./src/wwwroot/lib/wikirace/tailstrap.css

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

publish:
	make restore
	make build-ts
	make build-tailwind
	make build-tailstrap
	dotnet publish ./src/Wikirace.csproj -c Release