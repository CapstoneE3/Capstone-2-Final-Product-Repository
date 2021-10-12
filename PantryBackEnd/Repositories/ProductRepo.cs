using PantryBackEnd.Models;
using System.Linq;
using System.Collections.Generic;
namespace PantryBackEnd.Repositories
{
    public class ProductRepo : IProduct
    {
        private pantryContext context;

        public ProductRepo(pantryContext context)
        {
            this.context = context;
        }

        public void AddProduct(Product dt)
        {
            context.Products.Add(dt);
            context.SaveChanges();
        }
        public List<Product> listloadProduct(int index)
        {
            return context.Products.Skip(index).Take(100).ToList();
        }



    }
}