using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotPanelUI : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _amount;
    [SerializeField] private TextMeshProUGUI _name;

    public void SetData(InventorySlot slot)
    {
        if (slot == null || slot.IsEmpty)
        {
            Clear();
            return;
        }

        _itemImage.sprite = slot.ItemData.ItemIcon;
        _itemImage.enabled = true;

        _name.text = slot.ItemData.ItemName;

        _amount.text = slot.Quantity.ToString();
    }

    public void Clear()
    {
        _itemImage.sprite = null;
        _itemImage.enabled = false;

        _name.text = string.Empty;
        _amount.text = string.Empty;
    }
}