using Cw7;
using Cw7.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Exercise5.Controllers
{
    [Route("api/warehouses")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IDb _db;


        public WarehouseController(IConfiguration configuration, IDb db)
        {
            _configuration = configuration;
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductWarehouse(ProductWarehouse newProductWarehouse)
        {
            if (newProductWarehouse.Amount <= 0)
            {
                return NotFound("ilosc ma byc wieksza niż 0!!!");
            }
            int idOrder = await _db.NoOrder(newProductWarehouse);
            if (await _db.NoOrder(newProductWarehouse) == -1)
            {
                return NotFound("nie ma zamowienia...");
            }
            if (await _db.NoWarehouse(newProductWarehouse.IdWarehouse) || await _db.NoProduct(newProductWarehouse.IdProduct))
            {
                return NotFound("nie ma produktu/magazynu...");
            }

            Product product = new()
            {
                IdProduct = newProductWarehouse.IdProduct
            };
            Order order = new()
            {
                IdOrder = idOrder,
                IdProduct = newProductWarehouse.IdProduct,
                Amount = newProductWarehouse.Amount,
                CreatedAt = newProductWarehouse.CreatedAt
            };

            await _db.UpdateFulfilledAt(order);

            var pk = await _db.InsertProductWarehouse(newProductWarehouse, order, product);
            
            return Ok(pk);
        }
    }
    

}
