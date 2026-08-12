using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class WeaponStats
{
    public float damage;

    [SerializeField]
    [JsonProperty]
    private bool _isBroken;

    public WeaponStats(float dmg, bool broken)
    {
        damage = dmg;
        _isBroken = broken;
    }
}

[System.Serializable]
public class ItemData
{
    public string itemName;

    [SerializeField]
    [JsonProperty]
    private int _amount;

    [JsonIgnore]
    public int amount
    {
        get => _amount;
        set => _amount = value;
    }

    public ItemData(string name, int amt)
    {
        itemName = name;
        _amount = amt;
    }
}

[System.Serializable]
public class PlayerSaveWrapper
{
    public string playerName;

    public WeaponStats equippedWeapon;

    public List<ItemData> inventoryList;

    public Dictionary<string, string> secretTags = new Dictionary<string, string>();
}

public class JsonComparisonDemo : MonoBehaviour
{
    [SerializeField] private bool _useIndentation = true;

    [ContextMenu("Raw List Test")]
    private void RawListTest()
    {
        List<ItemData> rawList = new List<ItemData>
        {
            new ItemData("Sword", 1),
            new ItemData("Potion", 5)
        };

        string unityJson = JsonUtility.ToJson(rawList, _useIndentation);
        Debug.Log("JsonUtility RAW LIST:\n" + unityJson);

        string newtonJson = JsonConvert.SerializeObject(rawList, _useIndentation ? Formatting.Indented : Formatting.None);
        Debug.Log("Newtonsoft RAW LIST:\n" + newtonJson);
    }

    [ContextMenu("Complex Wrapper Test")]
    private void ComplexWrapperTest()
    {
        PlayerSaveWrapper wrapper = new PlayerSaveWrapper
        {
            playerName = "Hero",
            equippedWeapon = new WeaponStats(25.5f, true),
            inventoryList = new List<ItemData>
            {
                new ItemData("Axe", 1),
                new ItemData("Apple", 10)
            },
            secretTags = new Dictionary<string, string>
            {
                { "Quest1", "Completed" },
                { "Quest2", "Active" }
            }
        };

        string unityJson = JsonUtility.ToJson(wrapper, _useIndentation);
        File.WriteAllText(GetPath("UnityComplex.json"), unityJson);
        Debug.Log("JsonUtility COMPLEX:\n" + unityJson);

        string newtonJson = JsonConvert.SerializeObject(wrapper, _useIndentation ? Formatting.Indented : Formatting.None);
        File.WriteAllText(GetPath("NewtonComplex.json"), newtonJson);
        Debug.Log("Newtonsoft COMPLEX:\n" + newtonJson);
    }

    private string GetPath(string fileName)
    {
        return Path.Combine(Application.streamingAssetsPath, fileName);
    }
}