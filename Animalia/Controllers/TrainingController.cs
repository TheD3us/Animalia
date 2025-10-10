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

        [Route("api/training/getuser")]
        public List<Trainings> GetByUser(int id)
        {
            return new TrainingDao().GetByUser(id);
        }
    }
}
