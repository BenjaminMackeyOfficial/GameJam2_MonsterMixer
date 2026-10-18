using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundProgressionManager : MonoBehaviour
{
    public List<Tag> haveRecieved;
    [SerializeField] Tag[] totalLookingFor;
    public void Initialize()
    {  
        
    }
    void Start()
    {
        EventBus.RequestEvent("LevelReseting", true).ping += ResetData;
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
            EventBus.RequestEvent("GaveItemAndProgressed", true).Invoke();
        }
        else
        {
            EventBus.RequestEvent("GaveUslessItem", true).Invoke();
        }

        countSinceLastTime = haveRecieved.Count;
        Debug.Log(countSinceLastTime);
        Debug.Log(totalLookingFor.Length);
        if (countSinceLastTime >= 6) StartCoroutine(VictorySpeach());
    }
    private void ResetData()
    {
        haveRecieved = new List<Tag>();
    }
    private IEnumerator VictorySpeach()
    {
        ServiceHub.Instance.gameState.AttemptEnterDownTime();
        yield return new WaitForSeconds(5);
        EventBus.RequestEvent("PlayerWon", true).Invoke();
    }
}
