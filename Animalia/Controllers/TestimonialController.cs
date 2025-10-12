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
    public class TestimonialController : ApiController
    {
        [HttpGet]
        public List<Testimonials> Get()
        {
            return new TestimonialDao().SelectAll();
        }

        [HttpGet]
        public Testimonials Get(int id)
        {
            return new TestimonialDao().Select(id);
        }

        [HttpPost]
        public void Post([FromBody]Testimonials t)
        {
            new TestimonialDao().Input(t);
        }

        [HttpPut]
        public void Update([FromBody]Testimonials t)
        {
            new TestimonialDao().Put(t);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            new TestimonialDao().Delete(id);
        }
    }
}
