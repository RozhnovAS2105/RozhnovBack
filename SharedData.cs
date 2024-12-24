using RozhnovBack.Models;
using System.Collections.Generic;

namespace RozhnovBack
{
    public static class SharedData
    {
        public static List<User> Users { get; } = new List<User>
        {
            new User(){ Login = "user", Password = "user" },
            new User(){ Login = "admin", Password = "admin" },
        };
    }
}