using HealthSSI.Core.Requests;
using HealthSSI.Core.Responses;
using System.Threading.Tasks;

namespace HealthSSI.Core
{
    public interface IInsuraceCoService
    {
        Task<CreateInsuranceCoResponse> Create(CreateInsuraceCoRequest request);
    }
}
