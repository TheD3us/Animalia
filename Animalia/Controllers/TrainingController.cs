using Animalia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Animalia.Controllers
{
    public class TrainingController : ApiController
    {
        public List<Trainings> Get()
        {
            return new TrainingDao().SelectAll();
        }

        public Trainings Get(int id)
        {
            return new TrainingDao().Select(id);
        }

        public void Post(Trainings t)
        {
            new TrainingDao().Input(t);
        }

        public void Update(Trainings t)
        {
            new TrainingDao().Put(t);
        }

        public void Delete(int id)
        {
            new TrainingDao().Delete(id);
        }
    }
}
