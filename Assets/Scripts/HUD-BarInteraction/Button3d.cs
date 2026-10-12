using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

#region coder & project
/// <summary>
/// NSCC GAME2065/4087/Game Development III(B)/Cameron,Jordan
/// Jam 2 :Monster Mixer
/// team: Chris French, Benjamin Mackey, Arianna Cutler
/// Coder current script: Chris French Second Year NSCC Game Programming 
/// Additions / annotations:
/// code review: 
/// </summary>
#endregion

public class Button3d : MonoBehaviour
{
     private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) //&& !(gameObject.CompareTag("UIClickable")))
        {
            return;
        }
        _renderer.material.color = Highlight;//Color.green;// verifies the collider is working and picking up the mouse ratycast
    }

    private void OnMouseExit()
    {
        _renderer.material.color = _originalColor; // switches back to default 
    }

    public void OnMouseDown()
    {
        if (gameObject.CompareTag("UIClickable") || EventSystem.current.IsPointerOverGameObject())// prevents click if over a non tagged fgame object or ui clickable tagged object  to prevent multi clicking 
        {
            return;
        }

        if (gameObject.CompareTag("interactive"))
        {
            ExecuteCamSwitch();
        }
    }
}
