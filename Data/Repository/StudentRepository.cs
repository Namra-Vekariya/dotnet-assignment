using CollegeApp.Data;
using CollegeApp.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Routing.Data.Repository;

public class StudentRepository : CollegeRepository<Student> ,IStudentRepository{
    private readonly CollegeDBContext _dbContext;
    public StudentRepository(CollegeDBContext dbContext) : base(dbContext){
        _dbContext= dbContext;
    }

    
}