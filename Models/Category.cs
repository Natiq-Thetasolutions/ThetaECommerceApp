using System;
using System.Collections.Generic;

namespace ThetaECommerceApp.Models
{
    public partial class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Images { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? MetaData { get; set; }
        public string? SeoData { get; set; }
    }
}
