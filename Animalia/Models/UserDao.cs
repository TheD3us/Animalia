using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Animalia.Models
{
    public class UserDao
    {
        public List<Users> SelectAll()
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            return context.Users.ToList();
        }

        public Users Select(int id)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            return context.Users.Where(a => a.@Id == id).First();
        }

        public void Input(Users u)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            context.Users.Add(u);
            context.SaveChanges();
        }

        public void Put(Users u)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            context.Entry(u).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            Users u = Select(id);
            context.Users.Remove(u);
            context.SaveChanges();
        }

        //public int VerifLoginMdp(string login, string Mdp)
        //{
        //    AnimaliaDbEntities context = new AnimaliaDbEntities();
        //    return context.Users.Where(a => a.Email == login && a.Password == Mdp).First().Id;
            
        //}


        public Users VerifLoginMdp(string login, string mdp)
        {
            using (AnimaliaDbEntities context = new AnimaliaDbEntities())
            {
                Users user = context.Users
                                  .FirstOrDefault(a => a.Email == login && a.Password == mdp);

                if (user != null)
                {
                    // Ne jamais exposer le mot de passe
                    user.Password = null;
                }

                return user;
            }
        }

        public List<Events> SelectEventParticipation(int id)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();

            return context.Users.Where(u => u.Id == id).SelectMany(u => u.Events1).ToList();
        }

        public void EventSubscription(int idUser, int idEvent)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            var user = context.Users.Include(u => u.Events1).First(u => u.Id == idUser);
            var ev = context.Events.First(e => e.Id == idEvent);

            user.Events1.Add(ev);
            context.SaveChanges();
        }
    }
}