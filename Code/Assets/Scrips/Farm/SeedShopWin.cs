using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soulgame.Util;

public class SeedShopWin : MonoBehaviour {
    public GameObject ToFruit;
    public GameObject ToVege;

    public GameObject FruitKind;
    public GameObject VegeKind;

    public UITable FruitTable;
    public UITable VegeTable;

    private string ShopItemPrefabPath = "Farm/Prefabs/ShopItemView";

    private List<ShopItemView> ItemViews = new List<ShopItemView>();

    void Awake()
    {
        
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        FruitKind.SetActive(true);
        VegeKind.SetActive(false);

        InitItemViews();

        UIEventListener.Get(ToFruit).onClick = OnTabClick;
        UIEventListener.Get(ToVege).onClick = OnTabClick;
    }

    void InitItemViews()
    {
        GameObject prefab = ResourceManager.Load(ShopItemPrefabPath) as GameObject;

        for (int i = 0, max = SeedShop.Instance.FarmSeedTables.Count; i < max; i++)
        {
            FarmSeedTable table = SeedShop.Instance.FarmSeedTables[i];
            
            GameObject go = null;
            if (table.Seedtype == 10)
            {
                go = NGUITools.AddChild(FruitTable.gameObject, prefab);
            }
            else if (table.Seedtype == 20)
            {
                go = NGUITools.AddChild(VegeTable.gameObject, prefab);
            }

            if (go != null)
            {
                ShopItemView view = go.GetComponent<ShopItemView>();
                view.SetData(table);
                view.UpdateView();
                ItemViews.Add(view);                                
            }
        }

        FruitTable.repositionNow = true;
        VegeTable.repositionNow = true;
    }

    //********  OnClick  *************
    public void OnCloseClick()
    {
        gameObject.SetActive(false);
    }

    public void OnTabClick(GameObject go)
    {
        if (go != null)
        {
            if (go.name == "ToFruit" && !FruitKind.activeSelf)
            {
                FruitKind.SetActive(true);
                VegeKind.SetActive(false);
            }
            else if (go.name == "ToVege" && !VegeKind.activeSelf)
            {
                VegeKind.SetActive(true);
                FruitKind.SetActive(false);
            }
        }
    }


	
}
