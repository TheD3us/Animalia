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
    public class ProgramModelController : ApiController
    {
        // GET api/programmodel
        public List<ProgramModels> Get()
        {
            return new ProgramModelDao().SelectAll();
        }

        // GET api/programmodel/5
        public ProgramModels Get(int id)
        {
            return new ProgramModelDao().Select(id);
        }

        // POST api/programmodel
        public void Post([FromBody] ProgramModels program)
        {
            new ProgramModelDao().Input(program);
        }

        // PUT api/programmodel/5
        public void Put([FromBody] ProgramModels program)
        {
            new ProgramModelDao().Put(program);
        }

        // DELETE api/programmodel/5
        public void Delete(int id)
        {
            new ProgramModelDao().Delete(id);
        }
    }
}
