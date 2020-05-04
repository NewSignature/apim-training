FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
WORKDIR /build

COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o ./publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as runtime
WORKDIR /app
COPY --from=build /build/publish .

EXPOSE 80
EXPOSE 443
ENTRYPOINT [ "dotnet", "PeopleApi.dll" ]