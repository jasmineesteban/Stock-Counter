﻿using MauiApp1.Models;
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

        public async Task<bool> AddItemCount(string itemCountCode, string itemCode, string itemDescription, string itemUom, string itemBatchLotNumber, string itemExpiry, int itemQuantity)
        {
            var itemCount = new ItemCountAddition
            {
                ItemCountCode = itemCountCode,
                ItemCode = itemCode,
                ItemDescription = itemDescription,
                ItemUom = itemUom,
                ItemBatchLotNumber = itemBatchLotNumber,
                ItemExpiry = itemExpiry,
                ItemQuantity = itemQuantity
            };

            return await _itemCountService.AddItemCountAsync(itemCount);
        }

      
        public async Task<IEnumerable<ItemCount>> ShowItemCount(string countCode, int sortValue)
        {
            return await _itemCountService.ShowItemCountAsync(countCode, sortValue);

        }

        public async Task<bool> EditItemCount(string itemKey, string itemBatchLotNumber, string itemExpiry, int itemQuantity)
        {
            var itemCount = new ItemCount
            {
                ItemKey = itemKey,
                ItemBatchLotNumber = itemBatchLotNumber,
                ItemExpiry = itemExpiry,
                ItemQuantity = itemQuantity
            };

            return await _itemCountService.EditItemCountAsync(itemCount);
        }

        public async Task<bool> DeleteItemCount(string itemKey)
        {
            return await _itemCountService.DeleteItemCountAsync(itemKey);
        }
    }
}
