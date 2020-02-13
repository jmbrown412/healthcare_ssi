using System;
using System.Security.Cryptography;
using FluentAssertions;
using HealthSSI.Data.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HealthSSI.Core.Tests
{
    [TestClass]
    public class DocumentValidationTests : BaseCoreTest
    {
        private IDocumentVerificationService GetDocVerificationService()
        {
            return new DocumentVerificationService(new SignatureService());
        }

        [TestMethod]
        public void Checking_Message_That_Was_Signed_By_Hospital_For_A_Specific_Patient_Should_Return_Success()
        {
            // given 
            var cryptoProvider = GetCryptoProvider();
            RSAParameters rsaPrivateKeyInfo = cryptoProvider.ExportParameters(true);
            Patient patient = new Patient("joe", "smith", "joe.smith@gmail.com");

            // simulate having an ID from the DB
            patient.Id = 123;
            Document doc = new Document(DateTime.Now, patient.Id.ToString());
            var message = doc.ToJson();
            string signedMessage = SignData(message, rsaPrivateKeyInfo);
            string publicKeyPem = ExportPublicKey(cryptoProvider);

            // when
            var docValidatorService = GetDocVerificationService();
            var result = docValidatorService.ValidateDocument(patient, doc, signedMessage, publicKeyPem);

            // then
            result.Success.Should().BeTrue();
            result.HospitalSigned.Should().BeTrue();
            result.ForPatient.Should().BeTrue();
        }

        [TestMethod]
        public void Checking_Message_That_Was_Signed_By_Hospital_And_Not_For_A_Specific_Patient_Should_Not_Return_Success()
        {
            // given 
            var cryptoProvider = GetCryptoProvider();
            RSAParameters rsaPrivateKeyInfo = cryptoProvider.ExportParameters(true);
            Patient patient1 = new Patient("joe", "smith", "joe.smith@gmail.com");

            // simulate having an ID from the DB
            patient1.Id = 123;

            Patient patient2 = new Patient("jon", "smith", "jon.smith@gmail.com");

            // simulate having an ID from the DB
            patient2.Id = 124;
            Document doc = new Document(DateTime.Now, patient1.Id.ToString());
            var message = doc.ToJson();
            string signedMessage = SignData(message, rsaPrivateKeyInfo);
            string publicKeyPem = ExportPublicKey(cryptoProvider);

            // when
            var docValidatorService = GetDocVerificationService();
            var result = docValidatorService.ValidateDocument(patient2, doc, signedMessage, publicKeyPem);

            // then
            result.HospitalSigned.Should().BeTrue();
            result.ForPatient.Should().BeFalse();
        }

        [TestMethod]
        public void Checking_Message_That_Was_Not_Signed_By_Hospital_And_Is_For_A_Specific_Patient_Should_Not_Return_Success()
        {
            // given 
            var cryptoProvider1 = GetCryptoProvider();
            var cryptoProvider2 = GetCryptoProvider();
            RSAParameters rsaPrivateKeyInfo1 = cryptoProvider1.ExportParameters(true);
            RSAParameters rsaPrivateKeyInfo2 = cryptoProvider2.ExportParameters(true);
            Patient patient1 = new Patient("jon", "smith", "jon.smith@gmail.com");

            // simulate having an ID from the DB
            patient1.Id = 124;
            Document doc1 = new Document(DateTime.Now, patient1.Id.ToString());
            Document doc2 = new Document(DateTime.Now, patient1.Id.ToString());
            var message1 = doc1.ToJson();
            var message2 = doc2.ToJson();
            string signedMessage1 = SignData(message1, rsaPrivateKeyInfo1);
            string signedMessage2 = SignData(message2, rsaPrivateKeyInfo2);
            string publicKeyPem1 = ExportPublicKey(cryptoProvider1);
            string publicKeyPem2 = ExportPublicKey(cryptoProvider2);

            // when
            var docValidatorService = GetDocVerificationService();
            var result = docValidatorService.ValidateDocument(patient1, doc1, signedMessage1, publicKeyPem2);

            // then
            result.HospitalSigned.Should().BeFalse();
            result.ForPatient.Should().BeTrue();
        }
    }
}
