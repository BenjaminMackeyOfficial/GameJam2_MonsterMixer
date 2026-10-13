using UnityEngine;

public abstract class PopupBox : MonoBehaviour
{
    public GameObject attatchedObj; //what object its placing itself above
    public Canvas selfCanvasPlane; //the plane acting as a canvas
    
    public virtual void ShowCanvas(string includes, bool forceHandsCam)
    {
        
    }
    public virtual void ShowCanvas(string includes)
    {

    }
    public virtual void HideCanvas()
    {
        
    }
}


