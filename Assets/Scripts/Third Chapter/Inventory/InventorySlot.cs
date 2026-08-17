using System;
using UnityEngine;

public class InventorySlot
{
    [SerializeField] private InventoryItemData _itemData;
    [SerializeField] private int _quantity;

    public InventoryItemData ItemData => this._itemData;
    public int Quantity => this._quantity;
    public bool IsEmpty => this._itemData == null;

    public InventorySlot()
    {
        Clear();
    }

    public InventorySlot(InventoryItemData itemData, int quantity)
    {
        this._itemData = itemData;
        this._quantity = quantity;
    }

    public void SetItem(InventoryItemData itemData, int quantity)
    {
        this._itemData = itemData;
        this._quantity = quantity;
    }

    public int AddQuantity(int amount)
    {
        if (this._itemData == null)
        {
            return amount;
        }

        int maxStack = this._itemData.ItemStack;
        int spaceLeft = maxStack - this._quantity;
        int amountToAdd = Math.Min(spaceLeft, amount);

        this._quantity += amountToAdd;

        return amount - amountToAdd;
    }

    public void RemoveQuantity(int amount)
    {
        this._quantity -= amount;

        if (this._quantity <= 0)
        {
            Clear();
        }
    }

    public bool CanAccept(InventoryItemData itemData)
    {
        if (this.IsEmpty)
        {
            return true;
        }

        return this._itemData.ItemID == itemData.ItemID
            && this._quantity < this._itemData.ItemStack;
    }

    public void Clear()
    {
        this._itemData = null;
        this._quantity = 0;
    }
}