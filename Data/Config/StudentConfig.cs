using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config{
    public class StudentConfig : IEntityTypeConfiguration<Student>{
        public void Configure(EntityTypeBuilder<Student> builder){
            builder.ToTable("Students");
            builder.HasKey(x=>x.Id); //remove [key] in model 
            builder.Property(x=>x.Id).UseIdentityColumn();
                builder.Property(n=> n.StudentName);
                builder.Property(n=>n.StudentName).HasMaxLength(250);
                builder.Property(n=>n.Address).HasMaxLength(500);
                builder.Property(n=>n.Email).HasMaxLength(250);
                builder.HasData( new List<Student>(){

                    new Student{Id=1,StudentName="AA",Email="AA@gmail.com"},
                    new Student{Id=2,StudentName="bb",Email="bb@gmail.com"}
                });
        }
    }
}