using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Animalia.Models
{
    public class TrainingDao
    {
        public List<Trainings> SelectAll()
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            return context.Trainings.ToList();
        }

        public Trainings Select(int id)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            return context.Trainings.Where(a => a.@Id == id).First();
        }

        public void Input(Trainings t)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            context.Trainings.Add(t);
            context.SaveChanges();
        }

        public void Put(Trainings t)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            context.Entry(t).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            Trainings t = Select(id);
            context.Trainings.Remove(t);
            context.SaveChanges();
        }
    }
}