using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
            optionsBuilder.UseNpgsql(connection);

            optionsBuilder.UseLoggerFactory(_loggerFactory);
            optionsBuilder.LogTo(message => _logger.LogInformation(message), LogLevel.Information);
        }
    }
}