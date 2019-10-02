FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
# copy necessary files
COPY . .

# Restore all the dependencoes
WORKDIR /app
RUN dotnet restore
WORKDIR /app
RUN dotnet publish -f netcoreapp2.2 -c Release -o out

FROM microsoft/dotnet:2.2-runtime
LABEL version=ttf1.0
WORKDIR /app

COPY --from=build /app/out /app
EXPOSE 8088/tcp
ENTRYPOINT ["dotnet", "TTF-Printer.dll"]