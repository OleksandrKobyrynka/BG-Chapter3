using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemData", menuName = "Scriptable Objects/InventoryItemData")]
public class InventoryItemData : ScriptableObject
{
    [SerializeField] private string _itemID;
    [SerializeField] private string _itemName;
    [SerializeField] private Sprite _itemIcon;
    [SerializeField] private int _itemStack = 64;

    public string ItemID => this._itemID;
    public string ItemName => this._itemName;
    public Sprite ItemIcon => this._itemIcon;
    public int ItemStack => this._itemStack;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(_itemID))
        {
            _itemID = System.Guid.NewGuid().ToString();

            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            #endif
        }
    }
}
