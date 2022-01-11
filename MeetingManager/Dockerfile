#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 44303
EXPOSE 445
EXPOSE 5416
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MeetingManager/MeetingManager.csproj", "MeetingManager/"]
RUN dotnet restore "MeetingManager/MeetingManager.csproj"
COPY . .
WORKDIR "/src/MeetingManager"
RUN dotnet build "MeetingManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MeetingManager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MeetingManager.dll"]
