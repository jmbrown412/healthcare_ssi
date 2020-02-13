using HealthSSI.Core.Requests;
using HealthSSI.Core.Responses;
using System.Threading.Tasks;

namespace HealthSSI.Core
{
    public interface IHospitalService
    {
        Task<CreateHospitalResponse> Create(CreateHospitalRequest request);
    }
}
