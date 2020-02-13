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
    public class PatientService : IPatientService
    {
        private readonly SSIDbContext _dbContext;

        public PatientService(SSIDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreatePatientResponse> Create(CreatePatientRequest request)
        {
            try
            {
                var patient = new Patient(request.FirstName, request.LastName, request.Email);
                _dbContext.Patients.Add(patient);
                await _dbContext.SaveChangesAsync();
                return new CreatePatientResponse(patient);
            }
            catch (Exception ex)
            {
                // log details somewhere. i.e. inner exception, stacktrace, etc...
                return new CreatePatientResponse("There was an error creating a new Patient");
            }
        }
    }
}
