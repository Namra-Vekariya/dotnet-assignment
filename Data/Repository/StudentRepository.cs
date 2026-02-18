using CollegeApp.Data;
using CollegeApp.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Routing.Data.Repository;

public class StudentRepository : IStudentRepository{
    private readonly CollegeDBContext _dbContext;
    public StudentRepository(CollegeDBContext dbContext){
        _dbContext= dbContext;
    }

    
    public async Task<List<Student>> GetAllAsync(){
        return await _dbContext.Students.ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(int id, bool useNoTracking =false)
    {
        Student? student;

            if (useNoTracking)
                student = await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            else
                student = await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == id);

            return student ?? throw new Exception("Student not found"); 
    }

    // Get student by Name  
    public async Task<Student?> GetByNameAsync(string name)
    {
        return await _dbContext.Students
        .FirstOrDefaultAsync(s =>
            s.StudentName != null &&
            s.StudentName.ToLower() == name.ToLower());
    }

    // Create new student
    public async Task<Student> CreateAsync(Student student)
    {
        await _dbContext.Students.AddAsync(student);
        await _dbContext.SaveChangesAsync();
        return student;
    }

    // Update existing student
    public async Task<Student> UpdateAsync(Student student)
    {

        _dbContext.Students.Update(student);
        await _dbContext.SaveChangesAsync();
        
        return student;
    }

    // Delete student by Id
    public async Task<Student> DeleteAsync(Student student)
    {
      

        _dbContext.Students.Remove(student);
        await _dbContext.SaveChangesAsync();
        return student;
    }
    
}

