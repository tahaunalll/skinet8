using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Core.Interfaces;
using Core.Specifications;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IGenericRepository<Product> repo) : ControllerBase
    {
        /* aşağıdaki kod bloğu yerine primary constructor gerçekleştirdik 
        private readonly StoreContext context;

        public ProductsController(StoreContext context)
        {
            this.context = context;
        }
        */
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            var spec = new ProductSpecification(brand, type, sort);
            var products = await repo.ListAsync(spec);
            return Ok (products);
        }

        [HttpGet("{id:int}")] //api/products/2
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await repo.GetByIdAsync(id);
            if (product == null) return NotFound();
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repo.Add(product);
            if (await repo.SaveAllAsync())
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
            repo.Update(product);
            if (await repo.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Problem updating the product");

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await repo. GetByIdAsync(id);
            if (product == null) return NotFound();
            repo.Remove(product);
            if (await repo.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Problem deleting the product");
        }

        private bool ProductExists(int id)
        {
            return repo.Exists(id);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecification();
            return Ok(await repo.ListAsync(spec));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            return Ok(await repo.ListAsync(spec));
        }
    }
}