using PantryBackEnd.Models;
using System.Linq;
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


    }
}