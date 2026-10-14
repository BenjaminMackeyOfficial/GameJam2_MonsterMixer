using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HandsVisuals : MonoBehaviour //THIS WHOLE SCRIPT IS ONLY VISUALS!!! NO IMPORTANT DATA IS HELD HERE!!!!!!
{
    [SerializeField] GameObject leftHand;
    [SerializeField] GameObject rightHand;
    [SerializeField] Image LhandImg;
    [SerializeField] Image RhandImg;
    [SerializeField] GameObject inspectUiTargetRight;
    [SerializeField] GameObject inspectUiTargetLeft;

    private SpriteRenderer LHandRenderer;
    private SpriteRenderer RhandRenderer;

    private GameObject holdingInLeftHand;
    private GameObject holdingInRightHand;

    private bool inspectingRight;
    private bool inspectingLeft;

    private Vector3 LhandBasePos;
    private Vector3 RhandBasePos;

    private Vector3 RhandTargPos;
    private Vector3 LHandTargPos;

    void Awake()
    {
        EventBus.RequestEvent("GiveToLeftHand", true).ping += lHandGet;
        EventBus.RequestEvent("GiveToRightHand", true).ping += RHandGet;

        EventBus.RequestEvent("LeftClickNoTarget", true).ping += InspectLeftHand;
        EventBus.RequestEvent("RightClickNoTarget", true).ping += InspectRightHand;

        EventBus.RequestEvent("LeftClickRelease", true).ping += ResetInspect;
        EventBus.RequestEvent("RightClickRelease", true).ping += ResetInspect;

        EventBus.RequestEvent("ItemHovered", true).ping += Hovering;
        EventBus.RequestEvent("ItemUnHovered", true).ping += Unhovering;

        LhandBasePos = leftHand.transform.position;
        RhandBasePos = rightHand.transform.position;

        RhandTargPos = RhandBasePos;
        LHandTargPos = LhandBasePos;

        LHandRenderer = leftHand.GetComponent<SpriteRenderer>();
        RhandRenderer = rightHand.GetComponent<SpriteRenderer>();
    }

    private bool hovering;
    private void Hovering()
    {
        hovering = true;
        if (inspectingLeft || inspectingRight) return;
        EventBus.RequestEvent("HandsCameraZoom1", true).Invoke();


    }
    private void Unhovering()
    {
        hovering = false;
        if (inspectingLeft || inspectingRight) return;
        EventBus.RequestEvent("HandsResetZoom", true).Invoke();
    }

    private void lHandGet()
    {
        StartCoroutine(LeftHandGet());
    }
    private IEnumerator LeftHandGet()
    {
        LHandTargPos -= new Vector3(2, 0, 0);
        yield return new WaitForSeconds(0.2f);
        Destroy(holdingInLeftHand);
        GameObject obj = EventBus.RequestEvent("GiveToLeftHand", true).GetData<GameObjData>().obj;
        if(obj != null) 
        {
            LHandRenderer.enabled = false;
            holdingInLeftHand = Instantiate(obj);
            holdingInLeftHand.layer = 10;
            holdingInLeftHand.transform.LookAt(ServiceHub.Instance.cameras.overlayCamera.transform);
            holdingInLeftHand.transform.localScale *= 4;
        }
        else
        {
            LHandRenderer.enabled = true;
        }
        LHandTargPos = LhandBasePos;
    }
    private void RHandGet()
    {
        StartCoroutine(RightHandGet());
    }
    private IEnumerator RightHandGet()
    {
        RhandTargPos -= new Vector3(2,0,0);
        yield return new WaitForSeconds(0.2f);
        Destroy(holdingInRightHand);
        GameObject obj = EventBus.RequestEvent("GiveToRightHand", true).GetData<GameObjData>().obj;
        if(obj != null) 
        {
            RhandRenderer.enabled = false;
            holdingInRightHand = Instantiate(obj);
            holdingInRightHand.layer = 10;
            holdingInRightHand.transform.LookAt(ServiceHub.Instance.cameras.overlayCamera.transform);
            holdingInRightHand.transform.localScale *= 4;
        }
        else
        {
            RhandRenderer.enabled = true;
        }
        RhandTargPos = RhandBasePos;
    }

    private void InspectRightHand()
    {
        if(holdingInRightHand == null) return;
        if(inspectingLeft || inspectingRight) return;
        inspectingRight = true;
        RhandTargPos = new Vector3(RhandBasePos.x, RhandBasePos.y + 0.3f, RhandBasePos.z - 0.1f);
        EventBus.RequestEvent("CameraZoom1", true).Invoke();
        EventBus.RequestEvent("HandsCameraZoom1", true).Invoke();

        EventBus.RequestEvent("CancelRightHandInspectDialouge", true).Invoke(); //TEMP DO NOT DO THIS EVER THIS IS LAST MINUTE FIXES

        string toSay = "/I Okay.. this is: " + TagToStringBuilder(holdingInRightHand.GetComponent<ItemTags>().GetAllTags());
        ServiceHub.Instance.popupManager.CreateDialouge(toSay, inspectUiTargetRight, true);
    }

    private void InspectLeftHand()
    {
        if(holdingInLeftHand == null) return;
        if(inspectingLeft || inspectingRight) return;
        inspectingLeft = true;
        LHandTargPos = new Vector3(LhandBasePos.x, LhandBasePos.y + 0.3f, LhandBasePos.z + 0.1f);
        EventBus.RequestEvent("CameraZoom1", true).Invoke();
        EventBus.RequestEvent("HandsCameraZoom1", true).Invoke();

        string toSay = "/I Okay.. this is: " + TagToStringBuilder(holdingInLeftHand.GetComponent<ItemTags>().GetAllTags());
        ServiceHub.Instance.popupManager.CreateDialouge(toSay, inspectUiTargetLeft, true);
 
    }
    private void ResetInspect()
    {
        EventBus.RequestEvent("ResetZoom", true).Invoke();
        if(!hovering) EventBus.RequestEvent("HandsResetZoom", true).Invoke();
        RhandTargPos = RhandBasePos;
        LHandTargPos = LhandBasePos;

        inspectingLeft = false; 
        inspectingRight = false;

        EventBus.RequestEvent("CancelLeftHandInspectDialouge", true).Invoke();//TEMP DO NOT DO THIS EVER THIS IS LAST MINUTE FIXES
        EventBus.RequestEvent("CancelRightHandInspectDialouge", true).Invoke(); //TEMP DO NOT DO THIS EVER THIS IS LAST MINUTE FIXES
    }

    private string TagToStringBuilder(Tag[] tags)//temp
    {
        string total = "";
        foreach (Tag item in tags)
        {
            total = total + ", " + item._name;
        }
        return total;
    }
    void Update()
    {
        if(holdingInLeftHand != null ) holdingInLeftHand.transform.position = leftHand.transform.parent.position + leftHand.transform.localPosition;
        if(holdingInRightHand != null) holdingInRightHand.transform.position = rightHand.transform.parent.position + rightHand.transform.localPosition;

        rightHand.transform.position = Vector3.Lerp(rightHand.transform.position, RhandTargPos, Time.deltaTime * 20);
        leftHand.transform.position = Vector3.Lerp(leftHand.transform.position, LHandTargPos,Time.deltaTime * 20);
    }
}
