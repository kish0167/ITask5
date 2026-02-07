using ITask5;
using ITask5.Services;
using ITask5.Services.DataGenerator;
using ITask5.Services.TextGenerator;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews()
    .AddViewLocalization();
builder.Services.AddLocalization(options =>  options.ResourcesPath = "Resources");

builder.Services.AddSingleton<ITextGenerator, TextGenerator>();
builder.Services.AddSingleton<IDataGenerator, DataGenerator>();
builder.Services.Configure<DataGeneratorOptions>(options =>
{
    options.DefaultLanguage = Languages.Default;
    options.SongsPerPage = 30;
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture(Languages.Default)
        .AddSupportedCultures(Languages.All.ToArray())
        .AddSupportedUICultures(Languages.All.ToArray());
});
builder.Services.AddSession();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Main/Error");
    app.UseHsts();
}

app.UseRequestLocalization();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Index}/{id?}");
app.UseSession();
app.Run();