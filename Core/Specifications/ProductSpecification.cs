using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductSpecification:BaseSpecification<Product>
    {
         public ProductSpecification(ProductSpecParams productSpecParams):base(x=>
         (string.IsNullOrEmpty(productSpecParams.Search)||x.Name.ToLower().Contains(productSpecParams.Search))&&
         (productSpecParams.Brands.Count==0 || productSpecParams.Brands.Contains(x.Brand)) &&
         (productSpecParams.Types.Count==0 || productSpecParams.Types.Contains(x.Type))
         )
         {
            ApplyPaging(productSpecParams.PageSize * (productSpecParams.PageIndex-1), productSpecParams.PageSize);
            /*
            productSpecParams.PageSize * (productSpecParams.PageIndex - 1):

Bu ifade, atlanması gereken öğe sayısını hesaplar.
Örneğin, PageSize = 10 ve PageIndex = 2 olduğunda, (10 * (2 - 1)) = 10 ifadesi, 
ilk 10 öğenin atlanması gerektiğini belirtir. Bu durumda, ikinci sayfa görüntülenecektir.
SQL veya LINQ sorgusu, LIMIT 10 OFFSET 20 veya Skip(20).Take(10) şeklinde uygulanır.
*/
            switch (productSpecParams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(x=>x.Price); 
                    break;
                case "priceDesc":
                    AddOrderByDescending(x=>x.Price);
                    break;
                default:
                    AddOrderBy(x=>x.Name);
                    break;
            }
         }
    }
}