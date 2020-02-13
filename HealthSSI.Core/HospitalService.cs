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
    public class HospitalService : IHospitalService
    {
        private readonly SSIDbContext _dbContext;

        public HospitalService(SSIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateHospitalResponse> Create(CreateHospitalRequest request)
        {
            try
            {
                var hospital = new Hospital(request.Name, request.PublicKey);
                _dbContext.Hospitals.Add(hospital);
                await _dbContext.SaveChangesAsync();
                return new CreateHospitalResponse(hospital);
            }
            catch (Exception ex)
            {
                // log details somewhere. i.e. inner exception, stacktrace, etc...
                return new CreateHospitalResponse("There was an error creating a new Hospital");
            }
        }
    }
}
