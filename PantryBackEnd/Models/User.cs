using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PantryBackEnd.Models
{
    public class User
    {
        public Guid userID;

        public User(Guid userID)
        {
            this.userID = userID;
        }
    }
}