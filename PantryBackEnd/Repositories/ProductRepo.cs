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
        
        public Product getProductById(string Id)
        {
            Product x =context.Products.FirstOrDefault( i => i.ItemId.Equals(Id));
            return x;
        }
    
    }
}