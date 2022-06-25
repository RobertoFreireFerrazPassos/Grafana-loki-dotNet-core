using Serilog;
using Serilog.Sinks.Grafana.Loki;

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration()
                        .CreateLogger();

    Log.Information("Staring the Host");

    builder
        .Host
        .UseSerilog((ctx, cfg) =>
        {
            // To disable all microsoft information logs except AspNetCore.Hosting.Diagnostics (Ex: logs of http requests)
            cfg.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error) 
               .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", Serilog.Events.LogEventLevel.Information)
                .Enrich.WithProperty("Application", ctx.HostingEnvironment.ApplicationName)
                .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName)
                .WriteTo.GrafanaLoki(ctx.Configuration["Loki"], outputTemplate: "{Message}");
        });

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Host Terminated Unexpectedly");
}

finally
{
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}