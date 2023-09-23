namespace TodoApp.WebApi.Configuration
{
    public class TodoAppConfiguration
    {
        public CorsConfiguration Cors { get; set; }

        public ExternalConfiguration External { get; set; }
    }
}