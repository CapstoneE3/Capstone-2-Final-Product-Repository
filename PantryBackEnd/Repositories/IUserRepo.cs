using PantryBackEnd.Models;
using System;
namespace PantryBackEnd.Repositories
{
    public interface IUserRepo
    {
        Account GetByEmail(string email);

        Account GetByID(Guid id );
        Account Register(Account user);
    }
}