using BasicApi.Items.Types;

namespace BasicApi.Items.Entities
{
    public class Feedback : AuditableEntityBase
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
    }
}
