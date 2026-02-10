using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data{
    public class CollegeDBContext(DbContextOptions<CollegeDBContext> options) : DbContext(options){
        public required DbSet<Student>  Students {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<Student>().HasData( new List<Student>(){
            new Student{Id=1,StudentName="AA",Email="AA@gmail.com"},
            new Student{Id=2,StudentName="bb",Email="bb@gmail.com"}
           } );
        }
    }
}