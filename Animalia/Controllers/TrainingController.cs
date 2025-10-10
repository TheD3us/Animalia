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

        [HttpPost]
        public void Post([FromBody] Trainings t)
        {
            new TrainingDao().Input(t);
        }

        [HttpPut]
        public void Update([FromBody] Trainings t)
        {
            new TrainingDao().Put(t);
        }

        [HttpDelete]
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
