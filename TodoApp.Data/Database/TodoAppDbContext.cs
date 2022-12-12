using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using TodoApp.Domain.Todo;

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

            _logger.LogDebug("using custom connection string as {0}", connection);
            optionsBuilder.UseNpgsql(connection);
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.ToTable("todoitem");
                entity.HasKey(t => t.Id);

                entity.Property(o => o.Id)
                    .HasColumnName("id");

                entity.Property(o => o.Description)
                    .HasColumnName("description")
                    .HasMaxLength(100);

                entity.Property(o => o.State)
                    .HasColumnName("state");

            });
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