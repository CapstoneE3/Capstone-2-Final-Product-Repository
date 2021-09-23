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
            Account x = context.Accounts.Where(user => user.AccId == id).First();
            return x;
        }
        public Account GetAccountWithInventory(Guid id)
        {

            Account x = context.Accounts.Where(user => user.AccId == id).Include(a => a.InventoryLists).First();
            return x;
        }
    }
}