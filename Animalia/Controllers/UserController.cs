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

        [HttpPost]
        public void Post(Users u)
        {
            new UserDao().Input(u);
        }

        [HttpPut]
        public void Update(Users u)
        {
            new UserDao().Put(u);
        }

        public void Delete(int id)
        {
            new UserDao().Delete(id);
        }

        //[HttpGet]
        //[Route("api/user/verif")]
        //public int VerifLoginMdp([FromUri]string login, [FromUri]string mdp)
        //{
        //    return new UserDao().VerifLoginMdp(login, mdp);
        //}

        [HttpGet]
        [Route("api/user/verif")]
        public Users VerifLoginMdp([FromUri]string login, [FromUri]string mdp)
        {
            Users user = new UserDao().VerifLoginMdp(login, mdp);

           // return user; // renvoie { Id, Email, Prenom, Nom, IsAdmin }
            return new Users
            {
                Id = user.Id,
                Email = user.Email,
                Prenom = user.Prenom,
                Nom = user.Nom,
                IsAdmin = user.IsAdmin
            };

        }

        [HttpGet]
        [Route("api/user/events/{id:int}")]
        public List<Events> SelectEventParticipation(int id)
        {
            return new UserDao().SelectEventParticipation(id);
        }

        [HttpPost]
        [Route("api/user/subscribe/{idUser}/events/{idEvent}")]
        public void Subscribe(int idUser, int idEvent)
        {
            new UserDao().EventSubscription(idUser, idEvent);
        }
    }
}
