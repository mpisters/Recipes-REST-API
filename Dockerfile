FROM mcr.microsoft.com/dotnet/sdk:5.0.202-focal-amd64
COPY . /app
WORKDIR /app
ENV ASPNETCORE_URLS=https://127.0.0.1:5001;http://127.0.0.1:5000
ENV ASPNETCORE_ENVIRONMENT=Development
RUN dotnet restore
RUN dotnet build --no-restore
EXPOSE 5001 5000
ENTRYPOINT ["dotnet", "RecipesAPI/bin/Debug/net5.0/RecipesAPI.dll"]


