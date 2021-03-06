﻿using System;
using System.Security.Cryptography;
using FluentAssertions;
using HealthCareSSI.Tests.Common;
using HealthSSI.Data;
using HealthSSI.Data.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HealthSSI.Core.Tests
{
    [TestClass]
    public class DocumentValidationTests : BaseCoreTest
    {


        [TestMethod]
        public void Checking_Message_That_Was_Signed_By_Hospital_For_A_Specific_Patient_Should_Return_Success()
        {
            // given 
            var cryptoProvider = GetCryptoProvider();
            RSAParameters rsaPrivateKeyInfo = cryptoProvider.ExportParameters(true);
            string publicKeyPem = ExportPublicKey(cryptoProvider);
            Patient patient = new Patient("joe", "smith", "joe.smith@gmail.com");

            // simulate having an ID from the DB
            Hospital hospital = new Hospital("Acme Hospital", publicKeyPem);
            patient.Id = 123;
            Document doc = new Document(patient.Id, hospital);
            var message = doc.ToJson();
            string signedMessage = SignData(message, rsaPrivateKeyInfo);

            // when
            var docValidatorService = GetDocVerificationService();
            var result = docValidatorService.ValidateDocument(patient.Id, doc, signedMessage, publicKeyPem);

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
            string publicKeyPem = ExportPublicKey(cryptoProvider);
            Patient patient1 = new Patient("joe", "smith", "joe.smith@gmail.com");

            // simulate having an ID from the DB
            patient1.Id = 123;

            Patient patient2 = new Patient("jon", "smith", "jon.smith@gmail.com");
            patient2.Id = 124;

            // simulate having an ID from the DB
            Hospital hospital = new Hospital("Acme Hospital", publicKeyPem);
            
            Document doc = new Document(patient2.Id, hospital);
            var message = doc.ToJson();
            string signedMessage = SignData(message, rsaPrivateKeyInfo);

            // when
            var docValidatorService = GetDocVerificationService();
            var result = docValidatorService.ValidateDocument(patient1.Id, doc, signedMessage, publicKeyPem);

            // then
            result.Success.Should().BeFalse();
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
            string publicKeyPem1 = ExportPublicKey(cryptoProvider1);
            string publicKeyPem2 = ExportPublicKey(cryptoProvider2);
            Patient patient1 = new Patient("jon", "smith", "jon.smith@gmail.com");

            // simulate having an ID from the DB
            

            Hospital hospital = new Hospital("Acme Hospital", publicKeyPem1);
            patient1.Id = 124;
            Document doc1 = new Document(patient1.Id, hospital);
            var message1 = doc1.ToJson();
            string signedMessage1 = SignData(message1, rsaPrivateKeyInfo1);

            Document doc2 = new Document(patient1.Id, hospital);
            var message2 = doc2.ToJson();
            string signedMessage2 = SignData(message2, rsaPrivateKeyInfo2);

            // when
            var docValidatorService = GetDocVerificationService();
            var result = docValidatorService.ValidateDocument(patient1.Id, doc1, signedMessage1, publicKeyPem2);

            // then
            result.Success.Should().BeFalse();
            result.HospitalSigned.Should().BeFalse();
            result.ForPatient.Should().BeTrue();
        }
    }
}
