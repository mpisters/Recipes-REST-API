FROM mcr.microsoft.com/dotnet/sdk:5.0.202-focal-amd64
COPY . /app
WORKDIR /app
ENV ASPNETCORE_URLS=https://localhost:5001;http://localhost:5000
ENV ASPNETCORE_ENVIRONMENT=Development
RUN dotnet restore
RUN dotnet build --no-restore
EXPOSE 5001 5000
ENTRYPOINT ["dotnet", "RecipesAPI/bin/Debug/net5.0/RecipesAPI.dll","--urls", "https://0.0.0.0:5000"]


