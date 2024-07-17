#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 1883

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/OpenTask.WebApi/OpenTask.WebApi.csproj", "OpenTask.WebApi/"]
COPY ["src/OpenTask.Application/OpenTask.Application.csproj", "OpenTask.Application/"]
COPY ["src/OpenTask.Core/OpenTask.Core.csproj", "OpenTask.Core/"]
COPY ["src/OpenTask.Domain/OpenTask.Domain.csproj", "OpenTask.Domain/"]
COPY ["src/OpenTask.Utility/OpenTask.Utility.csproj", "OpenTask.Utility/"]
COPY ["src/OpenTask.Persistence/OpenTask.Persistence.csproj", "OpenTask.Persistence/"]
RUN dotnet restore "OpenTask.WebApi/OpenTask.WebApi.csproj"
COPY src .
WORKDIR "/src/OpenTask.WebApi"
RUN dotnet build "OpenTask.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OpenTask.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ui
FROM registry.cn-hangzhou.aliyuncs.com/hgui/node:20.12.0 AS ui
WORKDIR /app
COPY ["ui/vite-opentask", "./"]
RUN rm -rf package-lock.json node_modules
#RUN npm config set registry https://registry.npmmirror.com
RUN npm i
RUN npm run build

FROM base AS final
WORKDIR /app

ENV TZ=Asia/Shanghai

RUN chmod 777 /app
RUN chmod 777 /app/logs
COPY --from=publish /app/publish .
COPY --from=ui /app/dist ./wwwroot
ENTRYPOINT ["dotnet", "OpenTask.WebApi.dll"]