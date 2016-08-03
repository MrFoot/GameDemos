using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FarmGameControler : MonoBehaviour {
    private const int LAND_COUNT = 12;
    private Vector3 SEED_DRAG_OFFSET_VEC3 = Vector3.zero;
    private Vector2 SEED_DRAG_OFFSET_VEC2 = Vector2.zero;

    public UILabel text;
    public UISprite sprite;
    private bool m_isSeedReady = false;
    public UISprite DragSprite;
    private UIRoot mRoot;
    private Transform mHoverLandTrans;

    public GameObject LandParent;
    private List<LandView> LandViews = new List<LandView>();

    private Land mWantForceOpenLand;

    public SeedShopWin seedShopWin;

    void Awake()
    {
        mRoot = NGUITools.FindInParents<UIRoot>(DragSprite.transform);

        //Init Managers
        SeedShop.Instance.Init();
        SeedShop.Instance.SetWindow(seedShopWin);
    }

    // Use this for initialization
    void Start()
    {
        text.text = "HELLO WORLD";

        UIEventListener.Get(sprite.gameObject).onPress = OnSeedPress;
        UIEventListener.Get(sprite.gameObject).onDrag = OnSeedDrag;

        for (int i = 0; i < LAND_COUNT; i++)
        {
            Transform trans = LandParent.transform.Find("Land" + i);
            LandView view = trans.GetComponent<LandView>();
            view.SetData(FarmGameData.Instance.Lands[i]);
            
            LandViews.Add(view);
        }

        UpdateViews();
    }

    void UpdateViews()
    {
        for (int i = 0; i < LAND_COUNT; i++)
        {
            LandView view = LandViews[i];
            view.UpdateView();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
#if UNITY_EDITOR
        HandleMouse();
#else
        HandleTouch();
#endif

        //for (int i = 0; i < LAND_COUNT; i++)
        //{
        //    LandViews[i].Tick();
        //}
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                OnFoucsLand(hitInfo.transform);
            }
        }
        else if (m_isSeedReady && Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition + SEED_DRAG_OFFSET_VEC3);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                OnWantPlant(hitInfo.transform);
            }
            else
            {
                text.text = "Hit = none";
            }
        }
        else if (m_isSeedReady && Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition + SEED_DRAG_OFFSET_VEC3);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                OnHoverLand(hitInfo.transform);
            }
            else
            {
                text.text = "Mouse cross none";
                OnHoverLand(null);
            }
        }
    }

    void HandleTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                OnFoucsLand(hitInfo.transform);
            }
        }
        else if (m_isSeedReady && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position + SEED_DRAG_OFFSET_VEC2);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                OnWantPlant(hitInfo.transform);
            }
            else
            {
                text.text = "Hit = none";
            }
        }
        else if (m_isSeedReady && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position + SEED_DRAG_OFFSET_VEC2);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                OnHoverLand(hitInfo.transform);
            }
            else
            {
                text.text = "Touch cross none";
                OnHoverLand(null);
            }
        }
    }

    private int getIndex(Transform trans)
    {
        if (trans != null)
        {
            return int.Parse(trans.name.Substring(4, trans.name.Length - 4));
        }
        else
        {
            return -1;
        }
    }

    //点击种子
    public void OnSeedPress(GameObject go,bool isPress)
    {
        if (isPress)
        {
            DragSprite.transform.localPosition = sprite.transform.localPosition + SEED_DRAG_OFFSET_VEC3;
        }
        else if (!isPress)
        {
#if UNITY_EDITOR
            HandleMouse();
#else
            HandleTouch();
#endif
        }

        //should be after here
        m_isSeedReady = isPress;
        DragSprite.gameObject.SetActive(isPress);

    }

    //拖拽种子
    public void OnSeedDrag(GameObject go, Vector2 delta)
    {
        if (!m_isSeedReady) return;

        Vector3 dt = (Vector3)delta * mRoot.pixelSizeAdjustment;
        DragSprite.transform.localPosition += dt;
    }

    //点击某土地
    void OnFoucsLand(Transform landTrans)
    {
        int index = getIndex(landTrans);

        Land land = FarmGameData.Instance.Lands[index];

        Crop crop = FarmGameData.Instance.Lands[index].crop;
        if (crop != null)
        {
            if (crop.IsMature)
            {
                land.Harvest();
            }
            else
            {
                text.text = "LandId = " + land.Table.Id + ", CropId = " + crop.Table.Id;
            }
            
        }
        else
        {
            if (land.IsActive)
            {
                text.text = "LandId = " + land.Table.Id;
            }
            else
            {
                text.text = "Need Lv : " + land.Table.Needlv;

                OnWantForceOpen(land);
            }

        }

    }

    //
    void OnHoverLand(Transform landTrans)
    {
        if (landTrans == mHoverLandTrans) return;
        
        mHoverLandTrans = landTrans;

        int index = getIndex(landTrans);

        if (mHoverLandTrans != null)
        {
            text.text = "HoverLand  = " + landTrans.name;
        }
        else
        {
            text.text = "HoverLand  = none";
        }

        for (int i = 0; i < LAND_COUNT; i++)
        {
            LandViews[i].OnHover(index == i);
        }
    }

    //
    void OnWantPlant(Transform landTrans)
    {
        int index = getIndex(landTrans);
        Land land = FarmGameData.Instance.Lands[index];
        if (land.crop == null)
        {
            if (land.IsActive)
            {
                land.Plant(101);
                FarmGameData.Instance.Save();
            }
            else
            {
                text.text = "Not Available!";    
            }
            
        }
        else
        {
            text.text = "Already Have Crop";
        }
    }

    void OnWantForceOpen(Land land)
    {
        //打开窗口询问是否解锁
        mWantForceOpenLand = land;
        
    }

    void OnForceOpenBtnClick()
    {
        if (mWantForceOpenLand != null)
        {
            FarmGameData.Instance.TryForceOpen(mWantForceOpenLand);
            mWantForceOpenLand = null;
        }
        
    }


    //******************* UI Button *****************
    public void OnSeedShopClick()
    {
        SeedShop.Instance.ShowWin(true);
    }

    public void OnSeedBagClick()
    {
    }


    //***********************************************
}
