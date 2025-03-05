# dotnet6-webapi

## 自學 c# for webapi

- 背景
    - 後端
        - java, golang, c, node
    - 前端
        - react, vue3, angularJS, ember, nuxt3, next
    - 智能合約
        - solidity
    - 雲端
        - AWS, GCP

## 清除 Package

- dotnet nuget locals all --clear
- dotnet restore

## 前言

- 本專案全面設定成 http，因為理想的架構，前面應該有 gateway 處理 SSL 的問題(如：nginx)，故不用https。

## 私人 Docker Hub 推送

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

## EF core

- dotnet tool install --global dotnet-ef --version 6.0.6
    - dotnet ef --version
    - dotnet ef migrations add InitialCreate
        - 如果不能執行，就先執行 dotnet build，在執行此指令
    - dotnet ef migrations add UpdateUserTable
    - dotnet ef database update
    
## 自動掃描 service, repo

- dotnet add package Scrutor

## 里程碑

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
        - 用 filter(middleware) 跟 DB 實作
        - 概念：用jwt進行快速驗證，refresh token 進行控管，當大流量時候，伺服器本身即可進行驗證，不需要跟資料庫交互，每個token存活時間只有15分鐘，可在token內加入fingerprinter，避免被其他裝置盜用。

- phase 8
    - perfoamnce test with Locust (壓測)

- phase 9
    - redis + lua 
