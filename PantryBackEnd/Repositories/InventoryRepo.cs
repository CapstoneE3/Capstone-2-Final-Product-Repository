using PantryBackEnd.Models;
namespace PantryBackEnd.Repositories
{
        
    public class InventoryRepo : IInventoryRepo
    {
        private pantryContext context;
        public InventoryRepo(pantryContext context)
        {
            this.context = context;
        }
        public void AddProduct(InventoryList product)
        {
            context.InventoryLists.Add(product);
            context.SaveChanges();
        }
    }
}