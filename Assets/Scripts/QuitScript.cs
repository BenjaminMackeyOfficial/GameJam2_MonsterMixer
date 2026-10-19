using UnityEngine;
using UnityEngine.UI;

public class QuitScript : MonoBehaviour
{
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(QuitGame);
    }

    private void QuitGame()
    {
        Debug.Log("clicking");
        Application.Quit();
    }
}
