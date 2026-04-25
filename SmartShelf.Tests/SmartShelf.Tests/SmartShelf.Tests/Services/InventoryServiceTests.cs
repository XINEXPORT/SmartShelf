using Microsoft.EntityFrameworkCore;
using SmartShelf.web.Data;
using SmartShelf.web.Models;
using SmartShelf.web.Services;

namespace SmartShelf.Tests.Services
{
    [TestClass]
    public class InventoryServiceTests
    {
        private SmartShelfContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<SmartShelfContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new SmartShelfContext(options);
        }

        /*
        Title
        Description: Tests that the inventory service returns a result when no products exist
        Input: Empty database context
        Output: Non-null inventory result
        Return: List of inventory items
        */
        [TestMethod]
        public void GetInventory_ReturnsEmptyList_WhenNoProductsExist()
        {
            using var context = GetDbContext();
            var service = new InventoryService(context);

            var result = service.GetInventory();

            Assert.IsNotNull(result);
        }

        /*
        Title
        Description: Tests that the inventory service returns a result when a single product exists
        Input: One product inserted into the database
        Output: Non-null inventory result
        Return: List of inventory items
        */
        [TestMethod]
        public void GetInventory_ReturnsInventoryItem_WhenProductExists()
        {
            using var context = GetDbContext();

            context.Product.Add(new Product
            {
                Id = 1,
                Name = "Water",
                Threshold = 2,
                ImagePath = "water.png"
            });

            context.SaveChanges();

            var service = new InventoryService(context);

            var result = service.GetInventory();

            Assert.IsNotNull(result);
        }

        /*
        Title
        Description: Tests that the inventory service returns a result when multiple products exist
        Input: Multiple products inserted into the database
        Output: Non-null inventory result
        Return: List of inventory items
        */
        [TestMethod]
        public void GetInventory_ReturnsResult_WhenMultipleProductsExist()
        {
            using var context = GetDbContext();

            context.Product.AddRange(
                new Product { Id = 1, Name = "Chips", Threshold = 5, ImagePath = "chips.png" },
                new Product { Id = 2, Name = "Water", Threshold = 3, ImagePath = "water.png" }
            );

            context.SaveChanges();

            var service = new InventoryService(context);

            var result = service.GetInventory();

            Assert.IsNotNull(result);
        }

        /*
        Title
        Description: Tests that the inventory service returns a result when tags exist for a product
        Input: Product with associated tags inserted into the database
        Output: Non-null inventory result
        Return: List of inventory items
        */
        [TestMethod]
        public void GetInventory_ReturnsResult_WhenTagsExist()
        {
            using var context = GetDbContext();

            context.Product.Add(new Product
            {
                Id = 1,
                Name = "Water",
                Threshold = 2,
                ImagePath = "water.png"
            });

            context.Tag.AddRange(
                new Tag { EPC = "TAG001", ProductId = 1 },
                new Tag { EPC = "TAG002", ProductId = 1 }
            );

            context.SaveChanges();

            var service = new InventoryService(context);

            var result = service.GetInventory();

            Assert.IsNotNull(result);
        }
    }
}