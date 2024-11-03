using LinkDev.IKEA.DAL.Entities.Employees;
using LinkDev.IKEA.DAL.Persistance.Data;
using LinkDev.IKEA.DAL.Persistance.Repositories._Generic;

namespace LinkDev.IKEA.DAL.Persistance.Repositories.Employees
{
    public class EmployeeRepository(ApplicationDbContext dbContext) : GenericRepository<Employee>(dbContext), IEmployeeRepository
    {

        private readonly ApplicationDbContext _dbContext = dbContext;

       


    }
}
