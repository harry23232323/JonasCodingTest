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
    [RoutePrefix("api/company")]
    public class CompanyController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        private readonly IAppLogger _logger;

        public CompanyController(ICompanyService companyService, IMapper mapper, IAppLogger logger)
        {
            _companyService = companyService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            try
            {
                var items = await _companyService.GetAllCompaniesAsync();
                return Ok(_mapper.Map<IEnumerable<CompanyDto>>(items));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving all companies", ex);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{companyCode}")]
        public async Task<IHttpActionResult> GetAsync(string companyCode)
        {
            try
            {
                var item = await _companyService.GetCompanyByCodeAsync(companyCode);
                if (item == null)
                    return NotFound();

                return Ok(_mapper.Map<CompanyDto>(item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving company with code: {companyCode}", ex);
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> PostAsync([FromBody] CompanyDto companyDto)
        {
            try
            {
                if (companyDto == null)
                    return BadRequest("Company data is required.");

                var companyInfo = _mapper.Map<CompanyInfo>(companyDto);
                var success = await _companyService.SaveCompanyAsync(companyInfo);
                if (!success)
                    return BadRequest("Failed to save company.");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error saving company", ex);
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("{companyCode}")]
        public async Task<IHttpActionResult> PutAsync(string companyCode, [FromBody] CompanyDto companyDto)
        {
            try
            {
                if (companyDto == null)
                    return BadRequest("Company data is required.");

                companyDto.CompanyCode = companyCode;
                var companyInfo = _mapper.Map<CompanyInfo>(companyDto);
                var success = await _companyService.SaveCompanyAsync(companyInfo);
                if (!success)
                    return BadRequest("Failed to update company.");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating company with code: {companyCode}", ex);
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("{siteId}/{companyCode}")]
        public async Task<IHttpActionResult> DeleteAsync(string siteId, string companyCode)
        {
            try
            {
                var success = await _companyService.DeleteCompanyAsync(siteId, companyCode);
                if (!success)
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting company with siteId: {siteId}, code: {companyCode}", ex);
                return InternalServerError(ex);
            }
        }
    }
}
