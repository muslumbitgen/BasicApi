using BasicApi.Items.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasicApi.Data.Configurations
{
    public class FeedbackConfiguration : AuditableEntityBaseConfiguration<Feedback>
    {
        public override void Configure(EntityTypeBuilder<Feedback> builder)
        {
            base.Configure(builder);

            builder.ToTable("Feedbacks");

        }
    }
}
