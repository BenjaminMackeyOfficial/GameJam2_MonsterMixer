using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class WinConDept : ItemDepot
{
    protected override void Start()
    {
        base.Start();
        EventBus.RequestEvent("GaveItemAndProgressed", true).ping += PositiveResponse;
        EventBus.RequestEvent("GaveUslessItem", true).ping += NegativeResponse;
        EventBus.RequestEvent("PlayerWon", true).ping += WinningResponse;
        EventBus.RequestEvent("StartTutorial", true).ping += ProgressDialouge;
        ServiceHub.Instance.roundProgressionManager.HandOverRequiredTags(wantedTags);

        EventBus.RequestEvent("LevelReseting", true).ping += ResetData;
    }
    private void ResetData()
    {
        counter = -1;
        index = -1;
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

    private int index = -1;
    [Header("TutorialText")]
    [SerializeField] string[] text;
    public void ProgressDialouge()
    {
        index += 1;
        if (index >= text.Length) return;
        else if (index == text.Length - 1)
        {
            EventBus.RequestEvent("StartGame", true).Invoke();
            //EventBus.RequestEvent("PlayerWon", true).Invoke(); // <--temp, this is jsut for testing
            ServiceHub.Instance.popupManager.CreateDialouge("", gameObject);
        }
        else ServiceHub.Instance.popupManager.CreateDialouge(text[index], gameObject, false, true);
    }
    public override void Click(GameObject heldItem, bool leftHand)
    {
        base.Click(heldItem,leftHand);
        ProgressDialouge();
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
    private int counter = -1;
    [SerializeField] string[] rewardDialouge;
    private void PositiveResponse()
    {
        counter += 1;
        if (counter > rewardDialouge.Length - 1) return;
        ServiceHub.Instance.popupManager.CreateDialouge(rewardDialouge[counter], gameObject);
    }
    private void NegativeResponse()
    {
        ServiceHub.Instance.popupManager.CreateDialouge("...What the hell is this? You think this is going to end this? bring me 6 unique trinkets", gameObject);
    }
    private void WinningResponse()
    {
        //ServiceHub.Instance.popupManager.CreateDialouge("Well that was it... you did it.. ..see you next year... heh", gameObject);
    }
}
