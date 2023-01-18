using AppWord.API.Extensions;
using AppWord.Data.SeedData;
using AppWord.Data;
using Elastic.Apm.NetCoreAll;
using FM.Project.BaseLibrary.BaseGenericException;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using AppWord.Core.IServices;

var builder = WebApplication.CreateBuilder(args);

ConfigureLogging();

builder.Host.UseSerilog();

builder.Services.ServiceCollectionExtension(builder.Configuration);

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<AppWordDbContext>().DatabaseMigrator();
}

var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();

app.UseRequestLocalization(localizationOptions.Value);

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}


app.UseHangfireDashboard("/hangfire", new DashboardOptions()
{
    DashboardTitle = "Hangfire Dashboard",
    Authorization = new[]{
    new HangfireCustomBasicAuthenticationFilter{
        User = builder.Configuration.GetSection("HangfireCredentials:UserName").Value,
        Pass = builder.Configuration.GetSection("HangfireCredentials:Password").Value
    }
}
});
app.UseHangfireServer();

//https://dev.to/moe23/net-6-background-jobs-with-hangfire-4nj7
//https://www.borakasmer.com/net-6-0-uzerinde-hangfire-implementasyonu/
GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 3 }); //Tan�mlamas� ile, ilgili method ba�ar�l� �ekilde �al��t�r�lamaz ise, hata al�n�lmay�ncaya kadar 3 defa tekrar edilmektedir.
RecurringJob.RemoveIfExists("IHangfireService.WordOfDayUpdate"); //�oklama yapmamas� i�in ile �nce var olan� silip tekrar olu�turuyoruz.
//RecurringJob.AddOrUpdate<IHangfireService>(x => x.WordOfDayUpdate(), Cron.MinuteInterval(2), TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time"));

app.UseAllElasticApm(builder.Configuration);

app.UseCors("corsapp");

app.UseMiddleware<FMExceptionCatcherMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureLogging()
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
        .Enrich.WithProperty("Environment", environment)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
{
    return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{"appWord"}-{DateTime.UtcNow:yyyy-MM}"
    };
}
