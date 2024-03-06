FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.19 AS build-env

RUN apk add --no-cache nodejs npm make

WORKDIR /source

COPY . ./

RUN --mount=type=cache,id=node-modules,target=/source/node_modules \
    npm install
RUN --mount=type=cache,id=node-modules,target=/source/node_modules \
    make build-ts
RUN --mount=type=cache,id=node-modules,target=/source/node_modules \
    make build-tailwind
RUN --mount=type=cache,id=node-modules,target=/source/node_modules \
    make build-tailstrap
RUN --mount=type=cache,id=dn-tools,target=/root/.dotnet/tools \
    dotnet tool restore
RUN --mount=type=cache,id=dn-tools,target=/root/.dotnet/tools \
    dotnet tool run libman restore
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -o /app

# RUN ls bin/Release/net8.0/publish && exit 1

# RUN make publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine3.19 AS final

WORKDIR /app

COPY --from=build-env /app .

ENTRYPOINT ["dotnet", "Wikirace.dll"]