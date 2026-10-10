using UnityEngine;

public class VampireScorer : ItemDepot
{
    private VampireGeneralController controller;
    protected override void Start()
    {
        base.Start();
        TryGetComponent<VampireGeneralController>(out controller);
        if(controller == null) 
        {
            Destroy(this);
        }
    }
    private bool Score(ItemTags obj)
    {
        float score = 100;
        bool perfect = true;
        if(controller.waitTime < controller.maxWaitTimeToGetAchievment)
        {
            score += controller.maxWaitTimeToGetAchievment - controller.waitTime;
        }
        else
        {
            perfect = false;
            score -= controller.waitTime * controller.patienceLevel;
        }

        foreach (Tag item in wantedTags)
        {
            if(obj.HasTag(item._name) == null) //checking all the tags in the wanted tag list
            {
                perfect = false;
                score -= 5;
            }
        }
        foreach (Tag item in obj.GetAllTags())
        {
            bool match = false;
            foreach (Tag check in wantedTags)
            {
                if(check._name == item._name) match = true;
            }
            if(match == false)
            {
                score -= 5;
                perfect = false;
            }
        }

        if(perfect != true) score *= 0.7f; //cuts score if you didnt get a perfect score
        
        controller.Scored(score, perfect);
        return perfect;
    }
    public override GameObject TakeItem(GameObject item)
    {
        ItemTags tags = item.GetComponent<ItemTags>();

        GameObject toReturn = null;

        bool perf = Score(tags);

        if(heldObject == null) return toReturn;

        bool include = Tag.ListIncludes(ServiceHub.Instance.roundProgressionManager.haveRecieved, heldItemTags.GetAllTags()[0]);
        if(perf && !include)
        {
            toReturn = heldObject;
            heldObject = null;
            heldItemTags = null;
        }
        else if(perf && include) // gives the player a money bonus for perfecting a vamp whos plush they already have
        {
            IntData dat = new IntData(50);
            EventBus.RequestEvent("BonusTip", true).AddData(dat);
            EventBus.RequestEvent("BonusTip", true).Invoke();
        }
        
        Destroy(item);
        return toReturn;
    }
}
