using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]
public class CenterElements : MonoBehaviour {

    public List<UIWidget> Widgets;

    //Gap.Count should be Widgets.Count - 1
    public List<int> Gaps;

    public int CommonY = 0;

    [HideInInspector]
    public bool IsDirty = false;

	// Use this for initialization
	void Start () {
        IsDirty = true;
	}

    void Update()
    {
        if (!IsDirty) return;

        IsDirty = false;
        Center();
    }

    public void Center()
    {
        int width = 0;
        int i=0;
        foreach (UIWidget w in Widgets)
        {
            if (!w.gameObject.activeSelf) continue;

            width += w.width;
            if (i <= Gaps.Count)
            {
                if (i > 0)
                {
                    width += Gaps[i-1];
                }
                i++;
            }
        }

        i = 0;
        int x = 0;

        foreach (UIWidget w in Widgets)
        {
            if (!w.gameObject.activeSelf) continue;

            w.pivot = UIWidget.Pivot.Left;
            if (i > 0)
            {
                x += Widgets[i - 1].width;
                if (i <= Gaps.Count && Gaps[i - 1] != null)
                {
                    x += Gaps[i - 1];
                }
            }
            i++;
            w.transform.localPosition = new Vector3(-width / 2 + x, CommonY, 0);
        }

    }
	
}
