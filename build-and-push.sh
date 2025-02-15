#!/bin/bash

## 記得改成自己私人dockerhub IP位置

# 使腳本在發生任何錯誤時停止執行
set -e

# 1. 建置鏡像
echo "正在建置 Docker 鏡像..."
docker build --platform linux/amd64 --build-arg ENVIRONMENT=prod -t dotnet-webapi:latest .
echo "Docker 鏡像建置成功。"

# 2. 標記鏡像
echo "正在標記 Docker 鏡像..."
docker tag dotnet-webapi:latest 34.56.193.143:5000/dotnet-webapi:latest
echo "Docker 鏡像標記成功。"

# 3. 推送鏡像到私有 Registry
echo "正在將 Docker 鏡像推送到私有註冊庫..."
docker push 34.56.193.143:5000/dotnet-webapi:latest
echo "Docker 鏡像推送成功。"

# 4. 查詢鏡像標籤
echo "正在查詢鏡像標籤..."
curl http://34.56.193.143:5000/v2/dotnet-webapi/tags/list
