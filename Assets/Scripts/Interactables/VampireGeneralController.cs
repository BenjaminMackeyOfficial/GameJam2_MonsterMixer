using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

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

    public void Scored(float givenScore, bool isPerfect)
    {
        Debug.Log("Score: " + givenScore + " | perfect: " + isPerfect);
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
