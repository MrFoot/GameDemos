using UnityEngine;
using System;
using System.Collections;

public class LandView : MonoBehaviour {
    public MeshRenderer render;
    
    public UIPanel Panel;
    public UILabel RemainTime;
    public UILabel Tips;
    public UISprite CropSpr;
    public UISprite StateSpr;

    public Land _land;

    public Material Green;
    public Material Default;
    public Material HighLight;

    private const string SPRITENAME_STATE_INSECTS = "Emoticon - Skull";
    private const string SPRITENAME_STATE_THIRSTY = "Emoticon - Dead";
    private const string SPRITENAME_STATE_AVALIABLE = "Emoticon - Smirk";

    public void SetData(Land land)
    {
        _land = land;
        _land.SetView(this);
    }

    void Awake()
    {
        
    }

	void Start () 
    {
        RemainTime.gameObject.SetActive(false);
        CropSpr.gameObject.SetActive(false);
        StateSpr.gameObject.SetActive(false);
        Tips.gameObject.SetActive(false);
	}
	
	void Update()
    {
        if (!RemainTime.gameObject.activeSelf) return;
        TimeSpan remain = new TimeSpan(_land.crop.MatureRemain.Hours,_land.crop.MatureRemain.Minutes,_land.crop.MatureRemain.Seconds);
        RemainTime.text = remain.ToString();

        if (_land.crop.IsMature)
        {
            UpdateView();
        }
	}

    public void SetDepth(int depth)
    {
        Panel.depth = depth;
    }

    public void UpdateView()
    {
        if (_land == null) return;

        RemainTime.gameObject.SetActive(_land.crop != null && !_land.crop.IsMature);
        if(_land.crop != null)
        {
            RemainTime.text = _land.crop.MatureRemain.ToString();
        }

        CropSpr.gameObject.SetActive(_land.crop != null);
        StateSpr.gameObject.SetActive(_land.crop != null);
        Tips.gameObject.SetActive(!_land.IsActive);
        Tips.text = "Lv." + _land.Table.Needlv;

        if (_land.crop != null)
        {
            
            switch(_land.crop.State)
            {
                case CropBuff.Insects:
                    StateSpr.spriteName = SPRITENAME_STATE_INSECTS;
                    break;
                case CropBuff.Thirsty:
                    StateSpr.spriteName = SPRITENAME_STATE_THIRSTY;
                    break;

                case CropBuff.Healthy:
                    if (_land.crop.IsMature)
                    {
                        StateSpr.spriteName = SPRITENAME_STATE_AVALIABLE;
                    }
                    else
                    {
                        StateSpr.gameObject.SetActive(false);
                    }
                    break;
            }
            
        }

        render.material = _land.crop != null ? Green : Default;
        
    }

    public void OnHover(bool isHover)
    {
        if (_land != null && _land.crop != null) return;
        if (!_land.IsActive) return;

        render.material = isHover ? HighLight : Default;
    }

    public void Plant(int seedId)
    {
 
    }

}
