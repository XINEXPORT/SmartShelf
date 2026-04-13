using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartShelf.web.Data;

namespace SmartShelf.web.Controllers
{
    /*
    InventoryController
    Description:
    Provides aggregated inventory data at the product level.
    Converts tag-level presence data into stock counts
    and restocking signals for each product.
    */
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly SmartShelfContext _context;

        /*
        Constructor
        Description:
        Injects the database context to allow querying RFID and product data.
        */
        public InventoryController(SmartShelfContext context)
        {
            _context = context;
        }

        /*
        GetInventory
        Description:
        Retrieves current inventory levels by:
        1. Filtering only tags that are currently present
        2. Grouping tags by their associated product
        3. Counting how many tags exist per product
        4. Comparing count against the product's min stock threshold
        5. Calculating restock amount if below min stock threshold

        Returns:
        JSON list of products with:
        - ProductId
        - ProductName
        - Count (current inventory)
        - Threshold (minimum desired stock)
        - IsLowStock (true if below threshold)
        - RestockAmount (how many items needed to reach threshold)
        */
        [HttpGet]
        public IActionResult GetInventory()
        {
            var inventory = _context.TagCurrentState

                
                //Load Tag & Product data needed
                //for grouping
                .Include(tcs => tcs.Tag)
                    .ThenInclude(tag => tag.Product)

                //filter for isPresent 
                .Where(tcs => tcs.IsPresent)

                
                //Group by product-level data.
                //Each group represents all tags for a single product.
                .GroupBy(tcs => new
                {
                    ProductId = tcs.Tag.Product.Id,
                    ProductName = tcs.Tag.Product.Name,
                    Threshold = tcs.Tag.Product.Threshold
                })

                /*
                Project each group into an inventory summary object.
                Product identity
                */
                .Select(g => new
                {

                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,

                    Count = g.Count(),

                    Threshold = g.Key.Threshold,

                    /*
                    IsLowStock:
                    True if current inventory is below the product threshold
                    */
                    IsLowStock = g.Count() < g.Key.Threshold,

                    /*
                    RestockAmount:
                    If below threshold → how many items are needed
                    If above threshold → 0

                    Formula:
                    threshold - count (only when count < threshold)
                    */
                    RestockAmount = g.Count() < g.Key.Threshold
                        ? g.Key.Threshold - g.Count()
                        : 0
                })

                //return a list
                .ToList();

            //Return inventory data as JSON
            return Ok(inventory);
        }
    }
}