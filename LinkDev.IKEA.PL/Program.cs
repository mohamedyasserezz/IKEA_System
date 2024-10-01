using LinkDev.IKEA.BLL.Common.Services.Attachments;
using LinkDev.IKEA.BLL.Services.Departments;
using LinkDev.IKEA.BLL.Services.Employees;
using LinkDev.IKEA.DAL.Entities.Identity;
using LinkDev.IKEA.DAL.Persistance.Data;
using LinkDev.IKEA.DAL.Persistance.UnitOfWork;
using LinkDev.IKEA.PL.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace LinkDev.IKEA.PL
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			#region Configure Services
			// Add services to the container.
			builder.Services.AddControllersWithViews();


			builder.Services.AddDbContext<ApplicationDbContext>(
				optionsAction: (optionsBuilder) =>
				{

					//optionsBuilder.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["DefaultConnection"]);
					optionsBuilder.UseLazyLoadingProxies()
					.UseSqlServer(builder.Configuration
					.GetConnectionString("DefaultConnection"));

				});


			/// builder.Services.AddScoped<ApplicationDbContext>();
			/// builder.Services.AddScoped<DbContextOptions<ApplicationDbContext>>((ServiceProvider) =>
			/// {
			///     var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
			///     optionsBuilder.UseSqlServer();
			///     var options = optionsBuilder.Options;
			///     return options;
			/// });

			//builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
			//builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

			builder.Services.AddScoped<IDepartmentServices, DepartmentServices>();
			builder.Services.AddScoped<IEmployeeServices, EmployeeServices>();


			//builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
			//builder.Services.AddAutoMapper(typeof(MappingProfile));
			builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfile()));
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
			builder.Services.AddTransient<IAttachmentService, AttachmentService>();
			builder.Services.AddIdentity<ApplicationUser, IdentityRole>(/*option =>
			{
				option.Password.RequiredLength = 5;
				option.Password.RequireDigit = true;
				option.Password.RequireNonAlphanumeric = true;
				option.Password.RequireUppercase = true;
				option.Password.RequireLowercase = true;

				option.User.RequireUniqueEmail = true;
				option.Lockout.AllowedForNewUsers = true;
				option.Lockout.MaxFailedAccessAttempts = 5;
				option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
			}*/)
				.AddEntityFrameworkStores<ApplicationDbContext>();
			#endregion

			var app = builder.Build();

			#region Configure Kestrel Maddleware
			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			#endregion


			app.Run();
		}
	}
}
