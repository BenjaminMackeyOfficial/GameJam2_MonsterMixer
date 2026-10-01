using UnityEngine;

public class PlayerGeneral : MonoBehaviour
{
    private PlayerController controller;
    private PlayerStateManager stateManager;

    private Color green;
    private Color blue;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        stateManager = GetComponent<PlayerStateManager>();

        green = new Color(0, 255, 0, 195) / 255f;
        blue = new Color(0, 0, 255, 195) / 255f;
    }
}
