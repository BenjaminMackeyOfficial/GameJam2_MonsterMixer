using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public enum VampireState
{
    Entering,
    Waiting,
    Exiting
}
public class VampireGeneralController : MonoBehaviour
{
    public float waitTime = 0;

    public Vector3 ReturnPoint;
    public bool ableToGive = false; // <---- this is if the player already has the wincon
    
    [Header("How quickly score goes down based on wait time")]

    public float patienceLevel;
    public float maxWaitTimeToGetAchievment;

    private Vector3 currentTargetPos = Vector3.zero;
    
    public VampireState state;
    public BarPosition barPosition;

    [SerializeField] string eventPreheader;


    [Header("DIALOUGE OPTIONS")]

    [SerializeField] public string[] DialougeArivedAtBar;
    [SerializeField] public string[] DialougeUnhappyMidDrinkMake;
    [SerializeField] public string[] DialougePleasedMidDrinkMake;
    [SerializeField] public string[] DialougeLeavingBarUpset;
    [SerializeField] public string[] DialougeLeavingBarHappy;
    [SerializeField] public string[] DialougeLeavingBarPerfect;

    void Start()
    {
        if(DialougeArivedAtBar == null || DialougeArivedAtBar.Length == 0) DialougeArivedAtBar = new string[] { "..." };
        if(DialougeUnhappyMidDrinkMake == null || DialougeUnhappyMidDrinkMake.Length == 0) DialougeUnhappyMidDrinkMake = new string[] { "..." };
        if(DialougePleasedMidDrinkMake == null || DialougePleasedMidDrinkMake.Length == 0) DialougePleasedMidDrinkMake = new string[] { "..." };
        if(DialougeLeavingBarUpset == null || DialougeLeavingBarUpset.Length == 0) DialougeLeavingBarUpset = new string[] { "..." };
        if(DialougeLeavingBarHappy == null || DialougeLeavingBarHappy.Length == 0) DialougeLeavingBarHappy = new string[] { "..." };
        if(DialougeLeavingBarPerfect == null || DialougeLeavingBarPerfect.Length == 0) DialougeLeavingBarPerfect = new string[] { "..." };
    }

    public void Scored(float givenScore, bool isPerfect)
    {
        Debug.Log("Score: " + givenScore + " | perfect: " + isPerfect);

        if(isPerfect) ServiceHub.Instance.popupManager.CreateDialouge(DialougeLeavingBarPerfect.Random(), gameObject);
        else if(givenScore > 70) ServiceHub.Instance.popupManager.CreateDialouge(DialougeLeavingBarHappy.Random(), gameObject);
        else ServiceHub.Instance.popupManager.CreateDialouge(DialougeLeavingBarUpset.Random(), gameObject);

        barPosition.occupied = false;
        state = VampireState.Exiting;
        ServiceHub.Instance.tips.AddTipAmount(givenScore);
        WalkTo(ReturnPoint);
    }

    public void WalkTo(Vector3 pos)
    {
        currentTargetPos = pos;
    }

    private void ArrivedAtBar()
    {
        if(state == VampireState.Exiting) return; //shouldnt be called but hey
        state = VampireState.Waiting;

        //any ui effects or ding sounds, perhaps an event
        if(DialougeArivedAtBar != null) ServiceHub.Instance.popupManager.CreateDialouge(DialougeArivedAtBar.Random(), gameObject);
        EventBus.RequestEvent(eventPreheader + "Dialouge", true).Invoke();
        //
    }
    private void UpdateWaitTime()
    {
        waitTime+= Time.deltaTime;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTargetPos, Time.deltaTime * patienceLevel);

        if(state == VampireState.Waiting) UpdateWaitTime();
        if(state == VampireState.Entering && transform.position == currentTargetPos) ArrivedAtBar(); 
        if(state == VampireState.Exiting && transform.position == ReturnPoint) Destroy(gameObject);
    }
}
