FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
ARG NEXUSUSERNAME=${NEXUSUSERNAME}
ARG NEXUSPASSWORD=${NEXUSPASSWORD}

RUN apt-get update -q && apt-get install -q -y \
    curl apt-transport-https apt-utils dialog gnupg ca-certificates \
    make g++ build-essential libgdiplus libx11-dev libc6-dev 
    
WORKDIR /app

#RUN dotnet nuget disable source "nuget.org"

RUN echo " value of ${NEXUSUSERNAME}"
# Adding nuget source
RUN dotnet nuget add source https://nexus.gretatest.com/repository/nuget-group/ --name Nexus -u ${NEXUSUSERNAME} -p ${NEXUSPASSWORD} --store-password-in-clear-text

# Copy csproj and restore as distinct layers
#COPY *.csproj ./

# Copy everything 
COPY . ./
#restore
#RUN dotnet restore -s "https://nexus.gretatest.com/repository/nuget-group/"
RUN dotnet restore 
#build
RUN dotnet publish -c Release -f net7.0 -o out src/Greta.BO.Api/Greta.BO.Api.csproj --property WarningLevel=0

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
RUN apt-get update -q && apt-get install -q -y \
    curl apt-transport-https apt-utils dialog gnupg ca-certificates \
    make g++ build-essential libgdiplus libx11-dev libc6-dev
WORKDIR /app
EXPOSE 5000
# RUN apt install libmagic-dev
ENV ASPNETCORE_URLS=http://*:5000
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Greta.BO.Api.dll"]