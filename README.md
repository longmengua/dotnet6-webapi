# dotnet6-webapi

## import

- 本專案全面設定成 http，因為理想的架構，前面應該有 gateway 處理 SSL 的問題(如：nginx)，故不用https。

## private docker push

- 建置後推送到私人docker hub，請參照 build-and-push.sh 
    - 如果 docker registry 未設定https ，默認為http，那在推送端需要對docker做推送調整。
    - 打開 Docker 的配置文件：
        - Linux
            - 於檔案 /etc/docker/daemon.json，加入
                - { "insecure-registries" : ["34.56.193.143:5000"] }
            - 重新啟動 docker
                - sudo systemctl restart docker
                - sudo systemctl status docker
        - 如果是其他桌面版，可以直接找「設定」=>「docker engineer」=> 直接在json裡面加入，完成後重新啟動docker

- curl http://<your-gcp-vm-ip>:5000/v2/_catalog
    - curl http://34.56.193.143:5000/v2/_catalog

- curl http://<your-gcp-vm-ip>:5000/v2/<repository_name>/tags/list
    - curl http://34.56.193.143:5000/v2/dotnet-webapi/tags/list

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