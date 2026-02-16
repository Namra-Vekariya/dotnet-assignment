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
        if(useNoTracking)
            return await _dbContext.Students.AsNoTracking().Where(student => student.Id == id).FirstOrDefaultAsync(s => s.Id == id);
        return await _dbContext.Students.Where(student=>student.Id == id).FirstOrDefaultAsync(s => s.Id == id);
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
    public async Task<int> CreateAsync(Student student)
    {
        await _dbContext.Students.AddAsync(student);
        await _dbContext.SaveChangesAsync();
        return student.Id;
    }

    // Update existing student
    public async Task<int> UpdateAsync(Student student)
    {

        _dbContext.Update(student);
        await _dbContext.SaveChangesAsync();
        
        return student.Id;
    }

    // Delete student by Id
    public async Task<bool> DeleteAsync(Student student)
    {
      

        _dbContext.Students.Remove(student);
        await _dbContext.SaveChangesAsync();
        return true;
    }
    
}

