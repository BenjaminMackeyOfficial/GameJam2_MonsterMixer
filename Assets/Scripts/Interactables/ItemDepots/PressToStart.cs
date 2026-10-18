using UnityEngine;

public class PressToStart : ItemDepot, Iinteractable
{
    private float lastClicked = 0;
    public override void Click(GameObject heldItem, bool leftHand)
    {
        if (Time.time < lastClicked + 7) return;
        lastClicked = Time.time;

        EventBus.RequestEvent("StartTutorial", true).Invoke();
    }
}
