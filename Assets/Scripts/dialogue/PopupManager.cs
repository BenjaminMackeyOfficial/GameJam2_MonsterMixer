using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField] GameObject popupDialougePrefab;
    [SerializeField] GameObject popupNumberLabelPrefab;

    private List<PopupBox> popupBoxes = new List<PopupBox>();

    public void DialougeBoxDestroying(PopupBox box)
    {
        popupBoxes.Remove(box);
        Destroy(box.gameObject);
    }
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
            Debug.Log("Making new obj...");
            GameObject canvObj = Instantiate(popupDialougePrefab);
            box = canvObj.GetComponent<PopupDialouge>();

            box.Initialize(targObj);
            popupBoxes.Add(box);
        }
        box.ShowCanvas(text, forceHandsCamera);

    }


    public void CreateDialouge(string text, GameObject targObj, bool forceHandsCamera, bool holdUntilForcedOff)
    {
        PopupDialouge box = HasMatch<PopupDialouge>(targObj);

        if (box == null)
        {
            Debug.Log("Making new obj...");
            GameObject canvObj = Instantiate(popupDialougePrefab);
            box = canvObj.GetComponent<PopupDialouge>();

            box.Initialize(targObj);
            popupBoxes.Add(box);
        }
        box.ShowCanvas(text, forceHandsCamera, holdUntilForcedOff);
    }

    public void ShowText(int text, GameObject targObj)
    {
       
    }
}
