using FluentValidation;
using Restaurant.Application.DTO;
using Restaurant.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Validation
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        private readonly RestaurantDbContext _restaurantDbContext;
        public RegisterUserDtoValidator(RestaurantDbContext restaurantDbContext)
        {
            _restaurantDbContext = restaurantDbContext;
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);
            RuleFor(x => x.Email).Custom((value, context) =>
            {
                var emailInUse = _restaurantDbContext.Users.Any(x => x.Email == value);
                if (emailInUse)
                    context.AddFailure("Email", "Email is taken!!!!");
            });
            RuleFor(x => x.Nationality).NotEmpty();
        }
    }
}
