using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HealthSSI.Core.Requests;
using HealthSSI.Core.Responses;
using HealthSSI.Data;
using HealthSSI.Data.Entities;

namespace HealthSSI.Core
{
    public class InsuranceCoService : IInsuraceCoService
    {
        private readonly SSIDbContext _dbContext;

        public InsuranceCoService(SSIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateInsuranceCoResponse> Create(CreateInsuraceCoRequest request)
        {
            try
            {
                var insuranceCo = new InsuranceCo(request.Name);
                _dbContext.InsuranceCompanies.Add(insuranceCo);
                await _dbContext.SaveChangesAsync();
                return new CreateInsuranceCoResponse(insuranceCo);
            }
            catch (Exception ex)
            {
                // log details somewhere. i.e. inner exception, stacktrace, etc...
                return new CreateInsuranceCoResponse("There was an error creating a new Hospital");
            }
        }
    }
}
