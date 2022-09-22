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


    public SqlLiteDbContext(IConfiguration configuration) : base(configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_configuration["SqlLiteConnection"]);
    }
}