using UnityEngine;

public class VampireScorer : ItemDepot
{
    private VampireGeneralController controller;
    protected override void Start()
    {
        base.Start();
        TryGetComponent<VampireGeneralController>(out controller);
        if (controller == null) Destroy(this);
    }
    private void Score(ItemTags obj)
    {
        float score = 100;
        bool perfect = true;
        if (controller.waitTime < controller.maxWaitTimeToGetAchievment)
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
            if (obj.HasTag(item._name) == null) //checking all the tags in the wanted tag list
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
                if (check._name == item._name) match = true;
            }
            if (match == false)
            {
                score -= 5;
                perfect = false;
            }
        }

        if (perfect != true) score *= 0.7f; //cuts score if you didnt get a perfect score

        controller.Scored(score, perfect);
    }
    public override GameObject TakeItem(GameObject item) //vampires will extend off this
    {
        type = DepotType.Recieve;
        ItemTags tags = item.GetComponent<ItemTags>();

        Score(tags);

        Destroy(item);
        return null;
    }
}
