using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField] GameObject popupDialougePrefab;
    [SerializeField] GameObject popupNumberLabelPrefab;

    private List<PopupBox> popupBoxes = new List<PopupBox>();

    private T HasMatch<T>(GameObject obj) where T : PopupBox
    {
        foreach (PopupBox item in popupBoxes)
        {
            if(item.attatchedObj == null) continue;
            if (item.attatchedObj == obj && item is T match)
                return match;
        }
        return null;
    }

    public void CreateDialouge(string text, GameObject targObj)
    {
        PopupDialouge box = HasMatch<PopupDialouge>(targObj);
        if(box == null)
        {
            GameObject canvObj = Instantiate(popupDialougePrefab);
            box = canvObj.GetComponent<PopupDialouge>();
            
            box.Initialize(targObj);
            popupBoxes.Add(box);
        }
        box.ShowCanvas(text);
        
    }

    public void CreateDialouge(string text, GameObject targObj, bool forceHandsCamera)
    {
        PopupDialouge box = HasMatch<PopupDialouge>(targObj);
        if (box == null)
        {
            GameObject canvObj = Instantiate(popupDialougePrefab);
            box = canvObj.GetComponent<PopupDialouge>();

            box.Initialize(targObj);
            popupBoxes.Add(box);
        }
        box.ShowCanvas(text, forceHandsCamera);

    }


    public void ShowText(int text, GameObject targObj)
    {
       
    }
}
