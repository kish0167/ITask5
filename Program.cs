using ITask5;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews()
    .AddViewLocalization();
builder.Services.AddLocalization(options =>  options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture(Languages.All[0])
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