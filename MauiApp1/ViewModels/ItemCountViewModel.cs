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

        public async Task AddItemCount(string countcode, string itemno, string description, string uom, string batchlot, string expiry, int quantity)
        {
            var itemCount = new ItemCount
            {
                ItemCountCode = countcode,
                ItemCode = itemno,
                ItemDescription = description,
                ItemUom = uom,
                ItemBatchLotNumber = batchlot,
                ItemExpiry = expiry,
                ItemQuantity = quantity,
          
            };

            await _itemCountService.AddItemCountAsync(itemCount);
        }
    }
}