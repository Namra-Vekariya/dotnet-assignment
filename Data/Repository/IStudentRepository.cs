
namespace CollegeApp.Data.Repository;

public interface IStudentRepository : ICollegeRepository<Student>
// this is how we inherit the common repo 
{
//     we also add additional tasks or functions 
// Task<List<Student>> GetStudentsByFeeStatusAsync(int feeStatus);
}
