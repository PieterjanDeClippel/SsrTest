using Microsoft.AspNetCore.SpaServices.AngularCli;
using MintPlayer.AspNetCore.Hsts;
using MintPlayer.AspNetCore.SpaServices.Prerendering;
using MintPlayer.AspNetCore.SpaServices.Routing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

// In production, the Angular files will be served from this directory
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/dist";
});

builder.Services.AddSpaPrerenderingService<Project14.Services.SpaPrerenderingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseImprovedHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

if (!app.Environment.IsDevelopment())
{
    app.UseSpaStaticFiles();
}

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "ClientApp";

    spa.UseSpaPrerendering(options =>
    {
        options.BootModuleBuilder = app.Environment.IsDevelopment() ? new AngularPrerendererBuilder(npmScript: "build:ssr") : null;
        options.BootModulePath = $"{spa.Options.SourcePath}/dist/ClientApp/server/main.js";
        options.ExcludeUrls = new[] { "/sockjs-node" };
    });

    if (app.Environment.IsDevelopment())
    {
        spa.UseAngularCliServer(npmScript: "start");
    }
});

app.Run();
