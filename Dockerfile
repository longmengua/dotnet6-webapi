# 使用 .NET 6 SDK 映像進行構建
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /App

# 複製所有檔案
COPY . ./
# 還原 NuGet 套件
RUN dotnet restore
# 構建並發佈應用程序
RUN dotnet publish -c Release -o out

# 使用 .NET 6 ASP.NET 映像進行運行時構建
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App

# 複製從 build 階段生成的發佈文件
COPY --from=build /App/out .

# Set environment variables to use HTTP and specify the port
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# 暴露容器的 80 端口，讓 Web API 可從外部訪問
EXPOSE 80

# 設定容器啟動時的命令
ENTRYPOINT ["dotnet", "dotnet6-webapi.dll"] 
