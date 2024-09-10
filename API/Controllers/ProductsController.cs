using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using API.RequestHelpers;
using Infrastructure;

namespace API.Controllers
{
  
    public class ProductsController(IUnitOfWork unit) : BaseApiController
    {
        /* aşağıdaki kod bloğu yerine primary constructor gerçekleştirdik 
        private readonly StoreContext context;

        public ProductsController(StoreContext context)
        {
            this.context = context;
        }
        */
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProductSpecParams productSpecParams)
        {
            //[FromQuery] özniteliği, parametreleri HTTP sorgu dizesinden (query string) almak için kullanılır.
            /*
            ASP.NET Core, gelen sorgu dizesi parametrelerini ProductSpecParams sınıfının uygun özelliklerine otomatik olarak eşler. 
            Bu, manuel olarak parametre okuma ve eşleştirme ihtiyacını ortadan kaldırır.*/ 
            var spec = new ProductSpecification(productSpecParams);
            
            return await CreatePagedResult(unit.Repository<Product>(), spec, productSpecParams.PageIndex, productSpecParams.PageSize);
            
            
        }

        [HttpGet("{id:int}")] //api/products/2
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await unit.Repository<Product>().GetByIdAsync(id);
            if (product == null) return NotFound();
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            unit.Repository<Product>().Add(product);
            if (await unit.Complete())
            {
                return CreatedAtAction("GetProduct", new{id =product.ID}, product);
            }
            
            return BadRequest("Problem creating product");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
        {
            if (product.ID != id || !ProductExists(id))
            {
                return BadRequest("Cannot update this product");
            }
            unit.Repository<Product>().Update(product);
            if (await unit.Complete())
            {
                return NoContent();
            }
            return BadRequest("Problem updating the product");

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await unit.Repository<Product>().GetByIdAsync(id);
            if (product == null) return NotFound();
            unit.Repository<Product>().Remove(product);
            if (await unit.Complete())
            {
                return NoContent();
            }
            return BadRequest("Problem deleting the product");
        }

        private bool ProductExists(int id)
        {
            return unit.Repository<Product>().Exists(id);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecification();
            return Ok(await unit.Repository<Product>().ListAsync(spec));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            return Ok(await unit.Repository<Product>().ListAsync(spec));
        }
    }
}