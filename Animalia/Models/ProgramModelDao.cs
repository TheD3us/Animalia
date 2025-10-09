using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Animalia.Models
{
    public class ProgramModelDao
    {
        public List<ProgramModels> SelectAll()
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            return context.ProgramModels.Include(p => p.Trainings).ToList();
        }

        public ProgramModels Select(int id)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            return context.ProgramModels.Where(p => p.Id == id).First();
        }

        public void Input(ProgramModels program)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            context.ProgramModels.Add(program);
            context.SaveChanges();
        }

        public void Put(ProgramModels program)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            context.Entry(program).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            ProgramModels program = context.ProgramModels.Where(p => p.Id == id).First();
            context.ProgramModels.Remove(program);
            context.SaveChanges();
        }

        public List<Trainings> GetTrainingByProgram(int id)
        {
            AnimaliaDbEntities context = new AnimaliaDbEntities();
            return context.ProgramModels.Where(p => p.Id == id).SelectMany(t => t.Trainings).ToList();
        }
    }
}