
namespace CollegeApp.Data.Repository;

public interface IStudentRepository
{
    Task<List<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(int id,bool useNoTracking = false);
    Task<Student?> GetByNameAsync(string name);
    Task<Student> CreateAsync(Student student);
    Task<Student> UpdateAsync(Student student);
    Task<Student> DeleteAsync(Student student);
}
