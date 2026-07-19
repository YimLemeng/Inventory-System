using project_pos.DAL;
using project_pos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.BLL
{
    public class UserBLL
    {
        private readonly UserDAL _userDAL = new UserDAL();
        public User AuthenticateUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new Exception("Please enter Username.");
            if (string.IsNullOrWhiteSpace(password)) throw new Exception("Please enter Password.");
            return _userDAL.Login(username.Trim(), password);
        }
    }
}
