FROM mcr.microsoft.com/dotnet/sdk:5.0.202-focal-amd64
COPY . /app
WORKDIR /app
RUN dotnet restore
RUN dotnet build --no-restore
RUN dotnet publish -c release
RUN ls /app/RecipesAPI/bin/Release/
ENV ASPNETCORE_ENVIRONMENT="Production"
ENV ASPNETCORE_URLS="https://0.0.0.0:5000"
EXPOSE 5000
ENTRYPOINT ["dotnet", "/app/RecipesAPI/bin/Release/net5.0/RecipesAPI.dll"]


