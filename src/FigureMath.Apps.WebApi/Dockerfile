FROM mcr.microsoft.com/dotnet/sdk:5.0 AS source

# copy csproj and restore as distinct layers
COPY *.sln /build/

WORKDIR /build/src
COPY src/FigureMath.Apps/*.csproj FigureMath.Apps/
COPY src/FigureMath.Apps.WebApi/*.csproj FigureMath.Apps.WebApi/

COPY src/FigureMath.Apps.WebApi.Domain/*.csproj FigureMath.Apps.WebApi.Domain/

COPY src/FigureMath.Data/*.csproj FigureMath.Data/

COPY src/FigureMath.Common/*.csproj FigureMath.Common/
COPY src/FigureMath.Common.AspNet/*.csproj FigureMath.Common.AspNet/
COPY src/FigureMath.Common.Data/*.csproj FigureMath.Common.Data/

WORKDIR /build/src/FigureMath.Apps.WebApi
RUN dotnet restore FigureMath.Apps.WebApi.csproj

COPY src/ /build/src/

FROM source AS publish

WORKDIR /build/src/FigureMath.Apps.WebApi
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=publish /app ./
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "FigureMath.Apps.WebApi.dll"]