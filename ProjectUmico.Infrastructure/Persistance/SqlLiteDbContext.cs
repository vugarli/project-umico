using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectUmico.Infrastructure.Persistance;
using ProjectUmico.Infrastructure.Persistance.Interceptors;

namespace ProjectUmico.Infrastructure;

public class SqlLiteDbContext : ApplicationDbContext
{
    private readonly IConfiguration _configuration;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    private readonly string _connectionString;


    public SqlLiteDbContext(DbContextOptions<ApplicationDbContext> options,IConfiguration configuration,IPasswordHasher<ApplicationUser> passwordHasher,AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) :
        base(options,configuration,auditableEntitySaveChangesInterceptor,passwordHasher)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_configuration["SqlLiteConnection"]);
    }
}