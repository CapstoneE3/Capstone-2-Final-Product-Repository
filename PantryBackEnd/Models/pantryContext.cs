using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace PantryBackEnd.Models
{
    public partial class pantryContext : DbContext
    {
        public pantryContext()
        {
        }

        public pantryContext(DbContextOptions<pantryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<InventoryList> InventoryLists { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public virtual DbSet<RecipeList> RecipeLists { get; set; }
        public virtual DbSet<RecipeStep> RecipeSteps { get; set; }
        public virtual DbSet<ShoppingList> ShoppingLists { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=pantry;Username=postgres;Password=Kaizouku21");
            }
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "English_United States.1252");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccId)
                    .HasName("Account_pkey");

                entity.ToTable("Account");

                entity.HasIndex(e => e.Email, "Account_email_key")
                    .IsUnique();

                entity.Property(e => e.AccId)
                    .ValueGeneratedNever()
                    .HasColumnName("acc_ID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(127)
                    .HasColumnName("email");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("lastname");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("password");
            });

            modelBuilder.Entity<InventoryList>(entity =>
            {
                entity.HasKey(e => new { e.ItemId, e.AccId, e.ExpDate })
                    .HasName("Inventory_List_pkey");

                entity.ToTable("Inventory_List");

                entity.Property(e => e.ItemId)
                    .HasMaxLength(10)
                    .HasColumnName("item_ID");

                entity.Property(e => e.AccId).HasColumnName("acc_ID");

                entity.Property(e => e.ExpDate)
                    .HasColumnType("date")
                    .HasColumnName("exp_date");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.NotificationTime)
                    .HasColumnType("date")
                    .HasColumnName("notification_time");

                entity.HasOne(d => d.Acc)
                    .WithMany(p => p.InventoryLists)
                    .HasForeignKey(d => d.AccId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Inventory_List_acc_ID_fkey");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.InventoryLists)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Inventory_List_item_ID_fkey");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ItemId)
                    .HasName("Products_pkey");

                entity.Property(e => e.ItemId)
                    .HasMaxLength(10)
                    .HasColumnName("item_ID");

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("category");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.Property(e => e.Quantity)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("quantity");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.Property(e => e.RecipeId)
                    .ValueGeneratedNever()
                    .HasColumnName("recipe_ID");

                entity.Property(e => e.RecipeDescription)
                    .IsRequired()
                    .HasColumnName("recipe_Description");

                entity.Property(e => e.RecipeName)
                    .IsRequired()
                    .HasColumnName("recipe_name");
            });

            modelBuilder.Entity<RecipeIngredient>(entity =>
            {
                entity.HasKey(e => new { e.RecipeId, e.IngredientId })
                    .HasName("Recipe_Ingredients_pkey");

                entity.ToTable("Recipe_Ingredients");

                entity.Property(e => e.RecipeId).HasColumnName("recipe_ID");

                entity.Property(e => e.IngredientId)
                    .HasMaxLength(10)
                    .HasColumnName("ingredient_ID");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.UnitOfMeasure)
                    .IsRequired()
                    .HasColumnName("unit_of_measure");

                entity.HasOne(d => d.Ingredient)
                    .WithMany(p => p.RecipeIngredients)
                    .HasForeignKey(d => d.IngredientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Recipe_Ingredients_ingredient_ID_fkey");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeIngredients)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Recipe_Ingredients_recipe_ID_fkey");
            });

            modelBuilder.Entity<RecipeList>(entity =>
            {
                entity.HasKey(e => new { e.RecipeId, e.AccId })
                    .HasName("Recipe_List_pkey");

                entity.ToTable("Recipe_List");

                entity.Property(e => e.RecipeId).HasColumnName("recipe_ID");

                entity.Property(e => e.AccId).HasColumnName("acc_ID");

                entity.HasOne(d => d.Acc)
                    .WithMany(p => p.RecipeLists)
                    .HasForeignKey(d => d.AccId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Recipe_List_acc_ID_fkey");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeLists)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Recipe_List_recipe_ID_fkey");
            });

            modelBuilder.Entity<RecipeStep>(entity =>
            {
                entity.HasKey(e => e.RecipeId)
                    .HasName("recipe_Steps_pkey");

                entity.ToTable("Recipe_Steps");

                entity.Property(e => e.RecipeId)
                    .ValueGeneratedNever()
                    .HasColumnName("recipe_ID");

                entity.Property(e => e.Instructions)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("instructions");

                entity.Property(e => e.StepId).HasColumnName("step_ID");

                entity.HasOne(d => d.Recipe)
                    .WithOne(p => p.RecipeStep)
                    .HasForeignKey<RecipeStep>(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("recipe_Steps_recipe_ID_fkey");
            });

            modelBuilder.Entity<ShoppingList>(entity =>
            {
                entity.HasKey(e => new { e.ItemId, e.AccId })
                    .HasName("Shopping_List_pkey");

                entity.ToTable("Shopping_List");

                entity.Property(e => e.ItemId)
                    .HasMaxLength(10)
                    .HasColumnName("item_ID");

                entity.Property(e => e.AccId).HasColumnName("acc_ID");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.HasOne(d => d.Acc)
                    .WithMany(p => p.ShoppingLists)
                    .HasForeignKey(d => d.AccId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Shopping_List_acc_ID_fkey");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ShoppingLists)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Shopping_List_item_ID_fkey");
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(e => e.AccId)
                    .HasName("Subscription_pkey");

                entity.ToTable("Subscription");

                entity.Property(e => e.AccId)
                    .ValueGeneratedNever()
                    .HasColumnName("acc_ID");

                entity.Property(e => e.Audh)
                    .IsRequired()
                    .HasColumnName("audh");

                entity.Property(e => e.ExpNotif)
                    .HasColumnType("date")
                    .HasColumnName("exp_notif");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasColumnName("key");

                entity.Property(e => e.SubEndpoint)
                    .IsRequired()
                    .HasColumnName("sub_endpoint");

                entity.HasOne(d => d.Acc)
                    .WithOne(p => p.Subscription)
                    .HasForeignKey<Subscription>(d => d.AccId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Subscription_acc_ID_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
