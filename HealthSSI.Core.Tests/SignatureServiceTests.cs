using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace HealthSSI.Core.Tests
{
    [TestClass]
    public class SignatureServiceTests
    {
        private ISignatureService GetSignatureService()
        {
            return new SignatureService();
        }

        /// <summary>
        /// Helper method for getting public + private key pair for testing.
        /// In real scenario, key pairs will be provided by participants and they will secure their private key and provide their public keys
        /// </summary>
        /// <returns></returns>
        private RSACryptoServiceProvider GetCryptoProvider()
        {
            return new RSACryptoServiceProvider(2048);
        }

        /// <summary>
        /// Helper method for serizling an object into json string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string ObjectToJson(Document document)
        {
            return JsonConvert.SerializeObject(document);
        }

        /// <summary>
        /// Helper method for signing data with a private key. 
        /// This will be done by the hospital outside the system
        /// </summary>
        /// <param name="message"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        private string SignData(string message, RSAParameters privateKey)
        {
            ASCIIEncoding byteConverter = new ASCIIEncoding();

            byte[] signedBytes;

            using (var rsa = new RSACryptoServiceProvider())
            {
                // Write the message to a byte array using ASCII as the encoding.
                byte[] originalData = byteConverter.GetBytes(message);

                try
                {
                    // Import the private key used for signing the message
                    rsa.ImportParameters(privateKey);

                    // Sign the data, using SHA512 as the hashing algorithm 
                    signedBytes = rsa.SignData(originalData, CryptoConfig.MapNameToOID("SHA512"));
                }
                catch (CryptographicException e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
                finally
                {
                    // Set the keycontainer to be cleared when rsa is garbage collected.
                    rsa.PersistKeyInCsp = false;
                }
            }
            // Convert the byte array back to a string message
            return Convert.ToBase64String(signedBytes);
        }

        /// <summary>
        /// https://stackoverflow.com/a/23739932/2860309
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="length"></param>
        private static void EncodeLength(BinaryWriter stream, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "Length must be non-negative");
            if (length < 0x80)
            {
                // Short form
                stream.Write((byte)length);
            }
            else
            {
                // Long form
                var temp = length;
                var bytesRequired = 0;
                while (temp > 0)
                {
                    temp >>= 8;
                    bytesRequired++;
                }
                stream.Write((byte)(bytesRequired | 0x80));
                for (var i = bytesRequired - 1; i >= 0; i--)
                {
                    stream.Write((byte)(length >> (8 * i) & 0xff));
                }
            }
        }

        /// <summary>
        /// https://stackoverflow.com/a/23739932/2860309
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        /// <param name="forceUnsigned"></param>
        private static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value, bool forceUnsigned = true)
        {
            stream.Write((byte)0x02); // INTEGER
            var prefixZeros = 0;
            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] != 0) break;
                prefixZeros++;
            }
            if (value.Length - prefixZeros == 0)
            {
                EncodeLength(stream, 1);
                stream.Write((byte)0);
            }
            else
            {
                if (forceUnsigned && value[prefixZeros] > 0x7f)
                {
                    // Add a prefix zero to force unsigned if the MSB is 1
                    EncodeLength(stream, value.Length - prefixZeros + 1);
                    stream.Write((byte)0);
                }
                else
                {
                    EncodeLength(stream, value.Length - prefixZeros);
                }
                for (var i = prefixZeros; i < value.Length; i++)
                {
                    stream.Write(value[i]);
                }
            }
        }

        /// <summary>
        /// helper method for getting pub key as pem.
        /// Hospital will provide this value
        /// </summary>
        /// <param name="csp"></param>
        /// <returns></returns>
        private string ExportPublicKey(RSACryptoServiceProvider csp)
        {
            StringWriter outputStream = new StringWriter();
            var parameters = csp.ExportParameters(false);
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write((byte)0x30); // SEQUENCE
                using (var innerStream = new MemoryStream())
                {
                    var innerWriter = new BinaryWriter(innerStream);
                    innerWriter.Write((byte)0x30); // SEQUENCE
                    EncodeLength(innerWriter, 13);
                    innerWriter.Write((byte)0x06); // OBJECT IDENTIFIER
                    var rsaEncryptionOid = new byte[] { 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01 };
                    EncodeLength(innerWriter, rsaEncryptionOid.Length);
                    innerWriter.Write(rsaEncryptionOid);
                    innerWriter.Write((byte)0x05); // NULL
                    EncodeLength(innerWriter, 0);
                    innerWriter.Write((byte)0x03); // BIT STRING
                    using (var bitStringStream = new MemoryStream())
                    {
                        var bitStringWriter = new BinaryWriter(bitStringStream);
                        bitStringWriter.Write((byte)0x00); // # of unused bits
                        bitStringWriter.Write((byte)0x30); // SEQUENCE
                        using (var paramsStream = new MemoryStream())
                        {
                            var paramsWriter = new BinaryWriter(paramsStream);
                            EncodeIntegerBigEndian(paramsWriter, parameters.Modulus); // Modulus
                            EncodeIntegerBigEndian(paramsWriter, parameters.Exponent); // Exponent
                            var paramsLength = (int)paramsStream.Length;
                            EncodeLength(bitStringWriter, paramsLength);
                            bitStringWriter.Write(paramsStream.GetBuffer(), 0, paramsLength);
                        }
                        var bitStringLength = (int)bitStringStream.Length;
                        EncodeLength(innerWriter, bitStringLength);
                        innerWriter.Write(bitStringStream.GetBuffer(), 0, bitStringLength);
                    }
                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);
                    writer.Write(innerStream.GetBuffer(), 0, length);
                }

                var base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length).ToCharArray();
                // WriteLine terminates with \r\n, we want only \n
                outputStream.Write("-----BEGIN PUBLIC KEY-----\n");
                for (var i = 0; i < base64.Length; i += 64)
                {
                    outputStream.Write(base64, i, Math.Min(64, base64.Length - i));
                    outputStream.Write("\n");
                }
                outputStream.Write("-----END PUBLIC KEY-----");
            }

            return outputStream.ToString();
        }

        [TestMethod]
        public void Checking_Message_That_Was_Signed_By_Hospital_For_Should_Return_Success()
        {
            // given
            var cryptoProvider = GetCryptoProvider();
            RSAParameters rsaPrivateKeyInfo = cryptoProvider.ExportParameters(true);
            Patient patient = new Patient();
            Document doc = new Document(DateTime.Now, patient.Id);
            var message = ObjectToJson(doc);
            string signedMessage = SignData(message, rsaPrivateKeyInfo);
            string publicKeyPem = ExportPublicKey(cryptoProvider);

            // when
            var signatureService = GetSignatureService();
            RSACryptoServiceProvider importedKey = signatureService.ImportPublicKey(publicKeyPem);
            var result = signatureService.VerifySignature(message, signedMessage, importedKey.ExportParameters(false));

            // then
            result.Should().BeTrue();
        }

        [TestMethod]
        public void Checking_Message_That_Was_Not_Signed_By_Hospital_Should_Not_Return_Success()
        {
            // given
            var cryptoProvider = GetCryptoProvider();
            RSAParameters rsaPrivateKeyInfo = cryptoProvider.ExportParameters(true);
            Patient patient = new Patient();
            Document doc = new Document(DateTime.Now, patient.Id);
            var message = ObjectToJson(doc);
            string signedMessage = SignData(message, rsaPrivateKeyInfo);

            // Keypair not signed with
            var unusedCryptoProvider = GetCryptoProvider();
            string unusedPublicKeyPem = ExportPublicKey(unusedCryptoProvider);

            // when
            var signatureService = GetSignatureService();
            RSACryptoServiceProvider importedKey = signatureService.ImportPublicKey(unusedPublicKeyPem);
            var result = signatureService.VerifySignature(message, signedMessage, importedKey.ExportParameters(false));

            // then
            result.Should().BeFalse();
        }
    }
}
