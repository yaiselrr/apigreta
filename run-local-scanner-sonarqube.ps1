# Vars
$path = ".\tests\Greta.BO.Api.Test"
$project = "$path\Greta.BO.Api.Test.csproj"
$report = "$path\coverage.opencover.xml"
$sonarkey = "Greta.BO.Api"
$sonarid = "eebc1c24c333d2825c414b5e69610aa95a4e3357"

#dotnet test $project /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
# dotnet test $project `
#     /p:CollectCoverage=true `
#     /p:CoverletOutputFormat=opencover `
#     /p:Exclude="[Greta.BO.InMemory]*%2c[Greta.BO.Api.Abstractions]*%2c[Greta.BO.Api.Entities]*%2c[Greta.BO.Api.Sqlserver]*%2c[Greta.BO.Api.BusinessLogic]*"
    
#     dotnet test tests/Greta.BO.Api.Test/Greta.BO.Api.Test.csproj  /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:Exclude="[Greta.BO.InMemory]*%2c[Greta.BO.Api.Abstractions]*%2c[Greta.BO.Api.Entities]*%2c[Greta.BO.Api.Sqlserver]*%2c[Greta.BO.Api.BusinessLogic]*" --results-directory ./coverage

# dotnet test tests/Greta.BO.Api.Test/Greta.BO.Api.Test.csproj --configuration Release --no-build --verbosity normal --collect:"XPlat Code Coverage" --results-directory ./coverage

#dotnet sonarscanner begin `
#    /k:"$sonarkey" `
#    /d:sonar.host.url=http://sonarqube:9000 `
#    /d:sonar.login="$sonarid" `
#    /d:sonar.cs.opencover.reportsPaths="$report" `
#    /d:sonar.coverage.exclusions="**/Greta.BO.InMemory/**/*.cs,**Test*.cs,**/Greta.BO.Api.Entities/**/*.cs,**/Greta.BO.Api.Sqlserver/**/*.cs,**/Greta.BO.Api.Abstractions/**/*.cs" `
#    /d:sonar.exclusions="**/Greta.Sdk.InMemory/**/*.cs,**/Greta.BO.Api.Entities/**/*.cs,**/Greta.BO.Api.Sqlserver/**/*.cs,**/Greta.BO.Api.Abstractions/**/*.cs"

# dotnet sonarscanner begin `
#     /k:"$sonarkey" `
#     /d:sonar.host.url=http://sonarqube:9000 `
#     /d:sonar.login="$sonarid" `
#     /d:sonar.cs.opencover.reportsPaths="$report" `
#    /d:sonar.coverage.exclusions="**Test*.cs,**/Greta.BO.Api/Data/*.cs" `
#    /d:sonar.exclusions=",**/Greta.BO.Api/Data/*.cs"
# 
# dotnet build 
# 
# dotnet sonarscanner end /d:sonar.login="$sonarid"

#dotnet tool install --global dotnet-sonarscanner


dotnet64 test $project \
    /p:CollectCoverage=true \
    /p:CoverletOutputFormat=opencover \
    /p:Exclude="[Greta.BO.InMemory]*%2c[Greta.BO.Api.Abstractions]*%2c[Greta.BO.Api.Entities]*%2c[Greta.BO.Api.Sqlserver]*%2c[Greta.BO.Api.BusinessLogic]*"
    

# dotnet64 tool install --global dotnet-sonarscanner
dotnet64 sonarscanner begin \
    /k:"Greta.BO" \
    /d:sonar.login="8e9a8e3a7607d8fdac7c051123b2239d1369ebdd" \
    /d:sonar.host.url=http://localhost:9001 \
    /d:sonar.sonar.sourceEncoding=UTF-8 \
    /d:sonar.sonar.projectBaseDir="$(pwd)"/src/ \
    /d:sonar.exclusions="**/Greta.Sdk.InMemory/**/*.cs,**/Greta.BO.Api.Entities/**/*.cs,**/Greta.BO.Api.Sqlserver/**/*.cs,**/Greta.BO.Api.Abstractions/**/*.cs" \
    /d:sonar.coverage.exclusions="**/Greta.BO.InMemory/**/*.cs,**Test*.cs,**/Greta.BO.Api.Entities/**/*.cs,**/Greta.BO.Api.Sqlserver/**/*.cs,**/Greta.BO.Api.Abstractions/**/*.cs" \
    /d:sonar.cs.opencover.reportsPaths=".\tests\Greta.BO.Api.Test\coverage.opencover.xml" 
    
dotnet64 build Greta.BO.sln
dotnet64 sonarscanner end /d:sonar.login="8e9a8e3a7607d8fdac7c051123b2239d1369ebdd"