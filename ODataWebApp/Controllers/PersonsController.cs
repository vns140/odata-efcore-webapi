using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using ODataWebApp.Data;

namespace ODataWebApp.Controllers
{
    public class PersonsController : ODataController
    {
        private readonly ApplicationDbContext _db;

        public PersonsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [EnableQuery]
        public IQueryable<Person> Get()
        {
            return _db.Persons.AsQueryable();
        }
    }
}
