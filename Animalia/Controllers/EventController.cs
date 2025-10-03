using Animalia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Animalia.Controllers
{
    public class EventController : ApiController
    {
        // GET api/event
        public List<Events> Get()
        {
            return new EventDao().SelectAll();
        }

        // GET api/event/5
        public Events Get(int id)
        {
            return new EventDao().Select(id);
        }

        // POST api/event
        public void Post([FromBody] Events evt)
        {
            new EventDao().Input(evt);
        }

        // PUT api/event/5
        public void Put([FromBody] Events evt)
        {
            new EventDao().Put(evt);
        }

        // DELETE api/event/5
        public void Delete(int id)
        {
            new EventDao().Delete(id);
        }
    }
}
