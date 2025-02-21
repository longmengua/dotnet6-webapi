namespace dotnet6_webapi.Middleware;

public class TimeoutMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TimeSpan _timeout;

    // 建構函式，初始化超時時間
    public TimeoutMiddleware(RequestDelegate next, TimeSpan timeout)
    {
        _next = next;
        _timeout = timeout;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 創建一個取消令牌，當超時後會觸發請求的取消
        var cts = new CancellationTokenSource(_timeout);
        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, context.RequestAborted);

        try
        {
            // 這裡確保請求處理與超時進行並行處理
            var delayTask = Task.Delay(_timeout, linkedCts.Token); // 設置超時時間
            var requestTask = _next(context);

            // 使用 Task.WhenAny，當請求處理完成或超時發生時即返回
            var completedTask = await Task.WhenAny(requestTask, delayTask);
            // 如果 completedTask 是 delayTask，表示超時
            if (completedTask == delayTask)
            {
                context.Response.StatusCode = 408; // 請求超時
                await context.Response.WriteAsync("請求超時。");

                // 顯式取消請求
                cts.Cancel();
                return;
            }

            // 請求處理正常完成
            await requestTask;
        }
        catch (OperationCanceledException)
        {
            // 如果請求被取消（例如超時或被外部請求中止）
            context.Response.StatusCode = 408;
            await context.Response.WriteAsync("請求超時。");

            // 顯式取消請求
            cts.Cancel();
        }
    }
}