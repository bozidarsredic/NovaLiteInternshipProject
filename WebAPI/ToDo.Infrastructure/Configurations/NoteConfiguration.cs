using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoCore;

namespace ToDo.Infrastructure.Configurations
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {


        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
