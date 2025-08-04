# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/API/PixelzEcommerce.Api/PixelzEcommerce.Api.csproj", "src/API/PixelzEcommerce.Api/"]
COPY ["src/Shared/PixelzEcommerce.Shared.Presentation/PixelzEcommerce.Shared.Presentation.csproj", "src/Shared/PixelzEcommerce.Shared.Presentation/"]
COPY ["src/Shared/PixelzEcommerce.Shared.Infrastructure/PixelzEcommerce.Shared.Infrastructure.csproj", "src/Shared/PixelzEcommerce.Shared.Infrastructure/"]
COPY ["src/Shared/PixelzEcommerce.Shared.Application/PixelzEcommerce.Shared.Application.csproj", "src/Shared/PixelzEcommerce.Shared.Application/"]
COPY ["src/Shared/PixelzEcommerce.Shared.Domain/PixelzEcommerce.Shared.Domain.csproj", "src/Shared/PixelzEcommerce.Shared.Domain/"]
COPY ["src/Modules/Orders/PixelzEcommerce.Modules.Orders.Infrastructure/PixelzEcommerce.Modules.Orders.Infrastructure.csproj", "src/Modules/Orders/PixelzEcommerce.Modules.Orders.Infrastructure/"]
COPY ["src/Modules/Orders/PixelzEcommerce.Modules.Orders.Application/PixelzEcommerce.Modules.Orders.Application.csproj", "src/Modules/Orders/PixelzEcommerce.Modules.Orders.Application/"]
COPY ["src/Modules/Orders/PixelzEcommerce.Modules.Orders.Domain/PixelzEcommerce.Modules.Orders.Domain.csproj", "src/Modules/Orders/PixelzEcommerce.Modules.Orders.Domain/"]
COPY ["src/Modules/Orders/PixelzEcommerce.Modules.Orders.IntegrationEvents/PixelzEcommerce.Modules.Orders.IntegrationEvents.csproj", "src/Modules/Orders/PixelzEcommerce.Modules.Orders.IntegrationEvents/"]
COPY ["src/Modules/Orders/PixelzEcommerce.Modules.Orders.Presentation/PixelzEcommerce.Modules.Orders.Presentation.csproj", "src/Modules/Orders/PixelzEcommerce.Modules.Orders.Presentation/"]
COPY ["src/Modules/Products/PixelzEcommerce.Modules.Products.IntegrationEvents/PixelzEcommerce.Modules.Products.IntegrationEvents.csproj", "src/Modules/Products/PixelzEcommerce.Modules.Products.IntegrationEvents/"]
COPY ["src/Modules/Products/PixelzEcommerce.Modules.Products.Infrastructure/PixelzEcommerce.Modules.Products.Infrastructure.csproj", "src/Modules/Products/PixelzEcommerce.Modules.Products.Infrastructure/"]
COPY ["src/Modules/Products/PixelzEcommerce.Modules.Products.Application/PixelzEcommerce.Modules.Products.Application.csproj", "src/Modules/Products/PixelzEcommerce.Modules.Products.Application/"]
COPY ["src/Modules/Products/PixelzEcommerce.Modules.Products.Domain/PixelzEcommerce.Modules.Products.Domain.csproj", "src/Modules/Products/PixelzEcommerce.Modules.Products.Domain/"]
COPY ["src/Modules/Products/PixelzEcommerce.Modules.Products.Presentation/PixelzEcommerce.Modules.Products.Presentation.csproj", "src/Modules/Products/PixelzEcommerce.Modules.Products.Presentation/"]
RUN dotnet restore "./src/API/PixelzEcommerce.Api/PixelzEcommerce.Api.csproj"
COPY . .
WORKDIR "/src/src/API/PixelzEcommerce.Api"
RUN dotnet build "./PixelzEcommerce.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PixelzEcommerce.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PixelzEcommerce.Api.dll"]