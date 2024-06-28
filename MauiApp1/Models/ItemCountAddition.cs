namespace MauiApp1.Models
{
    public class ItemCountAddition
    {
        public string ItemCountCode { get; set; } = string.Empty;
        public string ItemCode { get; set; } = string.Empty;
        public string ItemDescription { get; set; } = string.Empty;
        public string ItemUom { get; set; } = string.Empty;
        public string ItemBatchLotNumber { get; set; } = string.Empty;
        public string ItemExpiry { get; set; } = string.Empty;
        public int ItemQuantity { get; set; }
    }
}
