using System;
using System.Security.Cryptography;
using System.Text;

namespace ABPvNextOrangeAdmin.Utils;

public class CryptoUtils
{
    private const string PublicKey =
        @"-----BEGIN PUBLIC KEY-----
MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBANynxrsrRE6ZKBK06iPI5eP+P4T/7lc3
WkB3ewzra/+Dw7XLb11ZUhRScKSRyPDjwMsAXAiIJ+yJk1ul1mmDl68CAwEAAQ==

-----END PUBLIC KEY-----";

    private const string PrivateKey =
        @"-----BEGIN PRIVATE KEY-----
MIIBVAIBADANBgkqhkiG9w0BAQEFAASCAT4wggE6AgEAAkEA3KfGuytETpkoErTq
I8jl4/4/hP/uVzdaQHd7DOtr/4PDtctvXVlSFFJwpJHI8OPAywBcCIgn7ImTW6XW
aYOXrwIDAQABAkAlfzltWyfrd2lw7F+RnzU57l3a+ycEmTp0FBnME0GyFOA2h5jj
vwGR2vixPDUkJhYC0ozFEXc1M3jKcKw5xjlRAiEA/t2+1yztkQRXE16laJa3ih09
pTvGPvlQ7Gtp1SASP7kCIQDdoxHfKdOB98PFQlEYJMUdoCmoHspomfjpL/cTjQM2
pwIgWX27WQrpkBYaDS8anZLud4y07KQEhHA+vgUpcDCGt+ECIQCso/4iz+jB3yXu
fIbAgLvOJNjt7PYLXoxFz6fs4bV0FwIgFibwrhdJ7C3B65XWuYDLht6c4kE12ISB
uYa5yYwkt8Y=
-----END PRIVATE KEY-----";


    public static String EncryptRSA(string plainText)
    {
        using (RSACryptoServiceProvider cryptoProvider = new RSACryptoServiceProvider())
        {
            cryptoProvider.ImportFromPem(PublicKey);;
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(cryptoProvider.Encrypt(plainBytes, false));
        }
    }


    public static string DecryptRSA(String encryptedData)
    {
        using (RSACryptoServiceProvider cryptoProvider = new RSACryptoServiceProvider())
        {
            cryptoProvider.ImportFromPem(PrivateKey);
            byte[] plainBytes = cryptoProvider.Decrypt(Convert.FromBase64String(encryptedData), false);
            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}