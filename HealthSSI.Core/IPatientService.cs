using HealthSSI.Core.Requests;
using HealthSSI.Core.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HealthSSI.Core
{
    public interface IPatientService
    {
        Task<CreatePatientResponse> Create(CreatePatientRequest request);
    }
}
