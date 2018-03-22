using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using ODataWebApp.Data;

namespace ODataWebApp.Controllers
{
    [Route("api/[controller]")]
    /// <summary>
    /// 
    /// </summary>
    public class PersonsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PersonsController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Obtem
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery]
        public IQueryable<Person> Get()
        {
            return _db.Persons.AsQueryable();
        }
    }
}
