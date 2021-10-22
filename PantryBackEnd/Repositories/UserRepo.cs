using PantryBackEnd.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

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
            return context.Accounts.Single(u => u.Email.Equals(email));
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
            Account x = context.Accounts.Where(user => user.AccId == id).Single();
            return x;
        }
        public Account GetAccountWithInv(Guid id)
        {
            Account x = context.Accounts.Where(user => user.AccId == id).Include(a => a.InventoryLists).Single();
            return x;
        }

        public string removeUser(Account user)
        {

            return "";
        }
    }
}