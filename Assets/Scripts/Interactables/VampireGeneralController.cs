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

    private Vector3 ReturnPoint;
    
    [Header("How quickly score goes down based on wait time")]

    public float patienceLevel;
    public float maxWaitTimeToGetAchievment;

    public void Scored(float givenScore, bool isPerfect)
    {
        Debug.Log("Score: " + givenScore + " | perfect: " + isPerfect);
    }

    private void WalkTo(Vector3 pos)
    {
        
    }

    void Update()
    {
        
    }
}
