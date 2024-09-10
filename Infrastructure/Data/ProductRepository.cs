using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository(StoreContext context) : IProductRepository
    {
        /* Bu kod blogu primary constructor ile eşdeğer yani public class ProductRepository (StoreContext context) : IProductRepository
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            _context = context;
            
        }
        */

        public void AddProduct(Product product)
        {
           context.Products.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            context.Products.Remove(product);
        }

        public async Task<IReadOnlyList<string>> GetBrandsAsync()
        {
            return await context.Products.Select(x=>x.Brand)
            .Distinct()
            .ToListAsync();
        }

        public async Task<IReadOnlyList<Product>> GetProductAsync(string? brand, string? type, string? sort)
        {
            var query = context.Products.AsQueryable();

            
            if (!string.IsNullOrWhiteSpace(brand))
                query = query.Where(x=>x.Brand ==brand);
            if(!string.IsNullOrWhiteSpace(type))
                query= query.Where(x=>x.Type==type);
            
            query = sort switch {
                "priceAsc" => query.OrderBy(x=>x.Price),
                "priceDesc" => query.OrderByDescending (x=>x.Price),
                _=> query.OrderBy(x=>x.Name)
            };
            
            return await query.ToListAsync();
            
        
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await context.Products.FindAsync(id);
        }

        public async Task<IReadOnlyList<string>> GetTypesAsync()
        {
            return await context.Products.Select(x=>x.Type)
            .Distinct()
            .ToListAsync();
        }

        public bool ProductExists(int id)
        {
            return context.Products.Any(x=>x.ID==id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync()>0;
        }

        public void UpdateProduct(Product product)
        {
            context.Entry(product).State = EntityState.Modified;
        }
    }
}