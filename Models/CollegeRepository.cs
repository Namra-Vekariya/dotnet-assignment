namespace CollegeApp.Models
{
    public class CollegeRepository {
        public static List<Student> Students {get;set;} = new List<Student>(){
            
            new Student { Id = 1, StudentName = "Parth", Email = "PArth@gmail.com", Address = "dhari, India" },
            new Student { Id = 2, StudentName = "maru", Email = "maru@gmail.com", Address = "Porbander, India" }
        };
    }
}