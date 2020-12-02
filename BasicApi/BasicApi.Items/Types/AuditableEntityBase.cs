using System;

namespace BasicApi.Items.Types
{
    public class AuditableEntityBase : EntityBase, IAuditableEntityBase
    {
        public Guid CreatedBy { get; set; }

        public Guid ModifiedBy { get; set; }
    }
}
