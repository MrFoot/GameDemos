using UnityEngine; 
using UnityEditor; 

public class ToggleActiveRecursively : ScriptableObject { 

    [MenuItem ("Example/Toggle Active Recursively of Selected $k")] 

    static void DoToggle() {

        Object[] objs = Selection.GetFiltered(typeof(GameObject), SelectionMode.DeepAssets);
        foreach (Object o in objs)
        {
            if (o != null)
                Debug.Log("Object.name = " + o.name);
        }
        Debug.Log("objs.Length = " + objs.Length);
    }  

}  
