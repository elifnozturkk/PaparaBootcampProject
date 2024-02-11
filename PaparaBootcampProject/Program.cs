using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PaparaApp.Project.API.Mapping.TenantFlat;
using PaparaApp.Project.API.Models;
using PaparaApp.Project.API.Models.UserTimelyPaymentDetails;
using PaparaApp.Project.API.Models.Flats;
using PaparaApp.Project.API.Models.Payments;
using PaparaApp.Project.API.Models.Users.Tenants;
using PaparaApp.Project.API.Models.Tokens;
using PaparaApp.Project.API.Models.UnitOfWorks;
using PaparaApp.Project.API.Seeders;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using PaparaApp.Project.API.BackgroundService;
using PaparaApp.Project.API.Models.UserDiscountStatuses;
using PaparaApp.Project.API.Helper;
using PaparaApp.Project.API.Models.Users;
using PaparaApp.Project.API.Filters;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IAppUserService, AppUserService>();
builder.Services.AddScoped<IFlatService, FlatService>();
builder.Services.AddScoped<IFlatRepository, FlatRepository>();
builder.Services.AddScoped<ITenantFlatService, TenantFlatService>();
builder.Services.AddScoped<ITenantFlatRepository, TenantFlatRepositrory>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IUserPaymentDetailRepository, UserPaymentDetailRepository>();
builder.Services.AddScoped<IUserDiscountStatusRepository, UserDiscountStatusRepository>();
builder.Services.AddScoped<IUserDiscountStatusService, UserDiscountStatusService>();
builder.Services.AddScoped<UserContextHelper>();
builder.Services.AddScoped<PaymentDetailHelper>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<NotFoundActionFilter>(); 
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});
builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddAuthentication(options =>
{


    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
{
    var signatureKey = builder.Configuration.GetSection("TokenOptions")["SignatureKey"]!;
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signatureKey))
    };
});

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });

builder.Services.AddHostedService<OverduePaymentsBackgroundService>();


var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await ManagerDataSeeder.SeedManagerUserAsync(userManager, roleManager);
}