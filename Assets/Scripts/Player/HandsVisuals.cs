using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HandsVisuals : MonoBehaviour //THIS WHOLE SCRIPT IS ONLY VISUALS!!! NO IMPORTANT DATA IS HELD HERE!!!!!!
{
    [SerializeField] GameObject leftHand;
    [SerializeField] GameObject rightHand;
    [SerializeField] Image LhandImg;
    [SerializeField] Image RhandImg;

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
        EventBus.RequestEvent("GiveToLeftHand", true).ping += LeftHandGet;
        EventBus.RequestEvent("GiveToRightHand", true).ping += RightHandGet;

        EventBus.RequestEvent("LeftClickNoTarget", true).ping += InspectLeftHand;
        EventBus.RequestEvent("RightClickNoTarget", true).ping += InspectRightHand;

        EventBus.RequestEvent("LeftClickRelease", true).ping += ResetInspect;
        EventBus.RequestEvent("RightClickRelease", true).ping += ResetInspect;

        LhandBasePos = leftHand.transform.position;
        RhandBasePos = rightHand.transform.position;

        RhandTargPos = RhandBasePos;
        LHandTargPos = LhandBasePos;
    }

    private void LeftHandGet()
    {
        Destroy(holdingInLeftHand);
        GameObject obj = EventBus.RequestEvent("GiveToLeftHand", true).GetData<GameObjData>().obj;
        if(obj != null) 
        {
            holdingInLeftHand = Instantiate(obj);
            holdingInLeftHand.layer = 10; 
        }
    }
    private void RightHandGet()
    {
        Destroy(holdingInRightHand);
        GameObject obj = EventBus.RequestEvent("GiveToRightHand", true).GetData<GameObjData>().obj;
        if(obj != null) 
        {
            holdingInRightHand = Instantiate(obj);
            holdingInRightHand.layer = 10; 
        }
    }

    private void InspectRightHand()
    {
        if(holdingInRightHand == null) return;
        if(inspectingLeft || inspectingRight) return;
        inspectingRight = true;
        RhandTargPos = new Vector3(RhandBasePos.x, RhandBasePos.y + 0.3f, RhandBasePos.z - 0.1f);
        EventBus.RequestEvent("CameraZoom1", true).Invoke();
        Debug.Log(TagToStringBuilder(holdingInRightHand.GetComponent<ItemTags>().GetAllTags()));
    }

    private void InspectLeftHand()
    {
        if(holdingInLeftHand == null) return;
        if(inspectingLeft || inspectingRight) return;
        inspectingLeft = true;
        LHandTargPos = new Vector3(LhandBasePos.x, LhandBasePos.y + 0.3f, LhandBasePos.z + 0.1f);
        EventBus.RequestEvent("CameraZoom1", true).Invoke();
        Debug.Log(TagToStringBuilder(holdingInLeftHand.GetComponent<ItemTags>().GetAllTags()));
    }
    private void ResetInspect()
    {
        EventBus.RequestEvent("ResetZoom", true).Invoke();
        RhandTargPos = RhandBasePos;
        LHandTargPos = LhandBasePos;

        inspectingLeft = false; 
        inspectingRight = false;
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

        rightHand.transform.position = Vector3.Lerp(rightHand.transform.position, RhandTargPos, Time.deltaTime * 4);
        leftHand.transform.position = Vector3.Lerp(leftHand.transform.position, LHandTargPos,Time.deltaTime * 4);
    }
}
