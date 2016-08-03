using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Soulgame.Util;

//地块
public class Land
{
    public FarmLandTable Table;
    public bool IsForceActive = false;   //是否强制激活
    public bool IsActive       //是否激活状态（是否开启该土地）
    {
        get {
            if (IsForceActive) return true;
            return Table.Needlv <= 1;
        }
    }
    public Land(int id, bool isForceActive = false)
    {
        Table = Main.Instance.TableManager.GetTableData((int)TableID.FarmLandTableID, id) as FarmLandTable;


        IsForceActive = isForceActive;
        _crop = null;
    }

    public LandView view;
    public void SetView(LandView v)
    {
        view = v;
    }

    private Crop _crop;       //该土地播种的庄稼
    public Crop crop { get { return _crop;} }

    public void Plant(int cropId)   //播种
    {
        if (!IsActive) return;
        _crop = new Crop(cropId);
        if (view)
        {
            view.UpdateView();
        }
    }

    public void Plant(Crop c)
    {
        if (!IsActive) return;
        _crop = c;
        if (view)
        {
            view.UpdateView();
        }
    }

    public void Harvest()    //收割
    {
        if (_crop == null ||  !_crop.IsMature) return;

        Debug.Log("Gain (" + _crop.Table.Gaintype + ") : " + _crop.Table.Gaingold);
        _crop = null;
        FarmGameData.Instance.Save();

        if (view)
        {
            view.UpdateView();
        }
    }

    public void Water()
    {
        if (_crop == null || _crop.State != CropBuff.Thirsty) return;

        _crop.State = CropBuff.Healthy;
        FarmGameData.Instance.Save();

        if (view)
        {
            view.UpdateView();
        }
    }

    public void KillInsects()
    {
        if (_crop == null || _crop.State != CropBuff.Insects) return;

        _crop.State = CropBuff.Healthy;
        FarmGameData.Instance.Save();

        if (view)
        {
            view.UpdateView();
        }
    }
}

public enum CropBuff
{
    Healthy,   //健康
    Thirsty,   //缺水
    Insects,   //蛀虫
}

//庄稼
public class Crop
{
    public DateTime SowTime; //播种时间
    public DateTime HarvestTime;  //收割时间
    public CropBuff State;    //作物状态

    public FarmSeedTable Table;

    public Crop(int id)
    {
        Table = Main.Instance.TableManager.GetTableData((int)TableID.FarmSeedTableID, id) as FarmSeedTable;
        DateTime now = DateTime.Now;
        SowTime = new DateTime(now.Year,now.Month,now.Day,now.Hour,now.Minute,now.Second);
        
        HarvestTime = SowTime.AddMinutes(Table.Needtime);
    }

    public Crop(int id, DateTime sowtime)
    {
        if (sowtime != DateTime.MinValue)
        {
            SowTime = sowtime;
            Table = Main.Instance.TableManager.GetTableData((int)TableID.FarmSeedTableID, id) as FarmSeedTable;
            HarvestTime = SowTime.AddMinutes(Table.Needtime);
        }
        else
        {
            Debug.LogError("SowTime Error");
        }
    }

    public bool IsMature     //是否已经成熟
    {
        get
        {
            return MatureRemain <= TimeSpan.Zero;
        }
    }

    public TimeSpan MatureRemain   //距离成熟剩余时间
    {
        get
        {
            TimeSpan delta = HarvestTime - DateTime.Now;
            return delta > TimeSpan.Zero ? delta : TimeSpan.Zero;
        }
    }
}

//种子
public class Seed
{
    public int Count;
    public FarmSeedTable Table;

    public Seed(int id,int count = 0)
    {
        Table = Main.Instance.TableManager.GetTableData((int)TableID.FarmSeedTableID, id) as FarmSeedTable;
        if (Table == null)
        {
            Debug.LogError("Error Seed Id!");
        }
        Count = count;
    }
}


public class FarmGameData{
    private static FarmGameData _instance;

    public static FarmGameData Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = new FarmGameData();
            }
            return _instance;
        }
    }

    public FarmGameEntity Entity;

    private const string PrefPath = "Farm.FarmGameEntity";
    private const int LAND_COUNT = 12;

    public SeedBag Bag; //种子仓库

    public List<Land> Lands;

    private FarmGameData() { }  //将构造函数私有

    public void Init()
    {
        load();
    }

    private void load()
    {
        if (Entity == null)
        {
            Entity = UserPrefs.GetXml<FarmGameEntity>(PrefPath, null);
            if (Entity != null)
            {
                Debug.Log("Farm Data Load Suc");
            }
            else
            {
                initEntity();
                Debug.Log("Farm Data Init Suc");
            }

            initSeedBag(Entity.SeedBag);
            initLands(Entity.Lands);
        }
    }


    //********************初始化数据***************

    private void initEntity()
    {
        Entity = new FarmGameEntity();
        Entity.SeedBag = new List<SeedEntity>();
        Entity.Lands = new List<LandEntity>();

        for (int i = 0; i < LAND_COUNT; i++)
        {
            LandEntity land = new LandEntity();
            land.Crop = null;
            land.IsForceActive = false;
            Entity.Lands.Add(land);
        }

        UserPrefs.SetXml<FarmGameEntity>(PrefPath, Entity);
    }

    private void initSeedBag(List<SeedEntity> seedBag)
    {
        Bag = new SeedBag(seedBag);
    }

    private void initLands(List<LandEntity> lands)
    {
        Lands = new List<Land>();
        int idx = 1;
        foreach (LandEntity entity in lands)
        {
            Land land = new Land(idx, entity.IsForceActive);

            if (entity.Crop != null)
            {
                Crop crop = new Crop(entity.Crop.CropId, entity.Crop.SowTime);
                land.Plant(crop);
            }
            Lands.Add(land);
            idx++;
        }
    }

    //*********************************************

    public void Save()
    {
        if (Bag == null || Lands == null) return;
        Entity.SeedBag = Bag.GetEntity();
        Entity.Lands = new List<LandEntity>();
        for (int i = 0, max = Lands.Count; i < max; i++)
        {
            LandEntity entity = new LandEntity();
            entity.IsForceActive = Lands[i].IsForceActive;
            if (Lands[i].crop != null)
            {
                CropEntity cropEntity = new CropEntity();
                cropEntity.CropId = Lands[i].crop.Table.Id;
                cropEntity.SowTime = Lands[i].crop.SowTime;
                cropEntity.State = Lands[i].crop.State;
                entity.Crop = cropEntity;
            }
            Entity.Lands.Add(entity);
        }

        UserPrefs.SetXml<FarmGameEntity>(PrefPath, Entity);
    }

    public void Reset()
    {
        UserPrefs.Remove(PrefPath);
        load();
    }

    //************************Method********************

    public void TryForceOpen(Land land)
    {
        if (land.Table.Id > 1)
        {
            Land preLand = FarmGameData.Instance.Lands[land.Table.Id - 2];  //-2 ： Id start with 1
            if (preLand.IsActive)
            {
                land.IsForceActive = true;
                FarmGameData.Instance.Save();
                land.view.UpdateView();
            }
            else
            {
                Debug.Log("U Must Force Open Land By Land~");
            }
        }
    }


    //*************************************************

}
