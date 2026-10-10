using UnityEngine;

public abstract class PopupBox : MonoBehaviour
{
    public GameObject attatchedObj; //what object its placing itself above
    public GameObject selfCanvasPlane; //the plane acting as a canvas
}


public class DialougeBox : PopupBox
{
    
}

public class NumberPopup : PopupBox
{
    
}