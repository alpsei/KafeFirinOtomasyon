using SharedClass.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeFirinMaui.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<Products>> GetProductsAsync();
        Task<bool> AddProductAsync(Products product);
        Task<bool> UpdateProductAsync(Products product);
    }
}
