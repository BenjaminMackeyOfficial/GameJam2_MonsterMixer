using UnityEngine;
using UnityEngine.UI;

public class resumeScript : MonoBehaviour
{
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ResumeGame);
    }

    private void ResumeGame()
    {
        Debug.Log("clicking");
        transform.parent.parent.GetComponent<PauseMenuController>().ToggleMenu();
    }
}
