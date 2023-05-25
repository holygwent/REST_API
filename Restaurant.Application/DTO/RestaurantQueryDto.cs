using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Application.DTO
{
    public class RestaurantQueryDto
    {
        public string searchingPhrase { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string? sortBy { get; set; }
        public SortDirection? sortDirection { get; set; }
    }
}
