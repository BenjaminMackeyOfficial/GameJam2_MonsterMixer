using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public enum DepotType
{
    Give, //things like a basket of lemons, only gives items 
    Recieve, //things that only recieve items, like vampires
    Alter, //things that alter items, like a mixer
    Mix, //will let you combine things that can be combine
}
public class ItemDepot : MonoBehaviour, Iinteractable
{
    [SerializeField] Color hoverColor;
    [SerializeField] string hoverText;
    [SerializeField] DepotType type;

    [SerializeField] bool enforceWrongTags;//denys taking the item if it doesnt have all the tags
    [SerializeField] bool hasInfinate;
    [SerializeField] Tag[] wantedTags;
    [SerializeField] Tag[] giveTags;

    //stuff about things that may take time
    private bool hasItem;
    private ItemTags heldItemTags;
    private GameObject heldObject;

    private bool proscessing;
    private float proscessingRemaining;
    //

    //events
    private CustomEvent giveToLeftHand;
    private CustomEvent giveToRightHand;
    //

    public void Click(GameObject heldItem, bool leftHand) //true for left, false for right
    {
        string eventName = "GiveToRightHand";
        if(leftHand) eventName = "GiveToLeftHand";
        if(CanTake(heldItem.GetComponent<ItemTags>()))
        {
            GameObject returnObj = TakeItem(heldItem);
            EventBus.RequestEvent(eventName, true).AddData(new GameObjData(returnObj));
            EventBus.RequestEvent(eventName, true).Invoke();
            return;
        }
        if(CanGive())
        {
            GameObject returnObj = GiveItem();
            EventBus.RequestEvent(eventName, true).AddData(new GameObjData(returnObj));
            EventBus.RequestEvent(eventName, true).Invoke();
            return;
        }
        
    }
    public void Hover()
    {

    }
    public void UnHover()
    {
        
    }

    public bool CanTake(ItemTags tags)
    {
        if(hasItem && proscessing) return false; //busy proscessing something already

        if(hasItem && type == DepotType.Mix) return true; //if it

        if(type == DepotType.Give) return false; //givers cant take items

        if(type != DepotType.Give && tags != null && enforceWrongTags == false) return true; // vampires are this, checking if the item has tags (all do) but not checking if right ones (yet)

        foreach (Tag tag in wantedTags)
        {
            Tag foundTag = tags.HasTag(tag._name);
            if(foundTag == null) return false; //missing a tag
        }
        return true; //makes it through checking against all required tags
    }

    
    public bool CanGive() // give as in the player recieves
    {
        if(type == DepotType.Recieve) return false;
        if(hasItem && !proscessing) return true;
        return false;
    }

    public GameObject TakeItem(GameObject item) //vampires will extend off this
    {
        ItemTags tags = item.GetComponent<ItemTags>();
        if(type == DepotType.Give) return item; //shouldnt happen, but can never be to careful
        if(type == DepotType.Mix)
        {
            if(hasItem == false)
            {
                heldItemTags = tags;
                heldObject = item;
                hasItem = true;   
            }
            foreach (Tag tag in tags.GetAllTags())
            {
                heldItemTags.AddTag(tag);
            }
            Destroy(item);
            return null;
        }
        if(type == DepotType.Alter || type == DepotType.Recieve)
        {
            GameObject toReturn = null;
            if(hasItem && type == DepotType.Alter) toReturn = heldItemTags.gameObject;

            heldItemTags = tags;
            return toReturn;
        }
        return item;
    }

    public GameObject GiveItem()
    {
        GameObject giving = heldObject;
        if(hasInfinate) 
        {
            giving = Instantiate(giving);
            return giving;
        }
        else
        {
            heldItemTags = null;
            heldObject = null;
            return giving;
        }
    }


    void Start()
    {
        if(this.GetComponent<Outline>() == null)
        {
            Outline outline = this.AddComponent<Outline>();
            outline.effectColor = hoverColor;
        }
        else
        {
            Outline outline = this.GetComponent<Outline>();
            outline.effectColor = hoverColor;
        }

        if(type == DepotType.Give)
        {
            heldObject = new GameObject();
            heldItemTags = heldObject.AddComponent<ItemTags>();
        }

    }
}
