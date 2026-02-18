using CollegeApp.Data.Config;
using Microsoft.EntityFrameworkCore;
using Routing.Data;
using Routing.Data.Config;

namespace CollegeApp.Data{
    public class CollegeDBContext(DbContextOptions<CollegeDBContext> options) : DbContext(options){
        public required DbSet<Student>  Students {get;set;}
        public  DbSet<Department> Department {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           //table 1
           modelBuilder.ApplyConfiguration(new StudentConfig());

           //table 2
           modelBuilder.ApplyConfiguration(new DepartmentConfig());
        }
    }
}