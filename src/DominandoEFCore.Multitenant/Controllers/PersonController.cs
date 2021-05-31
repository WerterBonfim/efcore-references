using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominandoEFCore.Multitenant.Data;
using DominandoEFCore.Multitenant.Domain;
using DominandoEFCore.Multitenant.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DominandoEFCore.Multitenant.Controllers
{
    [ApiController]
    [Route("{tenant}/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;

        public PersonController(ILogger<PersonController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Person> Get([FromServices]ApplicationContext db)
        {
            var people = db.People.ToArray();
            return people;
        }

        [HttpPost]
        public IActionResult Post([FromServices] ApplicationContext db, [FromBody]PersonViewModel person)
        {
            db.People.Add(new Person{ Name =  person.Name});
            db.SaveChanges();

            
            
            return Ok(person);
        }
    }
}