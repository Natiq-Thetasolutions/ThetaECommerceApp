using System;
using System.Collections.Generic;

namespace ThetaECommerceApp.Models
{
    public partial class OrderLine
    {
        public int Id { get; set; }
        public string? OrderId { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int? SellerId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string? CouriorName { get; set; }
        public string? TrackingNumber { get; set; }
        public int? Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? MetaData { get; set; }
    }
}
