using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
namespace TaskManagement.Domain.Interfaces
{
    public interface IHelperMethods
    {
        PasswordHashResult HashPassword(string password);
         bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt);
    }
}
