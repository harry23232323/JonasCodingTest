using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using WebApi.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("api/employee")]
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly IAppLogger _logger;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper, IAppLogger logger)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            try
            {
                var items = await _employeeService.GetAllEmployeesAsync();
                return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(items));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving all employees", ex);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{siteId}/{employeeCode}")]
        public async Task<IHttpActionResult> GetAsync(string siteId, string employeeCode)
        {
            try
            {
                var item = await _employeeService.GetEmployeeByCodeAsync(siteId, employeeCode);
                if (item == null)
                    return NotFound();

                return Ok(_mapper.Map<EmployeeDto>(item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving employee with siteId: {siteId}, code: {employeeCode}", ex);
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> PostAsync([FromBody] EmployeeDto employeeDto)
        {
            try
            {
                if (employeeDto == null)
                    return BadRequest("Employee data is required.");

                var employeeInfo = _mapper.Map<EmployeeInfo>(employeeDto);
                var success = await _employeeService.SaveEmployeeAsync(employeeInfo);
                if (!success)
                    return BadRequest("Failed to save employee.");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error saving employee", ex);
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("{siteId}/{employeeCode}")]
        public async Task<IHttpActionResult> PutAsync(string siteId, string employeeCode, [FromBody] EmployeeDto employeeDto)
        {
            try
            {
                if (employeeDto == null)
                    return BadRequest("Employee data is required.");

                employeeDto.SiteId = siteId;
                employeeDto.EmployeeCode = employeeCode;
                var employeeInfo = _mapper.Map<EmployeeInfo>(employeeDto);
                var success = await _employeeService.SaveEmployeeAsync(employeeInfo);
                if (!success)
                    return BadRequest("Failed to update employee.");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating employee with siteId: {siteId}, code: {employeeCode}", ex);
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("{siteId}/{employeeCode}")]
        public async Task<IHttpActionResult> DeleteAsync(string siteId, string employeeCode)
        {
            try
            {
                var success = await _employeeService.DeleteEmployeeAsync(siteId, employeeCode);
                if (!success)
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting employee with siteId: {siteId}, code: {employeeCode}", ex);
                return InternalServerError(ex);
            }
        }
    }
}
