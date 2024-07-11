namespace MauiApp1.Models
{
    public class ItemCount
    {
        public string? ItemKey { get; set; }
        public string ItemCountCode { get; set; } 
        public int? ItemCounter { get; set; }
        public string? ItemCode { get; set; } 
        public string? ItemDescription { get; set; } 
        public string? ItemUom { get; set; } 
        public string? ItemBatchLotNumber { get; set; } 
        public string? ItemExpiry { get; set; } 
        public int? ItemQuantity { get; set; }

        public bool IsSelected { get; set; }
        public Grid ItemGrid { get; set; }
    }
}
