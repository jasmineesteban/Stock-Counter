using MauiApp1.Models;
using MauiApp1.Services;

namespace MauiApp1.ViewModels
{
    public class ItemCountViewModel
    {
        private readonly ItemCountService _itemCountService;

        public ItemCountViewModel(ItemCountService itemCountService)
        {
            _itemCountService = itemCountService;
        }

        public async Task AddItemCount(string itemno, string description, string uom, string batchlot, string expiry, int quantity, string countcode)
        {
            var itemCount = new ItemCount
            {
                ItemCode = itemno,
                ItemDescription = description,
                ItemUom = uom,
                ItemBatchLotNumber = batchlot,
                ItemExpiry = expiry,
                ItemQuantity = quantity,
                ItemCountCode = countcode,
            };

            await _itemCountService.AddItemCountAsync(itemCount);
        }
    }
}