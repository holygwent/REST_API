using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.DTO;
using Restaurant.Application.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Restaurant.API.Controllers
{
    [Route("api/Restaurant/{restaurantId}/[controller]")]
    [ApiController]
    [Authorize]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }
        [HttpGet]
        public ActionResult<List<DishDto>> GetAll([FromRoute]int restaurantId)
        {
            var dishes = _dishService.GetAll(restaurantId);
            return Ok(dishes);
        }
        [HttpGet("{dishId}")]
        public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var dish = _dishService.Get(restaurantId,dishId);
            return Ok(dish);
        }
        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public ActionResult Post([FromRoute]int restaurantId, [FromBody]CreateDishDto dto)
        {
            var newDishId = _dishService.Create(restaurantId, dto);
            return Created($"api/Restaurant/{restaurantId}/Dish/{newDishId}",null);
        }
        [HttpDelete]
        [Authorize(Roles = "Manager,Admin")]
        public ActionResult DeleteAllDish([FromRoute] int restaurantId)
        {
            _dishService.DeleteAll(restaurantId);
            return NoContent();
        }
        [HttpDelete("{dishId}")]
        [Authorize(Roles = "Manager,Admin")]
        public ActionResult DeleteAllDish([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            _dishService.Delete(restaurantId,dishId);
            return NoContent();
        }
    }
}
