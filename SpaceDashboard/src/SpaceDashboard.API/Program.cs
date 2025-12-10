var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// База данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Внешние HTTP клиенты с политиками retry
builder.Services.AddHttpClient("NasaClient", client =>
{
    client.BaseAddress = new Uri("https://api.nasa.gov/");
    client.Timeout = TimeSpan.FromSeconds(30);
}).AddPolicyHandler(GetRetryPolicy());

builder.Services.AddHttpClient("JWSTClient", client =>
{
    client.BaseAddress = new Uri("https://api.jwstapi.com/");
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("x-api-key", 
        builder.Configuration["JWST:ApiKey"]);
});

// Кэширование Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "SpaceDashboard_";
});

// Регистрация сервисов
builder.Services.AddScoped<IIssService, IssService>();
builder.Services.AddScoped<IOsdrService, OsdrService>();
builder.Services.AddScoped<IJwstService, JwstService>();
builder.Services.AddScoped<ICmsService, CmsService>();
builder.Services.AddScoped<IAstroService, AstroService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();