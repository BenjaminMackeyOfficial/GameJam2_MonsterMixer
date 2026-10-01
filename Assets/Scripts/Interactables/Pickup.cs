using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour, Iinteractable
{
    [SerializeField] Color hoverColor;
    [SerializeField] string hoverText;
    public void Click()
    {
        
    }
    public void HighlightHover()
    {

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
        

    }
}
