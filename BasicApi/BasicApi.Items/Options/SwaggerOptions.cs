namespace BasicApi.Items.Options
{
    public class SwaggerOptions
    {
        public bool Enabled { get; set; }

        public bool ReDocEnabled { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Version { get; set; }

        public string RoutePrefix { get; set; }

        public bool IncludeSecurity { get; set; }

        public bool CommentsEnabled { get; set; }

        public string XmlFileName { get; set; }
    }
}
