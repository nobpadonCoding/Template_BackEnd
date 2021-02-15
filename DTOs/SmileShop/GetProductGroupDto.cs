using System;
using System.Collections.Generic;
using NetCoreAPI_Template_v3_with_auth.Models;

namespace NetCoreAPI_Template_v3_with_auth.DTOs.SmileShop
{
    public class GetProductGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public Boolean Status { get; set; }
        public List<GetProductDto> Products { get; set; }
    }
}