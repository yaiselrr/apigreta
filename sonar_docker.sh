#!/bin/sh

export JAVA_HOME=/Users/chenry/tmp/sonar-scanner-4.7.0.2747-macosx/jre

SONARQUBE_URL="localhost:9001"
PROJECT_KEY="YFZ3LWYFDRAOGJF5LIZH"
LOGIN="2e209e43cb700c75c918674cd3864ca231e0adfb"

#dotnet test tests/Greta.BO.Api.Test/Greta.BO.Api.Test.csproj  /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:Exclude="[Greta.BO.InMemory]*%2c[Greta.BO.Api.Abstractions]*%2c[Greta.BO.Api.Entities]*%2c[Greta.BO.Api.Sqlserver]*%2c[Greta.BO.Api.BusinessLogic]*"
#
#docker run \
#  --rm \
#  --net host \
#  -e SONAR_HOST_URL="http://${SONARQUBE_URL}" \
#  -v "$(pwd)":/usr/src  \
#  sonarsource/sonar-scanner-cli \
#  -Dsonar.projectKey="${PROJECT_KEY}" \
#      -Dsonar.sonar.sourceEncoding=UTF-8 \
#      -Dsonar.sonar.host.url=http://"${SONARQUBE_URL}" \
#      -Dsonar.login="${LOGIN}" \
#      -Dsonar.sonar.projectBaseDir=/usr/src/ \
#      -Dsonar.exclusions=",**/Greta.BO.Api/Data/*.cs" \
#      -Dsonar.coverage.exclusions="**Test*.cs,**/Greta.BO.Api/Data/*.cs" \
#      -Dsonar.cs.opencover.reportsPaths=".\tests\Greta.BO.Api.Test\coverage.opencover.xml" 

#export SONARQUBE_PATH="${HOME}/bin/sonarqube-9.4.0.54424/bin/macosx-universal-64/sonar.sh"
#export SONAR_SCANNER_PATH="${HOME}/bin/sonar-scanner-4.7.0.2747-macosx/bin/sonar-scanner"


current_dir="$(basename $(pwd))"
current_commit="$(git rev-parse --short HEAD)"

echo "token=${LOGIN}"
echo "current_dir=${current_dir}"
echo "current_commit=${current_commit}"
echo ""

#"${SONAR_SCANNER_PATH}" \
#-Dsonar.host.url=http://"${SONARQUBE_URL}" \
#-Dsonar.login="${LOGIN}" \
#-Dsonar.projectKey="${current_dir}" \
#-Dsonar.projectName="${current_dir}" \
#-Dsonar.projectVersion="${current_commit}" \
#-Dsonar.sonar.sourceEncoding=UTF-8 \
#-Dsonar.sonar.projectBaseDir="$(pwd)"/src/ \
#-Dsonar.exclusions="**/Greta.Sdk.InMemory/**/*.cs,**/Greta.BO.Api.Entities/**/*.cs,**/Greta.BO.Api.Sqlserver/**/*.cs,**/Greta.BO.Api.Abstractions/**/*.cs" \
#-Dsonar.coverage.exclusions="**/Greta.BO.InMemory/**/*.cs,**Test*.cs,**/Greta.BO.Api.Entities/**/*.cs,**/Greta.BO.Api.Sqlserver/**/*.cs,**/Greta.BO.Api.Abstractions/**/*.cs" \
#-Dsonar.cs.opencover.reportsPaths=".\tests\Greta.BO.Api.Test\coverage.opencover.xml" 


#dotnet test tests/Greta.BO.Api.Test/Greta.BO.Api.Test.csproj  \
#/p:CollectCoverage=true /p:CoverletOutputFormat=opencover \
#/p:Exclude="[Greta.BO.InMemory]*%2c[Greta.BO.Api.Abstractions]*%2c[Greta.BO.Api.Entities]*%2c[Greta.BO.Api.Sqlserver]*%2c[Greta.BO.Api.BusinessLogic]*"

dotnet64 test Greta.BO.sln \
    /p:CollectCoverage=true \
    /p:CoverletOutputFormat=opencover  \
    /p:Exclude="[Greta.BO.Api.Abstractions]*%2c[Greta.BO.Api.Entities]*%2c[Greta.BO.Api.Sqlserver]*%2c[Greta.BO.Api]*"

#dotnet64 tool install --global dotnet-sonarscanner
dotnet64 sonarscanner begin \
    /k:"Greta.BO" \
    /d:sonar.login="${LOGIN}" \
    /d:sonar.host.url=http://"${SONARQUBE_URL}" \
    /d:sonar.sonar.sourceEncoding=UTF-8 \
#    /d:sonar.sources="$(pwd)"/src/ \
    /d:sonar.sources=src/ \
    /d:sonar.tests=tests/ \
    /d:sonar.exclusions="**/*Test*.cs,**/Greta.BO.Api.Entities/**/*.cs,**/Greta.BO.Api.Sqlserver/**/*.cs,**/Greta.BO.Api.Abstractions/**/*.cs,src/Greta.BO.Api/**/*.cs,**/Greta.BO.Api.EventContracts/**/*.cs" \
    /d:sonar.coverage.exclusions="**/*Test*.cs,**/Greta.BO.Api.Entities/**/*.cs,**/Greta.BO.Api.Sqlserver/**/*.cs,**/Greta.BO.Api.Abstractions/**/*.cs,**/Greta.BO.Api/**/*.cs" \
    /d:sonar.cs.opencover.reportsPaths=".\tests\Greta.BO.Api.Test\coverage.opencover.xml" 
    
dotnet64 build Greta.BO.sln
dotnet64 sonarscanner end /d:sonar.login="${LOGIN}"