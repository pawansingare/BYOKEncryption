using BYOK_Encryption.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace BYOK_Encryption.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BYOK_EncryptionController : ControllerBase
    {
        private readonly AppDbContext context;
        public BYOK_EncryptionController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpPost("GetTenantWiseDataEncrypted")]
        public Employee GetTenantWiseDataEncrypted(Employee employee, int Id)
        {
           var tenantList = context.bYOK_Enabled_Tenants.Where(e => e.TenantId == Id).ToList();

            foreach (var item in tenantList)
            {
                if(item.Table_name == TableEnum.Employee)
                {
                    if(item.Column_name == ColumnName.FirstName)
                    {
                        employee.First_name = AddDataForEncryption(employee.First_name);

                    }
                    if (item.Column_name == ColumnName.LastName)
                    {
                        employee.last_name = AddDataForEncryption(employee.last_name);

                    }
                    if (item.Column_name == ColumnName.Address)
                    {
                        employee.Address = AddDataForEncryption(employee.Address);

                    }


                }
            }
            return employee;
            context.AddAsync(employee);
            context.SaveChanges();

        }




        [HttpPost]
        public string AddDataForEncryption(string text)
        {
            var key = "E546C8DF278CD5931069B522E695D4F2";
            var encrypted = EncryptString(text, key);

            return encrypted;
        }

        [HttpPost("get")]
        public string AddDataForDecryption(string text)
        {
            var key = "E546C8DF278CD5931069B522E695D4F2";
            var decrypted = DecryptString(text, key);

            return decrypted;
        }


        public static string EncryptString(string text, string keyString)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public static string DecryptString(string cipherText, string keyString)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
    }
}
