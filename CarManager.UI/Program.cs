global using Blazored.LocalStorage;
global using Microsoft.AspNetCore.Components.Authorization;
global using Microsoft.AspNetCore.Components.Web;
global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
global using MudBlazor;
global using MudBlazor.Services;
using AuthProviders;
using CarManager.UI;
using Microsoft.AspNetCore.Authentication;
using Services;
using StorageManagement.UI;
using ViewModels;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});
builder.Services.AddScoped<TokenStorage>();
builder.Services.AddScoped<TokenAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<TokenAuthenticationStateProvider>());
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAuthenticationViewModel, AuthenticationViewModel>();

await builder.Build().RunAsync();