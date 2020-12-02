using System;

namespace BasicApi.Items.Types
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }

        public bool? IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
    }
}
