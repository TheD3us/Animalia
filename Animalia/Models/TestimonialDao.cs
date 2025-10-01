using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Animalia.Models
{
    public class TestimonialDao
    {
        public List<Testimonials> SelectAll()
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            return context.Testimonials.ToList();
        }

        public Testimonials Select(int id)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            return context.Testimonials.Where(a => a.@Id == id).First();
        }

        public void Input(Testimonials t)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            context.Testimonials.Add(t);
            context.SaveChanges();
        }

        public void Put(Testimonials t)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            context.Entry(t).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            Testimonials t = Select(id);
            context.Testimonials.Remove(t);
            context.SaveChanges();
        }
    }
}