#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Core/Core.csproj", "Core/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["Database/Database.csproj", "Database/"]
RUN dotnet restore "Core/Core.csproj"
COPY . .
WORKDIR "/src/Core"
RUN dotnet build "Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
RUN apt update
RUN apt install ruby-full -y
RUN apt install python -y
RUN apt install python-pip -y 
RUN pip install requests
RUN apt install nodejs npm -y
ENV MP_CONNECTIONSTRING="mongodb://192.168.87.107:27017"
ENV MP_COLLECTION="Scripts"
ENV MP_DATABASE="MapsPeople"
ENV MP_QUEUENAME="queue1"
ENV MP_INTERPRETERPATH="node"
ENV MP_MESSAGEBROKER="192.168.87.107" 
RUN mkdir -p /root/scripts/ruby
RUN mkdir -p /root/scripts/python
RUN mkdir -p /root/scripts/javascript       
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Core.dll"]