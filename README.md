### Change Log

[ChangeLog](src/Greta.BO.Api/CHANGELOG.md)


### Utils commands

Add-Migration <name> -o Data
Update-Database

dotnet tool install --global dotnet-ef --version 5.0.3

dotnet ef migrations add Initial -p src/Greta.BO.Api -o Data
dotnet ef database update -p src/Greta.BO.Api

2fa library documentation
https://github.com/NickyBall/GoogleAuthenticatorService.Core


Docfx

docker run --mount type=bind,source="$(pwd)",target=/work nullreference/docfx-docker /work/docfx.json


sonnar compose
version: '2'

services:
my-sonar:
image: davealdon/sonarqube-with-docker-and-m1-macs
#      platform: linux/amd64
      ports:
        - 9001:9000

Change log generator
dotnet tool install --global Versionize

- dry run
    versionize -w ./src/Greta.BO.Api/ -d -i

- prerelease
    versionize -i -w ./src/Greta.BO.Api/ --pre-release alpha

- Normal Run with Agregate all prerelease data
    versionize -i -w ./src/Greta.BO.Api/ --aggregate-pre-releases