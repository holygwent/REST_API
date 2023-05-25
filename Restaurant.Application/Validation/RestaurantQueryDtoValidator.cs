using FluentValidation;
using Restaurant.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.Validation
{
    public class RestaurantQueryDtoValidator:AbstractValidator<RestaurantQueryDto>
    {
        private readonly int[] _allowedPageSizes = new[] { 5, 10, 15 };
        private readonly string[] _allowedSortBy = 
            { nameof(Restaurant.Domain.ORM.Restaurant.Name), nameof(Restaurant.Domain.ORM.Restaurant.Description), nameof(Restaurant.Domain.ORM.Restaurant.Category)};
        public RestaurantQueryDtoValidator()
        {
            RuleFor(x=>x.pageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.pageSize).Custom((value, context) =>
            {
                if(!_allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"Page size must be in [{string.Join(",", _allowedPageSizes)}]");
                }
            });
            RuleFor(x => x.sortBy).Must(value => string.IsNullOrEmpty(value) || _allowedSortBy.Contains(value)).WithMessage($"sort by is optional or must by in [{string.Join(",",_allowedSortBy)}]");
        }
    }
}
