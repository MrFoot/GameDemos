using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Soulgame.Util;

/*
 * 
 * 农场实体类 Created By MrFoot @ SoulGame 2016/7/28
 * 
 * 
 **/

public class SeedEntity
{
    public int SeedId;
    public int Count;
}

public class CropEntity
{
    public int CropId;
    public DateTime SowTime;   //播种时间
    public CropBuff State;     //作物状态
}

public class LandEntity
{
    public bool IsForceActive;  //是否强制开启
    public CropEntity Crop;     // null : 没有种植物
}

//实体类，用于序列化
[Serializable]
public class FarmGameEntity
{
    public List<SeedEntity> SeedBag;  //种子仓库
    public List<LandEntity> Lands;    //地块
}
