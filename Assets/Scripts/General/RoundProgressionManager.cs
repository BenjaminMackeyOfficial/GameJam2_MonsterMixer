using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoundProgressionManager : MonoBehaviour
{
    public List<Tag> haveRecieved;
    [SerializeField] Tag[] totalLookingFor;
    public void Initialize()
    {  
        
    }

    public void HandOverRequiredTags(Tag[] tags)
    {
        if(totalLookingFor == null) totalLookingFor = tags;
    }
    public void AddToHasRecieved(Tag tag)
    {
        haveRecieved.Add(tag);
        UpdateShownProgress();
    }
    public void FailAddRecieve()
    {
        UpdateShownProgress();
    }
    private int countSinceLastTime = 0;
    private void UpdateShownProgress()// whatever needs to happe
    {
        if(haveRecieved.Count > countSinceLastTime)
        {
            Debug.Log("Hey, yeah okay, this'll work..");
        }
        else
        {
            Debug.Log("Nope, i dont want whatever that is...");
        }
        countSinceLastTime = haveRecieved.Count;
    }
}
