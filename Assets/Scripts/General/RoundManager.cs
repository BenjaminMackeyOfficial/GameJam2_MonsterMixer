using UnityEngine;

public class RoundManager : MonoBehaviour //deals with switching, like when the player dies it handles all state resets and whatnot
{
    private ServiceHub serviceHub;
    void Start()
    {
        serviceHub = ServiceHub.Instance;
    }

    private void PlayerDiedReset()
    {

    }
    private void PlayerWonReset()
    {

    }
}
