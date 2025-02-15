# dotnet6-webapi

## self learn c# for webapi

- background
    - backend skills => java, golang, c, node
    - frontend skills => react, vue

## Oauth and JWT

- Installation
    - dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 6.0.1
    - dotnet add package Microsoft.IdentityModel.Tokens
    - dotnet add package System.IdentityModel.Tokens.Jwt
    - dotnet add package Microsoft.AspNetCore.Authentication.OAuth

## Testing 

- dotnet add package xunit
    - 單元測試框架，用來撰寫和執行測試案例
    - 單元測試（Unit Test）：測試 Controller、Service、Repository。
    - 整合測試（Integration Test）：結合 WebApplicationFactory 來測試整個 API。

- dotnet add package Microsoft.AspNetCore.Mvc.Testing
    - 提供 WebApplicationFactory<T>，用來啟動一個 測試版的 Web API，讓測試可以直接呼叫 API 進行整合測試
    - 整合測試（Integration Test）：測試 API 端點是否能夠正確運行。
    - 模擬真正的 HTTP 請求，確保 API 可用性。


## phases

- phase 1
    - create a project with the command
        - dotnet new webapi -n dotnet6-webapi 

- phase 2
    - remove IIS of launchSetting, coz it will be run in linux.

- phase 3
    - change log mechanism to integrate with ELK

- phase 4
    - middleware for exception capture 

- phase 5
    - graceful shutdown
        - test
            - Use the command `dotnet run` to launch the app.
            - Then use `Ctrl + C` to stop the app and check if the graceful shutdown is triggered.

- phase 6
    - middleware for api timeout management 
        - api timeout management by CancellationTokenSource

- phase 7
    - jwt, SSO, encryptiopn

- phase 8
    - perfoamnce testing + loading testing

- phase 9
    - redis + lua 