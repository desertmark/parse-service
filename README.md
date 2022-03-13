# Parse-Service

## Description

Parse Service is a .Net Core WebAPI intended to parse xls Excel files into JSON response.

## Build

Install Dotnet SDK
in /src directory:

1. dotnet restore
2. dotnet publish -c Release -o out
3. dotnet parse-service.dll

## Development

in /src `dotnet run`

## Docker

in /src

1. `docker build .`
2. `docker run -p 8080:80 <image-name/id>`
3. Test by sending a request to: `localhost:8080/parse/xls` method `POST` body `FormData` fields: `file` with an XLS file and filed `headerIndex` with row number (0-based) where the Header of the excel data starts.
