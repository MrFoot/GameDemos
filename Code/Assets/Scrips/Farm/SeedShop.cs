using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeedShop{

    private static SeedShop _instance;
    public static SeedShop Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SeedShop();
            }
            return _instance;
        }
    }

    private SeedShopWin m_window;
    public void SetWindow(SeedShopWin win) { m_window = win; }

    public List<FarmSeedTable> FarmSeedTables = new List<FarmSeedTable>();

    public void Init()
    {
        TableDataDict dic = Main.Instance.TableManager.GetTable((int)TableID.FarmSeedTableID);

        foreach(KeyValuePair<int,TableBaseData> pair in dic.TableDatas)
        {
            FarmSeedTable table = pair.Value as FarmSeedTable;
            FarmSeedTables.Add(table);
        }
    }

    public void ShowWin(bool isShow)
    {
        if (m_window != null)
        {
            m_window.gameObject.SetActive(isShow);
        }
    }

    public void Buy(FarmSeedTable table)
    {
        Debug.Log("Wana Buy Seed : " + table.Name);
        FarmGameData.Instance.Bag.AddSeed(table.Id);
    }




}
