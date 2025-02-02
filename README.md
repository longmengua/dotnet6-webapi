# dotnet6-webapi

## self learn c# for webapi

- background
    - backend skills => java, golang, c, node
    - frontend skills => react, vue

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
    - perfoamnce testing + loading testing

- phase 8
    - redis + lua 