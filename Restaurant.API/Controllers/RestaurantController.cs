﻿using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.DTO;
using Restaurant.Application.Services;
using Restaurant.Infrastructure;

namespace Restaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IMapper _mapper;
        private readonly IValidator<RestaurantQueryDto> _validator;

        public RestaurantController(IRestaurantService restaurantService, IMapper mapper,IValidator<RestaurantQueryDto> validator)
        {

            _restaurantService = restaurantService;
            _mapper = mapper;
           _validator = validator;
        }
        [HttpGet]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll([FromQuery] RestaurantQueryDto query)
        {
            var validation =  _validator.ValidateAsync(query).Result;
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }
            var restaurants = _restaurantService.GetAll(query);

            return Ok(restaurants);

        }

        [HttpGet("{id}")]
        [Authorize(Policy ="Atleast20")]
        public ActionResult<RestaurantDto> Get([FromRoute] int id)
        {
            var restaurant = _restaurantService.GetById(id);
            if (restaurant is null)
                return NotFound();

            return Ok(restaurant);

        }

        [HttpPost]
        [Authorize(Roles ="Manager,Admin")]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var id = _restaurantService.Create(dto);
            return Created($"api/Restaurant/{id}", null);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager,Admin")]
        public ActionResult Delete([FromRoute] int id)
        {
            _restaurantService.Delete(id);
            return NoContent();
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Manager,Admin")]
        public ActionResult Update([FromBody] UpdateRestaurantDto dto, [FromRoute] int id )
        {
            _restaurantService.Update(dto,id);

            return Ok();
        }
    }
}
