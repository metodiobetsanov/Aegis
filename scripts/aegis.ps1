dotnet tool install --global coverlet.console
dotnet restore
dotnet build --no-incremental --no-restore
coverlet .\tests\Aegis.UnitTests\bin\Debug\net7.0\Aegis.UnitTests.dll --target "dotnet" --targetargs "test --no-build" -f=opencover -o="coverage.xml"