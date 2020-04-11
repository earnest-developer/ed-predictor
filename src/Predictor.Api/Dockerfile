FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

# copy csproj and restore as distinct layers
COPY ./src/Predictor.Api/Predictor.Api.csproj ./src/Predictor.Api/
COPY ./src/Predictor/Predictor.csproj ./src/Predictor/
COPY ./extensions/DependencyInjection/DependencyInjection.csproj ./extensions/DependencyInjection/
RUN dotnet restore src/Predictor.Api/Predictor.Api.csproj

# copy everything else and build
COPY ./src/ ./
COPY ./extensions/ ./extensions/
WORKDIR /Predictor.Api/
RUN dotnet publish -c $BUILDCONFIG -o out /p:Version=$VERSION

# build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app
COPY --from=build /Predictor.Api/out ./

EXPOSE 5000
ENTRYPOINT ["dotnet", "Predictor.Api.dll"]