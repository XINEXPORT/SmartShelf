using Microsoft.EntityFrameworkCore;
using SmartShelf.web.Data;
using SmartShelf.web.Models;
using SmartShelf.web.Services;

namespace SmartShelf.Tests.Services
{
    [TestClass]
    public class SummaryServiceTests
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
        Description: Tests that total product count is correctly calculated
        Input: Multiple products inserted into the database
        Output: TotalProducts equals expected value
        Return: Summary result object
        */
        [TestMethod]
        public void GetSummary_ReturnsCorrectProductCounts()
        {
            using var context = GetDbContext();

            context.Product.AddRange(
                new Product { Id = 1, Name = "Chips", Threshold = 5, ImagePath = "chips.png" },
                new Product { Id = 2, Name = "Soda", Threshold = 3, ImagePath = "soda.png" }
            );

            context.SaveChanges();

            var service = new SummaryService(context);

            var result = service.GetSummary();

            Assert.AreEqual(2, result.TotalProducts);
        }

        /*
        Title
        Description: Tests that summary service executes when low stock conditions exist
        Input: Product with fewer tags than threshold
        Output: Non-null summary result
        Return: Summary result object
        */
        [TestMethod]
        public void GetSummary_DetectsLowStockProducts()
        {
            using var context = GetDbContext();

            context.Product.Add(new Product
            {
                Id = 1,
                Name = "Chips",
                Threshold = 5,
                ImagePath = "chips.png"
            });

            context.Tag.AddRange(
                new Tag { EPC = "TAG001", ProductId = 1 },
                new Tag { EPC = "TAG002", ProductId = 1 }
            );

            context.SaveChanges();

            var service = new SummaryService(context);

            var result = service.GetSummary();

            Assert.IsNotNull(result);
        }

        /*
        Title
        Description: Tests that summary service returns a result when called
        Input: Empty database context
        Output: Non-null summary result
        Return: Summary result object
        */
        [TestMethod]
        public void GetSummary_ReturnsResult_WhenCalled()
        {
            using var context = GetDbContext();

            var service = new SummaryService(context);

            var result = service.GetSummary();

            Assert.IsNotNull(result);
        }

        /*
        Title
        Description: Tests that last updated timestamp is set
        Input: Empty database context
        Output: LastUpdated is not default value
        Return: Summary result object
        */
        [TestMethod]
        public void GetSummary_ReturnsLastUpdatedValue()
        {
            using var context = GetDbContext();

            var service = new SummaryService(context);

            var result = service.GetSummary();

            Assert.IsTrue(result.LastUpdated != default);
        }

        /*
        Title
        Description: Tests that all counts are zero when database is empty
        Input: Empty database context
        Output: All summary counts are zero
        Return: Summary result object
        */
        [TestMethod]
        public void GetSummary_ReturnsZeroCounts_WhenDatabaseIsEmpty()
        {
            using var context = GetDbContext();

            var service = new SummaryService(context);

            var result = service.GetSummary();

            Assert.AreEqual(0, result.TotalProducts);
            Assert.AreEqual(0, result.LowStockProducts);
            Assert.AreEqual(0, result.OutOfStockProducts);
        }

        /*
        Title
        Description: Tests total product count when multiple products exist
        Input: Multiple products inserted
        Output: TotalProducts equals expected count
        Return: Summary result object
        */
        [TestMethod]
        public void GetSummary_ReturnsTotalProducts_WhenMultipleProductsExist()
        {
            using var context = GetDbContext();

            context.Product.AddRange(
                new Product { Id = 1, Name = "Chips", Threshold = 5, ImagePath = "chips.png" },
                new Product { Id = 2, Name = "Water", Threshold = 3, ImagePath = "water.png" },
                new Product { Id = 3, Name = "Candy", Threshold = 10, ImagePath = "candy.png" }
            );

            context.SaveChanges();

            var service = new SummaryService(context);

            var result = service.GetSummary();

            Assert.AreEqual(3, result.TotalProducts);
        }

        /*
        Title
        Description: Tests summary behavior when product threshold is zero
        Input: Product with zero threshold
        Output: Non-null summary result
        Return: Summary result object
        */
        [TestMethod]
        public void GetSummary_ReturnsResult_WhenProductHasZeroThreshold()
        {
            using var context = GetDbContext();

            context.Product.Add(new Product
            {
                Id = 1,
                Name = "Test Product",
                Threshold = 0,
                ImagePath = "test.png"
            });

            context.SaveChanges();

            var service = new SummaryService(context);

            var result = service.GetSummary();

            Assert.IsNotNull(result);
        }

        /*
        Title
        Description: Tests summary service when tags exist for a product
        Input: Product with multiple tags
        Output: Non-null summary result
        Return: Summary result object
        */
        [TestMethod]
        public void GetSummary_ReturnsResult_WhenTagsExist()
        {
            using var context = GetDbContext();

            context.Product.Add(new Product
            {
                Id = 1,
                Name = "Chips",
                Threshold = 5,
                ImagePath = "chips.png"
            });

            context.Tag.AddRange(
                new Tag { EPC = "TAG001", ProductId = 1 },
                new Tag { EPC = "TAG002", ProductId = 1 },
                new Tag { EPC = "TAG003", ProductId = 1 }
            );

            context.SaveChanges();

            var service = new SummaryService(context);

            var result = service.GetSummary();

            Assert.IsNotNull(result);
        }
    }
}