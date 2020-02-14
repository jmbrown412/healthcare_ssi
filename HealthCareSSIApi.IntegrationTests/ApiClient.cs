using HealthSSI.Core.Requests;
using HealthSSI.Core.Responses;
using HealthSSI.Data;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareSSIApi.IntegrationTests
{
    public class ApiClient
    {
        private readonly RestClient _restCient;

        public ApiClient(string url)
        {
            _restCient = new RestClient(url);
        }

        public async Task<CreateHospitalResponse> CreateHospital(CreateHospitalRequest request)
        {
            string resource = $"/api/hospitals";
            var restRequest = new RestRequest(resource, Method.POST);
            restRequest.AddParameter("application/json", JsonConvert.SerializeObject(request), ParameterType.RequestBody);
            IRestResponse response = await MakeCall(restRequest);
            return JsonConvert.DeserializeObject<CreateHospitalResponse>(response.Content);
        }

        public async Task<CreateInsuranceCoResponse> CreateInsuranceCo(CreateInsuraceCoRequest request)
        {
            string resource = $"/api/insurancecos";
            var restRequest = new RestRequest(resource, Method.POST);
            restRequest.AddParameter("application/json", JsonConvert.SerializeObject(request), ParameterType.RequestBody);
            IRestResponse response = await MakeCall(restRequest);
            return JsonConvert.DeserializeObject<CreateInsuranceCoResponse>(response.Content);
        }

        public async Task<CreatePatientResponse> CreatePatient(CreatePatientRequest request)
        {
            string resource = $"/api/patients";
            var restRequest = new RestRequest(resource, Method.POST);
            restRequest.AddParameter("application/json", JsonConvert.SerializeObject(request), ParameterType.RequestBody);
            IRestResponse response = await MakeCall(restRequest);
            return JsonConvert.DeserializeObject<CreatePatientResponse>(response.Content);
        }

        public async Task<CreateDocResponse> CreateDoc(CreateDocRequest request)
        {
            string resource = $"/api/docs";
            var restRequest = new RestRequest(resource, Method.POST);
            restRequest.AddParameter("application/json", JsonConvert.SerializeObject(request), ParameterType.RequestBody);
            IRestResponse response = await MakeCall(restRequest);
            return JsonConvert.DeserializeObject<CreateDocResponse>(response.Content);
        }

        public async Task<Document> GetDoc(long id)
        {
            string resource = $"/api/docs?docId={id}";
            var restRequest = new RestRequest(resource, Method.GET);
            IRestResponse response = await MakeCall(restRequest);
            return JsonConvert.DeserializeObject<Document>(response.Content);
        }

        private async Task<IRestResponse> MakeCall(RestRequest restRequest)
        {
            return _restCient.Execute(restRequest);
        }


    }
}
