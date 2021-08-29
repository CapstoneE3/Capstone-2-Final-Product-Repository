namespace PantryBackEnd.Models
{
    public class Users
    {
        
        private string name;
        private string password;

        private ShoppingList userShoppingList;
        private Inventory userInventory;

        public Users(string name, string password, ShoppingList userShoppingList, Inventory userInventory)
        {
            this.name = name;
            this.password = password;
            this.userShoppingList = userShoppingList;
            this.userInventory = userInventory;
        }
    }
}