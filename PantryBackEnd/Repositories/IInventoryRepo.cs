using PantryBackEnd.Models;
namespace PantryBackEnd.Repositories
{
    public interface IInventoryRepo
    {
        void AddProduct(InventoryList product);
    }
}