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

    public void LeftClick()
    {
        if(prevInteract == null) return;
        prevInteract.Click(holdingInLeftHand);
    }

    public void RightClick()
    {
        if(prevInteract == null) return;
        prevInteract.Click(holdingInRightHand);
    }
}
