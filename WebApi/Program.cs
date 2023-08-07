using Serilog;
using SimpleInjector;
using WebApi.CompositionRoot;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddHealthChecks();

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen();

    builder.Host.UseSerilog((_, lc) => lc
        .WriteTo.Console());
    var container = Bootstrapper.Bootstrap(builder.Services, builder.Configuration);

    var app = builder.Build();

    app.Services.UseSimpleInjector(container);

    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHsts();
    app.UseRouting();


    app.UseEndpoints(endpoints =>
    {
        endpoints.MapHealthChecks("/healthz");
        endpoints.MapControllers();
        endpoints.MapSwagger();
    });

#if DEBUG
    container.Verify(VerificationOption.VerifyAndDiagnose);
#endif

app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
