using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanelManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Dropdown _itemDropdown;
    [SerializeField] private TMP_InputField _quantityInput;
    [SerializeField] private Button _addButton;
    [SerializeField] private Button _removeButton;

    [Header("References")]
    [SerializeField] private Inventory _inventory;

    [Header("Database")]
    [SerializeField] private string _resourcesFolder = "InventoryItems";

    private List<InventoryItemData> _itemDatabase;

    private void Awake()
    {
        LoadItemDatabase();
    }

    private void Start()
    {
        SetupDropdown();
    }

    private void OnEnable()
    {
        _addButton.onClick.AddListener(OnAddButtonClicked);
        _removeButton.onClick.AddListener(OnRemoveButtonClicked);
    }

    private void OnDisable()
    {
        _addButton.onClick.RemoveListener(OnAddButtonClicked);
        _removeButton.onClick.RemoveListener(OnRemoveButtonClicked);
    }

    private void LoadItemDatabase()
    {
        InventoryItemData[] loadedItems = Resources.LoadAll<InventoryItemData>(_resourcesFolder);
        _itemDatabase = new List<InventoryItemData>(loadedItems);

        HashSet<string> seenIds = new HashSet<string>();
        foreach (InventoryItemData item in _itemDatabase)
        {
            if (!seenIds.Add(item.ItemID))
            {
                Debug.LogError($"Duplicate ItemID {item.ItemID} found on {item.name}");
            }
        }
    }

    private void SetupDropdown()
    {
        _itemDropdown.ClearOptions();

        List<string> options = new List<string>();

        foreach (InventoryItemData item in _itemDatabase)
        {
            options.Add(item.ItemName);
        }

        _itemDropdown.AddOptions(options);
        _itemDropdown.value = 0;
        _itemDropdown.RefreshShownValue();
    }

    private void OnAddButtonClicked()
    {
        if (!TryGetInputData(out InventoryItemData itemData, out int quantity))
        {
            return;
        }

        int remaining = _inventory.AddItem(itemData, quantity);

        if (remaining > 0)
        {
            Debug.Log($"Inventory full! Failed to add {remaining} units of item {itemData.ItemName}");
        }
        else
        {
            Debug.Log($"Successfully added {quantity} units of item {itemData.ItemName}");
        }
    }

    private void OnRemoveButtonClicked()
    {
        if (!TryGetInputData(out InventoryItemData itemData, out int quantity))
        {
            return;
        }

        bool success = _inventory.RemoveItem(itemData, quantity);

        if (!success)
        {
            Debug.Log($"Not enough items {itemData.ItemName} in inventory to remove ({quantity})");
        }
        else
        {
            Debug.Log($"Successfully removed {quantity} units of item {itemData.ItemName}");
        }
    }

    private bool TryGetInputData(out InventoryItemData itemData, out int quantity)
    {
        itemData = null;
        quantity = 0;

        if (!int.TryParse(_quantityInput.text, out quantity) || quantity <= 0)
        {
            Debug.Log("Invalid quantity! Please enter a positive integer");
            return false;
        }

        int selectedIndex = _itemDropdown.value;

        if (selectedIndex < 0 || selectedIndex >= _itemDatabase.Count)
        {
            Debug.Log("Invalid item selection");
            return false;
        }

        itemData = _itemDatabase[selectedIndex];
        return true;
    }
}