using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using sti_sys_backend.DB;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using sti_sys_backend.Authentication;
using sti_sys_backend.Core.Constructors;
using sti_sys_backend.JWT;
using sti_sys_backend.Utilization;
using sti_sys_backend.Utilization.Implementations;
using sti_sys_backend.Utilization.MailDto;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
builder.Services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
builder.Services.AddDbContext<DatabaseQueryable>(options => 
    options.UseSqlServer(configuration["ConnectionStrings:prodenv"],
    providerOptions => providerOptions.EnableRetryOnFailure())
);

builder.Services.AddIdentity<JWTIdentity, IdentityRole>()
    .AddEntityFrameworkStores<DatabaseQueryable>()
    .AddDefaultTokenProviders();

var passwordOptions = new PasswordOptions{
    RequireDigit = false,
    RequiredLength = 6,
    RequireLowercase = false,
    RequireNonAlphanumeric = false,
    RequireUppercase = false
};
builder.Services.Configure<IdentityOptions>(options => {
    options.Password = passwordOptions;
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:IssuerSigningKey"]))
    };
});
var myappOrigins = "_myAppOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(myappOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:3001")
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    );
});
builder.Services.AddControllers();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
});
Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseUrls("http://localhost:5240");
        webBuilder.UseStartup<WebApplication>();
});
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 5001;
});
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "The API key to access API's",
        Type = SecuritySchemeType.ApiKey,
        Name = "x-api-key",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });
    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    var requirement = new OpenApiSecurityRequirement{
        { scheme, new List<string>() }
    };
    c.AddSecurityRequirement(requirement);
});
builder.Services.AddScoped<AccountsConstructor>();
builder.Services.AddScoped<VerificationConstructor>();
builder.Services.AddScoped<SectionsConstructor>();
builder.Services.AddScoped<TicketingConstructor>();
builder.Services.AddScoped<MeetingRoomConstructor>();
builder.Services.AddScoped<KeyAuthFilter>();
builder.Services.AddTransient<IMailImplementation, MailService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseForwardedHeaders();
app.UseRouting();
app.UseCors(myappOrigins);

//app.UseHttpsRedirection();

app.UseMiddleware<Middleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();