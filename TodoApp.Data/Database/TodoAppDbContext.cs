using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using TodoApp.Data.Models;

namespace TodoApp.Data.Database
{
    public class TodoAppDbContext : DbContext
    {
        private readonly ILogger<TodoAppDbContext> _logger; 
        private readonly ILoggerFactory _loggerFactory; 
        private readonly IConfiguration _configuration;

        public DbSet<TodoItem> TodoItems { get; set; }

        public TodoAppDbContext(
            DbContextOptions<TodoAppDbContext> options, 
            IConfiguration configuration,
            ILogger<TodoAppDbContext> logger,
            ILoggerFactory loggerFactory)
            : base(options)
        {
            _configuration = configuration;
            _logger = logger;
            _loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            var connection = _configuration.GetConnectionString("TodoAppDbConnection");
            if (connection == null)
            {
                var dbSocketDir = _configuration.GetValue<string>("DB_SOCKET_PATH") ?? "/cloudsql";
                var instanceConnectionName = _configuration.GetValue<string>("INSTANCE_CONNECTION_NAME");
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder()
                {
                    SslMode = SslMode.Disable,

                    Host = $"{dbSocketDir}/{instanceConnectionName}",
                    Username = _configuration.GetValue<string>("DB_USER"),
                    Password = _configuration.GetValue<string>("DB_PASS"),
                    Database = _configuration.GetValue<string>("DB_NAME"),
                };
                connection = connectionStringBuilder.ToString();
            }
            
            optionsBuilder.UseNpgsql(connection);
            optionsBuilder.UseLoggerFactory(_loggerFactory);
            optionsBuilder.LogTo(message => _logger.LogInformation(message), LogLevel.Information);
        }

        public void Migrate()
        {
            try
            {
                _logger.LogInformation("checking migration...");
                base.Database.Migrate();
            }
            catch (System.Exception ex)
            {
                _logger.LogCritical(ex, "Problem occurred while running the migration check.");
            }            
        }
    }
}