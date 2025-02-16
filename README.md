# dotnet6-webapi

## clean Nuget

- dotnet nuget locals all --clear
- dotnet restore

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
    - 建立 .net 6 專案
        - dotnet new webapi -n dotnet6-webapi 

- phase 2
    - 移除 IIS of launchSetting，因為伺服器會架設在 Linux 系列的主機
    - 降低 window license 費用

- phase 3
    - 整合 Log 到 ELK，用 Serilog 取代 Logstash，其餘不變。

- phase 4
    - 異常捕捉 middleware

- phase 5
    - graceful shutdown (優雅關閉，用於滾動式更新、藍綠部署、金絲雀部署...等等。)
        - 測試方式
            - Use the command `dotnet run` to launch the app.
            - Then use `Ctrl + C` to stop the app and check if the graceful shutdown is triggered.

- phase 6
    - API timeout middleware
        - 透過 CancellationTokenSource 設定每個 API timeout 時間，以避免死鎖發生。

- phase 7
    - 實作登入、JWT 驗證
        - 先用內建的
```
// 啟用授權
app.UseAuthentication(); // 先認證用戶身份
app.UseAuthorization(); // 然後根據身份授權
```
        - 再改用 filter 跟 DB 實作

- phase 8
    - perfoamnce test with Locust (壓測)

- phase 9
    - redis + lua 
