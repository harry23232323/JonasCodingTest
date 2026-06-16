using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbWrapper<Employee> _employeeDbWrapper;

        public EmployeeRepository(IDbWrapper<Employee> employeeDbWrapper)
        {
            _employeeDbWrapper = employeeDbWrapper;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employeeDbWrapper.FindAllAsync();
        }

        public async Task<Employee> GetByCodeAsync(string siteId, string employeeCode)
        {
            var results = await _employeeDbWrapper.FindAsync(e =>
                e.SiteId.Equals(siteId) && e.EmployeeCode.Equals(employeeCode));
            return results?.FirstOrDefault();
        }

        public async Task<bool> SaveAsync(Employee employee)
        {
            var existing = await _employeeDbWrapper.FindAsync(e =>
                e.SiteId.Equals(employee.SiteId) && e.EmployeeCode.Equals(employee.EmployeeCode));
            var itemRepo = existing?.FirstOrDefault();
            if (itemRepo != null)
            {
                itemRepo.EmployeeName = employee.EmployeeName;
                itemRepo.CompanyCode = employee.CompanyCode;
                itemRepo.Occupation = employee.Occupation;
                itemRepo.EmployeeStatus = employee.EmployeeStatus;
                itemRepo.EmailAddress = employee.EmailAddress;
                itemRepo.Phone = employee.Phone;
                itemRepo.LastModified = employee.LastModified;
                return await _employeeDbWrapper.UpdateAsync(itemRepo);
            }

            return await _employeeDbWrapper.InsertAsync(employee);
        }

        public async Task<bool> DeleteAsync(string siteId, string employeeCode)
        {
            return await _employeeDbWrapper.DeleteAsync(e =>
                e.SiteId.Equals(siteId) && e.EmployeeCode.Equals(employeeCode));
        }
    }
}
