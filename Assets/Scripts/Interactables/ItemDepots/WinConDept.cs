using UnityEngine;

public class WinConDept : ItemDepot
{
    protected override void Start()
    {
        base.Start();
        EventBus.RequestEvent("GaveItemAndProgressed", true).ping += PositiveResponse;
        EventBus.RequestEvent("GaveUslessItem", true).ping += NegativeResponse;
        EventBus.RequestEvent("PlayerWon", true).ping += WinningResponse;

        ServiceHub.Instance.roundProgressionManager.HandOverRequiredTags(wantedTags);
    }
    private bool RecieveKeyItem(ItemTags obj)
    {
        Tag[] tags = obj.GetAllTags();
        foreach (Tag item in wantedTags)
        {
            foreach (Tag item2 in tags)
            {
                if(Tag.CompareTags(item, item2) && !Tag.ListIncludes(ServiceHub.Instance.roundProgressionManager.haveRecieved, item2))
                {
                    ServiceHub.Instance.roundProgressionManager.AddToHasRecieved(item2);
                    return true;
                }
            }
        }
        ServiceHub.Instance.roundProgressionManager.FailAddRecieve();
        return false;
    }

    public override GameObject TakeItem(GameObject item)
    {
        ItemTags tags = item.GetComponent<ItemTags>();
        Tag[] tagsAsArray = tags.GetAllTags();

        if(RecieveKeyItem(tags))
        {

        }

        Destroy(item);
        return null;
    }

    private void PositiveResponse()
    {
        ServiceHub.Instance.popupManager.CreateDialouge("Yeah... thisll work", gameObject);
    }
    private void NegativeResponse()
    {
        ServiceHub.Instance.popupManager.CreateDialouge("...What the hell is this? You think this is going to end this? bring me 6 unique trinkets", gameObject);
    }
    private void WinningResponse()
    {
        ServiceHub.Instance.popupManager.CreateDialouge("Well that was it... you did it.. ..see you next year... heh", gameObject);
    }
}
