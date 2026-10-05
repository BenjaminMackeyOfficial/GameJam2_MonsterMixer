using UnityEngine;
using UnityEngine.UI;

public class ItemDepotTagApplier : MonoBehaviour, Iinteractable
{
    private ItemDepot depot;

    public void Hover()
    {
        
    }
    public void UnHover()
    {
        
    }
    public void Click(GameObject heldItem, bool leftHand)
    {
        depot.ApplyTags();
    }

    void Start()
    {
        transform.parent.TryGetComponent<ItemDepot>(out depot);
        if(depot == null) 
        {
            Debug.LogError("Depot Tag Applier Script Added To Non Dept!");
            return;
        }
    }
}
