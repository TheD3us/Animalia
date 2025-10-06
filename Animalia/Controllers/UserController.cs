using Animalia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Animalia.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        public List<Users> Get()
        {
            return new UserDao().SelectAll();
        }

        public Users Get(int id)
        {
            return new UserDao().Select(id);
        }

        public void Post(Users u)
        {
            new UserDao().Input(u);
        }

        public void Update(Users u)
        {
            new UserDao().Put(u);
        }

        public void Delete(int id)
        {
            new UserDao().Delete(id);
        }

        [Route("api/user/verif")]
        public bool VerifLoginMdp(string login, string Mdp)
        {
            return new UserDao().VerifLoginMdp(login, Mdp);
        }
    }
}
