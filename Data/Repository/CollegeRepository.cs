using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Repository{
    public class CollegeRepository<T> : ICollegeRepository<T> where T :class{
        private readonly CollegeDBContext _dbContext;
        private DbSet<T> _dbSet;
    public CollegeRepository(CollegeDBContext dbContext){
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
    public async Task<T> CreateAsync(T dbRecord)
        {
            _dbSet.Add(dbRecord);
            await _dbContext.SaveChangesAsync();
            return dbRecord;
        }
    public async Task<T> DeleteAsync(T dbRecord)
        {
            _dbSet.Remove(dbRecord);
            await _dbContext.SaveChangesAsync();
            return dbRecord;
        }

    public async Task<List<T>> GetAllAsync(){
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false)
    {
         if (useNoTracking){
                var entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(filter);
                return entity ?? throw new Exception("Record not found");
         }else {
                var entity = await _dbSet.FirstOrDefaultAsync(filter);
                return entity ?? throw new Exception("Record not found");
            }
    }

    // Get student by Name  
    public async Task<T?> GetByNameAsync(Expression<Func<T, bool>> filter)
    {
        return await _dbSet.Where(filter).FirstOrDefaultAsync();
    }
     public async Task<T> UpdateAsync(T dbRecord)
    {

        _dbSet.Update(dbRecord);
        await _dbContext.SaveChangesAsync();
        
        return dbRecord;
    }
    }
}