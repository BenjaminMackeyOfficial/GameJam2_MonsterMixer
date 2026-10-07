using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{

    private GameObject prevHovered;
    private Iinteractable prevInteract;
    [SerializeField] Transform raycastFrom;
    [SerializeField] float maxHoverDist;

    //events
    private CustomEvent rightHandClick;
    private CustomEvent leftHandClick;
    //

    //
    private GameObject holdingInLeftHand;
    private GameObject holdingInRightHand;
    //

    //
    private PlayerController controller;
    //

    //
    private Vector3 itemPoolPosition = new Vector3(0,-900,0);
    //
    void Start()
    {
        //data are whatever the player previously interacted with
        // for example, if the player picked up a lemon, it would be lemon, if they placed lemon in blender, it would be the blender

        //right hand click event
        rightHandClick = EventBus.RequestEvent("RightHandClick", true);

        GameObjData dat = new GameObjData();
        rightHandClick.AddData(dat);

        //left hand click event
        leftHandClick = EventBus.RequestEvent("LeftHandClick", true);

        GameObjData dat2 = new GameObjData();
        leftHandClick.AddData(dat2);

        //

        EventBus.RequestEvent("GiveToLeftHand", true).ping += LeftHandRecieve;
        EventBus.RequestEvent("GiveToRightHand", true).ping += RightHandRecieve;
    }

    void Update()
    {
        RaycastHit hit;
        Physics.Raycast(raycastFrom.position, raycastFrom.forward, out hit, maxHoverDist);
        if(hit.collider != null && hit.collider.gameObject != prevHovered)
        {
            if(prevInteract != null) prevInteract.UnHover();
            prevInteract = null;
            prevHovered = null;

            if(hit.collider.gameObject.GetComponent<Iinteractable>() == null) return;

            prevHovered = hit.collider.gameObject;
            prevInteract = prevHovered.GetComponent<Iinteractable>();

            prevInteract.Hover();
        }
        
    }
    private void LeftHandRecieve()
    {

        holdingInLeftHand = EventBus.RequestEvent("GiveToLeftHand", true).GetData<GameObjData>().obj;
        EventBus.RequestEvent("UpdateHands", true).Invoke();
        if(holdingInLeftHand != null) holdingInLeftHand.transform.position = itemPoolPosition; 
    }
    private void RightHandRecieve()
    {
        holdingInRightHand = EventBus.RequestEvent("GiveToRightHand", true).GetData<GameObjData>().obj;
        EventBus.RequestEvent("UpdateHands", true).Invoke();
        if(holdingInRightHand != null) holdingInRightHand.transform.position = itemPoolPosition;
    }

    public void LeftClick()
    {
        if(prevInteract == null)
        {
            EventBus.RequestEvent("LeftClickNoTarget", true).Invoke();
            return;
        }
        prevInteract.Click(holdingInLeftHand, true);
    }
    public void LeftUnClick()
    {
        EventBus.RequestEvent("LeftClickRelease", true).Invoke();
    }
    public void RightClick()
    {
        if(prevInteract == null)
        {
            EventBus.RequestEvent("RightClickNoTarget", true).Invoke();
            return;
        }
        prevInteract.Click(holdingInRightHand, false);
    }
    public void RightUnClick()
    {
        EventBus.RequestEvent("RightClickRelease", true).Invoke();
    }
}
