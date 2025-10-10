using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

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

        public bool VerifLoginMdp(string login, string Mdp)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            Users u = context.Users.Where(a => a.Email == login && a.Password == Mdp).First();
            return u != null;
            
        }
    }
}