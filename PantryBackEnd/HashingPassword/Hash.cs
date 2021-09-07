using System;
using System.Text;
using System.Security.Cryptography;
namespace PantryBackEnd.HashingPassword
{
    public class Hash
    {
       public string HashPass(string password)
        {
            using(SHA256 hash = SHA256.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = hash.ComputeHash(sourceBytes);
                string hashPassword = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                return hashPassword;
            }
        }
        public bool Verify(string pasword,string Realpassword)
        {
            return HashPass(pasword).Equals(Realpassword);
        }
    }
}