using DAL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shop.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        DatabaseContext _db;

        public ProductController(DatabaseContext db)
        {
            _db = db;
        }
        // GET: api/product/getproducts
        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            return _db.Products.ToList();
        }

        //GET: api/product/getproduct/2

        [HttpGet("{id}")]
        public Product GetProduct(int id)
        {
            return _db.Products.Find(id);
        }



        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product model)
        {
            try
            {
                if (id != model.ProductId)
                    return BadRequest();

                _db.Products.Update(model);
                _db.SaveChanges();

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            Product model = _db.Products.Find(id);
            if (model != null)
            {
                _db.Products.Remove(model);
                _db.SaveChanges();
                return StatusCode(StatusCodes.Status200OK);
            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}
