using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;

namespace BusinessLayer.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly IAppLogger _logger;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper, IAppLogger logger)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CompanyInfo>> GetAllCompaniesAsync()
        {
            _logger.LogInfo("Retrieving all companies");
            var res = await _companyRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CompanyInfo>>(res);
        }

        public async Task<CompanyInfo> GetCompanyByCodeAsync(string companyCode)
        {
            _logger.LogInfo($"Retrieving company with code: {companyCode}");
            var result = await _companyRepository.GetByCodeAsync(companyCode);
            return _mapper.Map<CompanyInfo>(result);
        }

        public async Task<bool> SaveCompanyAsync(CompanyInfo companyInfo)
        {
            _logger.LogInfo($"Saving company with code: {companyInfo?.CompanyCode}");
            var entity = _mapper.Map<Company>(companyInfo);
            return await _companyRepository.SaveAsync(entity);
        }

        public async Task<bool> DeleteCompanyAsync(string siteId, string companyCode)
        {
            _logger.LogInfo($"Deleting company with siteId: {siteId}, code: {companyCode}");
            return await _companyRepository.DeleteAsync(siteId, companyCode);
        }
    }
}
