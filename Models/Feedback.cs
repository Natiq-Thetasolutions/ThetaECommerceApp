using System;
using System.Collections.Generic;

namespace ThetaECommerceApp.Models
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? OrderId { get; set; }
        public string? Feedback1 { get; set; }
        public string? Images { get; set; }
        public int? Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
