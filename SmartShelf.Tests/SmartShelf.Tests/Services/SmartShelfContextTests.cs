using Microsoft.EntityFrameworkCore;
using SmartShelf.web.Data;
using SmartShelf.web.Models;

namespace SmartShelf.Tests.Services
{
    [TestClass]
    public class SmartShelfContextTests
    {
        private SmartShelfContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<SmartShelfContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new SmartShelfContext(options);
        }

        /*
        Title: CanInsertProduct_AndRetrieveIt
        Description: Tests inserting a product into the database and retrieving it
        Input: Product object
        Output: Retrieved product is not null
        Return: Product object
        */
        [TestMethod]
        public void CanInsertProduct_AndRetrieveIt()
        {
            using var context = GetDbContext();

            context.Product.Add(new Product
            {
                Id = 1,
                Name = "Test Product",
                Threshold = 5,
                ImagePath = "test.png"
            });

            context.SaveChanges();

            var product = context.Product.FirstOrDefault();

            Assert.IsNotNull(product);
        }

        /*
        Title: CanInsertTag_AndRetrieveIt
        Description: Tests inserting a tag and retrieving it from the database
        Input: Tag object
        Output: Retrieved tag is not null
        Return: Tag object
        */
        [TestMethod]
        public void CanInsertTag_AndRetrieveIt()
        {
            using var context = GetDbContext();

            context.Tag.Add(new Tag
            {
                EPC = "TAG001",
                ProductId = 1
            });

            context.SaveChanges();

            var tag = context.Tag.FirstOrDefault();

            Assert.IsNotNull(tag);
        }

        /*
        Title: CanInsertReader_AndRetrieveIt
        Description: Tests inserting a reader and retrieving it
        Input: Tag object
        Output: Retrieved reader is not null
        Return: Reader object
        */
        [TestMethod]
        public void CanInsertReader_AndRetrieveIt()
        {
            using var context = GetDbContext();

            context.Reader.Add(new Reader
            {
                Id = 1,
                Location = "Shelf A"
            });

            context.SaveChanges();

            var reader = context.Reader.FirstOrDefault();

            Assert.IsNotNull(reader);
        }

        /*
        Title: CanInsertTagReadEvent_AndRetrieveIt
        Description: Tests inserting a tag read event and retrieving it
        Input: TagReadEvent object
        Output: Retrieved tag read event is not null
        Return: TagReadEvent object
        */
        [TestMethod]
        public void CanInsertTagReadEvent_AndRetrieveIt()
        {
            using var context = GetDbContext();

            context.TagReadEvent.Add(new TagReadEvent
            {
                Id = 1,
                EPC = "TAG001",
                ReaderId = 1,
                Timestamp = DateTime.UtcNow,
                Antenna = 1,
                Rssi = -45
            });

            context.SaveChanges();

            var tagReadEvent = context.TagReadEvent.FirstOrDefault();

            Assert.IsNotNull(tagReadEvent);
        }

        /*
        Title: CanInsertTagCurrentState_AndRetrieveIt
        Description: Tests inserting current state of a tag and retrieving it
        Input: TagCurrentState object
        Output: Retrieved state is not null
        Return: TagCurrentState object
        */
        [TestMethod]
        public void CanInsertTagCurrentState_AndRetrieveIt()
        {
            using var context = GetDbContext();

            context.TagCurrentState.Add(new TagCurrentState
            {
                EPC = "TAG001",
                ReaderId = 1,
                Rssi = -50,
                LastSeenTimestamp = DateTime.UtcNow,
                IsPresent = true
            });

            context.SaveChanges();

            var state = context.TagCurrentState.FirstOrDefault();

            Assert.IsNotNull(state);
        }

        /*
        Title: CanInsertMultipleProducts_AndCountThem
        Description: Tests inserting multiple products and verifying count
        Input: Multiple Product objects
        Output: Count equals expected number of inserted products
        Return: Integer count
        */
        [TestMethod]
        public void CanInsertMultipleProducts_AndCountThem()
        {
            using var context = GetDbContext();

            context.Product.AddRange(
                new Product { Id = 1, Name = "Chips", Threshold = 5, ImagePath = "chips.png" },
                new Product { Id = 2, Name = "Water", Threshold = 3, ImagePath = "water.png" },
                new Product { Id = 3, Name = "Candy", Threshold = 10, ImagePath = "candy.png" }
            );

            context.SaveChanges();

            Assert.AreEqual(3, context.Product.Count());
        }

        /*
        Title: CanInsertMultipleTags_ForSameProduct
        Description: Tests inserting multiple tags associated with a single product
        Input: Multiple Tag objects for one product
        Output: Tag count matches expected value
        Return: Integer count
        */
        [TestMethod]
        public void CanInsertMultipleTags_ForSameProduct()
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

            var tagCount = context.Tag.Count(t => t.ProductId == 1);

            Assert.AreEqual(3, tagCount);
        }

        /*
        Title: CanUpdateTagCurrentStatePresence
        Description: Tests updating the presence status and missed scan count
        Input: TagCurrentState object
        Output: Updated values match expected values
        Return: Updated TagCurrentState object
        */
        [TestMethod]
        public void CanUpdateTagCurrentStatePresence()
        {
            using var context = GetDbContext();

            context.TagCurrentState.Add(new TagCurrentState
            {
                EPC = "TAG001",
                ReaderId = 1,
                Antenna = 1,
                Rssi = -50,
                ReadCount = 3,
                Frequency = 915,
                IsPresent = true,
                MissedScanCount = 0,
                LastSeenTimestamp = DateTime.UtcNow
            });

            context.SaveChanges();

            var state = context.TagCurrentState.First();
            state.IsPresent = false;
            state.MissedScanCount = 1;

            context.SaveChanges();

            var updatedState = context.TagCurrentState.First();

            Assert.IsFalse(updatedState.IsPresent);
            Assert.AreEqual(1, updatedState.MissedScanCount);
        }
    }
}