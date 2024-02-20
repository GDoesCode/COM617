using COM617.Data;
using COM617.Data.Identity;
using COM617.Services;
using COM617.Services.Identity;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace COM617
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));


            builder.Services.AddControllersWithViews()
                .AddMicrosoftIdentityUI();

            builder.Services.AddAuthorization(options =>
            {
                // By default, all incoming requests will be authorized according to the default policy
                //options.FallbackPolicy = options.DefaultPolicy;
            });

            builder.Services
            .AddIdentityCore<User>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
            })
            .AddUserStore<UserStore>().AddSignInManager<SignInManager<User>>();
            builder.Services.AddTransient<IUserClaimStore<User>, UserStore>();

            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor()
                .AddMicrosoftIdentityConsentHandler();

            builder.Services.AddSingleton<MongoDbService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<SignInService>();

            // Set MongoDb's guid serializer
#pragma warning disable CS0618 // This line won't be required in future versions of the MongoDb driver.
            BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
            #pragma warning restore CS0618 
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();
            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}
