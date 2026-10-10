using UnityEngine;

public class WinConDept : ItemDepot
{
    protected override void Start()
    {
        base.Start();
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
            //do whatever you need, the checking function adds it to the score if its correct
        }

        Destroy(item);
        return null;
    }
}
