using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum DepotType
{
    Give, //things like a basket of lemons, only gives items 
    Recieve, //things that only recieve items, like vampires
    Alter, //things that alter items, like a mixer
    Mix, //will let you combine things that can be combine
}
public class ItemDepot : MonoBehaviour, Iinteractable
{
    [SerializeField] protected Color hoverColor;
    [SerializeField] string hoverText;
    [SerializeField] protected DepotType type;

    [SerializeField] protected bool enforceWrongTags;//denys taking the item if it doesnt have all the tags
    [SerializeField] protected bool enforceAllTags;
    [SerializeField] protected bool hasInfinate;
    [SerializeField] protected Tag[] wantedTags;
    [SerializeField] protected Tag[] giveTags;

    [Header("This is where the item is visually shown")]
    [SerializeField] GameObject itemDisplayPlatform;

    //stuff about things that may take time
    protected ItemTags heldItemTags;
    [SerializeField] protected GameObject heldObject;
    protected float proscessingRemaining;
    [SerializeField] protected float timeToProscess;
    //

    public void Click(GameObject heldItem, bool leftHand) //true for left, false for right
    {
        string eventName = "GiveToRightHand";
        if(leftHand) eventName = "GiveToLeftHand";//event stuff

        if(heldItem != null && CanTake(heldItem.GetComponent<ItemTags>()))
        {
            GameObject returnObj = TakeItem(heldItem);
            EventBus.RequestEvent(eventName, true).AddData(new GameObjData(returnObj));
            EventBus.RequestEvent(eventName, true).Invoke();
            return;
        }
        if(CanGive(heldItem))
        {
            GameObject returnObj = GiveItem();
            EventBus.RequestEvent(eventName, true).AddData(new GameObjData(returnObj));
            EventBus.RequestEvent(eventName, true).Invoke();
            return;
        }
        
    }
    public void Hover()
    {
        //Debug.Log(gameObject.name);
    }
    public void UnHover()
    {

    }

    public bool CanTake(ItemTags tags)
    {
        if(heldObject != null && proscessingRemaining > 0) return false; //busy proscessing something already

        if(heldObject != null && type == DepotType.Mix) return true; //if it

        if(type == DepotType.Give) return false; //givers cant take items

        if(type != DepotType.Give && tags != null && enforceWrongTags == false) return true; // vampires are this, checking if the item has tags (all do) but not checking if right ones (yet)

        foreach (Tag tag in wantedTags)
        {
            Tag foundTag = tags.HasTag(tag._name);
            if(foundTag == null && enforceAllTags == true) 
            {
                return false; //missing a tag
            }
        }
        return true; //makes it through checking against all required tags
    }

    
    public bool CanGive(GameObject itm) // give as in the player recieves
    {
        if(type == DepotType.Give && itm != null) return false;
        if(type == DepotType.Recieve) return false;
        if(heldObject != null && proscessingRemaining  <= 0) return true;
        return false;
    }

    public virtual GameObject TakeItem(GameObject item) //vampires will extend off this
    {
        ItemTags tags = item.GetComponent<ItemTags>();
        if(type == DepotType.Give) return item; //shouldnt happen, but can never be to careful

        if(type == DepotType.Mix)
        {
            if(heldObject == null)
            {
                heldItemTags = tags;
                heldObject = item; 
            }
            foreach (Tag tag in tags.GetAllTags())
            {
                heldItemTags.AddTag(tag);
            }
            if(heldObject != item) Destroy(item);
            PlaceOnPedestal();
            return null;
        }
        if(type == DepotType.Alter || type == DepotType.Recieve)
        {
            GameObject toReturn = null;
            if(heldObject != null && type == DepotType.Alter) toReturn = heldObject;

            heldObject = item;
            heldItemTags = tags;
            
            PlaceOnPedestal();
            return toReturn;
        }
        PlaceOnPedestal();
        return item;
    }

    public GameObject GiveItem()
    {
        GameObject giving = heldObject;
        if(giving == null)
        {
            return giving;
        }
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

    protected void PlaceOnPedestal()
    {
        if(heldObject == null || itemDisplayPlatform == null) return;
        UnityEngine.Vector3 platPos = itemDisplayPlatform.transform.position;
        heldObject.transform.position = new UnityEngine.Vector3(
            platPos.x,
            platPos.y + heldObject.transform.lossyScale.y / 2f, 
            platPos.z);
    }

    public void ApplyTags()
    {
        if(heldObject == null || proscessingRemaining > 0) return;
        proscessingRemaining = timeToProscess;
        foreach (Tag item in giveTags)
        {
            heldItemTags.AddTag(item);
        }
    }

    

    protected virtual void Start()
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

        if(type == DepotType.Give && heldObject == null)
        {
            
            heldObject = new GameObject();
            heldItemTags = heldObject.AddComponent<ItemTags>();   
        }

        if(heldObject != null)
        {
            heldObject = Instantiate(heldObject);
            heldObject.name = "pickup";
            heldItemTags = heldObject.GetComponent<ItemTags>();
        }
        PlaceOnPedestal();
        
    }
    void Update()
    {
        if(proscessingRemaining > -1) 
        {
            proscessingRemaining -= Time.deltaTime;
        }
    }
}
