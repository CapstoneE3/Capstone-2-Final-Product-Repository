using PantryBackEnd.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
namespace PantryBackEnd.Repositories
{
    public class UserRepo : IUserRepo
    {
        private pantryContext context;
        public UserRepo(pantryContext context)
        {
            this.context = context;
        }
        public Account GetByEmail(string email)
        {
            return context.Accounts.FirstOrDefault(u => u.Email.Equals(email));
        }
        public Account Register(Account user)
        {
            context.Accounts.Add(user);
            context.SaveChanges();
            user = GetByEmail(user.Email);
            return user;
        }
        public Account GetByID(Guid id)
        {
            Account x = context.Accounts.Where(user => user.AccId == id).FirstOrDefault();
            return x;
        }
    }
}