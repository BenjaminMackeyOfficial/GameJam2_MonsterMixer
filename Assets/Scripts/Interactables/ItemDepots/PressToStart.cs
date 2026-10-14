using UnityEngine;

public class PressToStart : ItemDepot, Iinteractable
{
    public override void Click(GameObject heldItem, bool leftHand)
    {
        EventBus.RequestEvent("StartGame", true).Invoke();
    }
}
