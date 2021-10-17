using PantryBackEnd.Models;
using System.Collections.Generic;
namespace PantryBackEnd.Repositories
{
    public interface IProduct
    {
        void AddProduct(Product Id);
        List<Product> fetchProducts(int index);
    }
}