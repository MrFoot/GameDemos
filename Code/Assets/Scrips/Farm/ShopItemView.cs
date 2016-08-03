using UnityEngine;
using System.Collections;

public class ShopItemView : MonoBehaviour {

    private FarmSeedTable _table;

    public UILabel Name;
    public UISprite SeedSpr;
    public UILabel CostTime;
    public UILabel Price;
    public UISprite CostSpr;
    public UILabel ItemNumHas;
    public CenterElements PriceCenter;
    public CenterElements HasCenter;

    public void SetData(FarmSeedTable table)
    {
        _table = table;
    }

    public void UpdateView()
    {
        if (_table == null) return;

        Name.text = _table.Name;
        //SeedSpr.spriteName = _table.Icon;
        CostTime.text = ((float)_table.Needtime / 60).ToString() + "h";
        //CostSpr.spriteName = 
        Price.text = _table.Price.ToString();

        //ItemNumHas.text = 
        PriceCenter.Center();
        HasCenter.Center();
    }

    public void OnBtnClick()
    {
        if (_table == null) return;
        SeedShop.Instance.Buy(_table);
        Debug.Log(_table.Name + " : " + FarmGameData.Instance.Bag.Get(_table.Id).Count);
    }
}
