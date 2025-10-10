using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Animalia.Models
{
    public class EventDao
    {
        public List<Events> SelectAll()
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            return context.Events.Include(e => e.Users).ToList();
        }

        public Events Select(int id)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            return context.Events.Where(e => e.Id == id).First();
        }

        public void Input(Events evt)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            context.Events.Add(evt);
            context.SaveChanges();
        }

        public void Put(Events evt)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            context.Entry(evt).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            Events evt = context.Events.Where(e => e.Id == id).First();
            context.Events.Remove(evt);
            context.SaveChanges();
        }
    }
}