using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;

namespace BusinessLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly IAppLogger _logger;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            ICompanyRepository companyRepository,
            IMapper mapper,
            IAppLogger logger)
        {
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeesAsync()
        {
            _logger.LogInfo("Retrieving all employees");
            var employees = await _employeeRepository.GetAllAsync();
            var companies = (await _companyRepository.GetAllAsync()).ToList();

            return employees.Select(e =>
            {
                var info = _mapper.Map<EmployeeInfo>(e);
                info.CompanyName = companies
                    .FirstOrDefault(c => c.CompanyCode.Equals(e.CompanyCode))?.CompanyName;
                return info;
            });
        }

        public async Task<EmployeeInfo> GetEmployeeByCodeAsync(string siteId, string employeeCode)
        {
            _logger.LogInfo($"Retrieving employee with siteId: {siteId}, code: {employeeCode}");
            var employee = await _employeeRepository.GetByCodeAsync(siteId, employeeCode);
            if (employee == null) return null;

            var info = _mapper.Map<EmployeeInfo>(employee);
            var company = await _companyRepository.GetByCodeAsync(employee.CompanyCode);
            info.CompanyName = company?.CompanyName;
            return info;
        }

        public async Task<bool> SaveEmployeeAsync(EmployeeInfo employeeInfo)
        {
            _logger.LogInfo($"Saving employee with code: {employeeInfo?.EmployeeCode}");
            var entity = _mapper.Map<Employee>(employeeInfo);
            entity.LastModified = DateTime.UtcNow;
            return await _employeeRepository.SaveAsync(entity);
        }

        public async Task<bool> DeleteEmployeeAsync(string siteId, string employeeCode)
        {
            _logger.LogInfo($"Deleting employee with siteId: {siteId}, code: {employeeCode}");
            return await _employeeRepository.DeleteAsync(siteId, employeeCode);
        }
    }
}
