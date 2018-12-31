using System.Collections.Generic;
using ImageManager.API.Entities;

namespace ImageManager.API.Interfaces
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
    }
}