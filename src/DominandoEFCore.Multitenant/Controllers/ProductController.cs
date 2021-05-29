using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominandoEFCore.Multitenant.Data;
using DominandoEFCore.Multitenant.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DominandoEFCore.Multitenant.Controllers
{
    [ApiController]
    [Route("{tenant}/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Product> Get([FromServices]ApplicationContext db)
        {
            var products = db.Products.ToArray();
            return products;
        }
    }
}