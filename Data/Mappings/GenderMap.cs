using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyNotesApp.Models;

namespace MyNotesApp.Data.Mappings
{
    public class GenderMap : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(100);
            builder.Property(x => x.Name).IsRequired();
            builder.ToTable("Genders");
            builder.HasData(new Gender{Id = 3,Name = "זכר"},new Gender { Id=4,Name="נקבה"}, new Gender { Id = 5, Name = "אחר" });
        }
    }
}
