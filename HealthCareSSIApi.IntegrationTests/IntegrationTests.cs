using FluentAssertions;
using HealthCareSSI.Tests.Common;
using HealthSSI.Core.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HealthCareSSIApi.IntegrationTests
{
    [TestClass]
    public class IntegrationTests : BaseCoreTest
    {
        private ApiClient GetApiClient()
        {
            return new ApiClient("https://localhost:5001/");
        }

        [TestMethod]
        public async Task Verifying_A_Document_Signed_By_A_Specific_Hospital_And_Patient_Should_Return_Success()
        {
            var cryptoProvider = GetCryptoProvider();
            RSAParameters rsaPrivateKeyInfo = cryptoProvider.ExportParameters(true);
            string publicKeyPem = ExportPublicKey(cryptoProvider);
            var client = GetApiClient();

            var hospital = await client.CreateHospital(new CreateHospitalRequest($"New hospital-{DateTime.UtcNow}", publicKeyPem));
            hospital.Success.Should().BeTrue();
            var insuranceCo = await client.CreateInsuranceCo(new CreateInsuraceCoRequest($"New insurance co - {DateTime.UtcNow}"));
            insuranceCo.Success.Should().BeTrue();
            var patient = await client.CreatePatient(new CreatePatientRequest($"Joe-{DateTime.UtcNow}", $"Smith-{DateTime.Now}", $"joe.smith-{DateTime.Now}@gmail.com"));
            patient.Success.Should().BeTrue();
            var createdDoc = await client.CreateDoc(new CreateDocRequest(patient.Id, hospital.Id));
            createdDoc.Success.Should().BeTrue();
            var doc = await client.GetDoc(createdDoc.Id);

            // sign the doc
            var message = doc.ToJson();
            string signedMessage = SignData(message, rsaPrivateKeyInfo);

            // verify the doc
            var docValidatorService = GetDocVerificationService();
            var result = docValidatorService.ValidateDocument(patient.Id, doc, signedMessage, publicKeyPem);

            result.Success.Should().BeTrue();
            result.HospitalSigned.Should().BeTrue();
            result.ForPatient.Should().BeTrue();
        }
    }
}
