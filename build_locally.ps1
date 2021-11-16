new-item .\_build\_package\app\wwwroot\ -itemtype directory -Force

# dotnet paths are relative
dotnet publish .\backend\Gim.PriceParser.WebApi -c Release -o _build\_package\app

Set-Location .\frontend\price-parser\
npm install --silent
npm run build --production
Set-Location ..\..\
Copy-Item ".\frontend\price-parser\build\*" -Destination ".\_build\_package\app\wwwroot\" -Recurse -Force

docker build -t price-parser --no-cache _build
