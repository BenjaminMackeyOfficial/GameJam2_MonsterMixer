using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    private Canvas canvas;
    void Start()
    {
        canvas = GetComponent<Canvas>();
        EventBus.RequestEvent("EscapePressed", true).ping += ToggleMenu;
    }

    private bool open;
    public void ToggleMenu()
    {
        if (ServiceHub.Instance.gameState.gameplayState == GameState.GameplayState.Menu) return; //should use the ispaused state but cant be bothered
        if(open)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            canvas.enabled = false;
            open = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            canvas.enabled =true;
            open = true;
        }
    }

}
