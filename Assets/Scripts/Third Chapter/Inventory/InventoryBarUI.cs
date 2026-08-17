using System.Collections.Generic;
using UnityEngine;

public class InventoryBar : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventorySlotPanelUI _slotPrefab;
    [SerializeField] private Transform _slotsParent;

    private readonly List<InventorySlotPanelUI> _slotPanels = new();

    private void Start()
    {
        BuildSlots();
        RefreshUI();
    }

    private void OnEnable()
    {
        _inventory.OnInventoryChanged += RefreshUI;
    }

    private void OnDisable()
    {
        _inventory.OnInventoryChanged -= RefreshUI;
    }

    private void BuildSlots()
    {
        foreach (InventorySlot slot in _inventory.Slots)
        {
            InventorySlotPanelUI panel = Instantiate(_slotPrefab, _slotsParent);
            _slotPanels.Add(panel);
        }
    }

    private void RefreshUI()
    {
        IReadOnlyList<InventorySlot> slots = _inventory.Slots;

        for (int i = 0; i < slots.Count; i++)
        {
            _slotPanels[i].SetData(slots[i]);
        }
    }
}