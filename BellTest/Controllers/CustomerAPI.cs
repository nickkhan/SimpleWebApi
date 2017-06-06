using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BellTest.Models;

namespace BellTest.Controllers
{
    [Produces("application/json")]
    [Route("api/Customers")]
    public class CustomerAPI : Controller
    {
        private readonly CustomerDbContext _context;

        public CustomerAPI(CustomerDbContext context)
        {
            this._context = context;

            if (_context.Customers.Count() == 0)
            {
                string[] names = System.IO.File.ReadAllLines("CustomerNames.txt");

                foreach (var name in names)
                {
                    var customer = new Customer() { FirstName = name.Split()[0], LastName = name.Split()[1] };
                    _context.Customers.Add(customer);
                }
                _context.SaveChanges();
            }

        }

        // GET: api/Customers
        [HttpGet]
        public JsonResult Get(int page = 1, int per_page = 10)
        {
            if (page <= 0)
            {
                page = 1;
            }

            if(per_page <= 0)
            {
                per_page = 10;
            }

            int pageCount = _context.Customers.Count() / per_page;

            if(page == 1)
            {
                var data = _context.Customers.Take(page * per_page);
                return Json(data);
            }

           var list = _context.Customers.Skip((page-1)*per_page).Take(per_page);

            return Json(list);
        }
        
    }
}
