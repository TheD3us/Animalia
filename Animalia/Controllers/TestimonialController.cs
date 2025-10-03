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

        public List<Testimonials> Get()
        {
            return new TestimonialDao().SelectAll();
        }

        public Testimonials Get(int id)
        {
            return new TestimonialDao().Select(id);
        }

        public void Post(Testimonials t)
        {
            new TestimonialDao().Input(t);
        }

        public void Update(Testimonials t)
        {
            new TestimonialDao().Put(t);
        }

        public void Delete(int id)
        {
            new TestimonialDao().Delete(id);
        }
    }
}
