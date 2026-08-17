using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Range(1, 9)]
    [SerializeField] private int _slotCount = 9;

    private List<InventorySlot> _slots;

    public IReadOnlyList<InventorySlot> Slots => _slots;
    public event Action OnInventoryChanged;

    private void Awake()
    {
        _slots = new List<InventorySlot>(_slotCount);

        for (int i = 0; i < _slotCount; i++)
        {
            _slots.Add(new InventorySlot());
        }
    }

    public int AddItem(InventoryItemData itemData, int quantity)
    {
        if (itemData == null || quantity <= 0)
        {
            return quantity;
        }

        int remaining = quantity;

        for (int i = 0; i < _slots.Count; i++)
        {
            if (remaining <= 0)
            {
                break;
            }

            InventorySlot slot = _slots[i];
            if (!slot.IsEmpty && slot.CanAccept(itemData))
            {
                remaining = slot.AddQuantity(remaining);
            }
        }

        for (int i = 0; i < _slots.Count; i++)
        {
            if (remaining <= 0)
            {
                break;
            }

            InventorySlot slot = _slots[i];
            if (slot.IsEmpty)
            {
                int placed = Mathf.Min(remaining, itemData.ItemStack);
                _slots[i].SetItem(itemData, placed);
                remaining -= placed;
            }
        }

        OnInventoryChanged?.Invoke();
        return remaining;
    }

    public bool RemoveItem(InventoryItemData itemData, int quantity)
    {
        int totalAvailable = 0;

        for (int i = 0; i < _slots.Count; i++)
        {
            InventorySlot slot = _slots[i];
            if (!slot.IsEmpty && slot.ItemData.ItemID == itemData.ItemID)
            {
                totalAvailable += slot.Quantity;
            }
        }

        if (totalAvailable < quantity)
        {
            return false;
        }

        int remaining = quantity;

        for (int i = 0; i < _slots.Count; i++)
        {
            if (remaining <= 0)
            {
                break;
            }

            InventorySlot slot = _slots[i];
            if (!slot.IsEmpty && slot.ItemData.ItemID == itemData.ItemID)
            {
                int toRemove = Mathf.Min(slot.Quantity, remaining);
                slot.RemoveQuantity(toRemove);
                remaining -= toRemove;
            }
        }

        OnInventoryChanged?.Invoke();
        return true;
    }
}