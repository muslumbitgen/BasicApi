using System;

namespace BasicApi.Items.Types
{
    public interface IAuditableEntityBase : IEntityBase
    {
        Guid CreatedBy { get; set; }

        Guid ModifiedBy { get; set; }
    }
}
