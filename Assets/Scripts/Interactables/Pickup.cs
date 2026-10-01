using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour, Iinteractable
{
    [SerializeField] Color hoverColor;
    [SerializeField] string hoverText;
    public void Click(GameObject heldItem)
    {
        
    }
    public void Hover()
    {

    }
    public void UnHover()
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
