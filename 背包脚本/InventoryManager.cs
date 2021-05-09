using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager instance;

    public ItemConf itemConf;
    public List<string> itemNames;
    public GameObject slotGrid;
    public Slot slotPrefeb;
    public Text itemDesription;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }

    private void Start()
    {
        GlobalData.SaveName = "0";
        if (!GlobalData.HasObject("itemNamesList"))
        {
            itemNames = new List<string>();
            GlobalData.SetObject("itemNamesList", itemNames);
            AddNewItem("cap");
            AddNewItem("weapon");
        }
        else
            itemNames = GlobalData.GetData<List<string>>("itemNamesList");

    }

    public void OnEnable()
    {
        //test   
        Debug.Log("enable");
        instance.itemDesription.text = "";
        RefreshItem();
    }

    public static void UpdateItemInfo(string itemDescription)
    {
        instance.itemDesription.text = itemDescription;
    }

    //向背包列表添加item
    public static void AddNewItem(string itemName)
    {
        instance.itemNames.Add(itemName);

    }

    //向背包栏界面添加item
    public static void CreateNewItem(BagItem item)
    {
        Slot newItem = Instantiate(instance.slotPrefeb, instance.slotGrid.transform.position, Quaternion.identity);
        newItem.gameObject.transform.SetParent(instance.slotGrid.transform);
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.Icon;

    }

    //刷新背包栏物品
    public static void RefreshItem()
    {
        for (int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            if (instance.slotGrid.transform.childCount == 0)
                break;
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
        }

        foreach (string iName in instance.itemNames)
        {
            BagItem bagItem = instance.itemConf.GetItemByName(iName);
            CreateNewItem(bagItem);
        }
    }


}
