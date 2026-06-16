using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Model.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByCodeAsync(string siteId, string employeeCode);
        Task<bool> SaveAsync(Employee employee);
        Task<bool> DeleteAsync(string siteId, string employeeCode);
    }
}
