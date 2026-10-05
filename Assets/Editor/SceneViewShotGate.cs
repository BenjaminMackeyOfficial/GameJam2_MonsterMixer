using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class SceneViewShotGate
{
    private static bool isGateEnabled = true;
    private static readonly Color gateColor = new Color(1f, 1f, 0f, 0.4f); // Semi-transparent yellow

    static SceneViewShotGate()
    {
    
        SceneView.duringSceneGui -= OnSceneGUI;  // adds option intop the gui
        SceneView.duringSceneGui += OnSceneGUI; // adds option intop the gui
    }

    [MenuItem("Tools/Toggle 16:9 Shot Gate")]
    public static void ToggleGate()
    {
        isGateEnabled = !isGateEnabled;
        SceneView.RepaintAll();
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        if (!isGateEnabled) return;


        float windowWidth = sceneView.position.width;// gets current screen dimnensions
        float windowHeight = sceneView.position.height;// gets current screen dimnensions


        float targetWidth = windowWidth;   // calculates 16: 9 width for shot  gate
        float targetHeight = windowWidth * (9f / 16f); // calculates 16: 9 width for shot  gate

        if (targetHeight > windowHeight)
        {
            targetHeight = windowHeight;
            targetWidth = windowHeight * (16f / 9f);
        }

       
        float xOffset = (windowWidth - targetWidth) / 2f;//centers  the shot box
        float yOffset = (windowHeight - targetHeight) / 2f;//centers  the shot box

        Rect gateRect = new Rect(xOffset, yOffset, targetWidth, targetHeight);

      
        Handles.BeginGUI();// draw the shot gate on scen window

        Handles.DrawSolidRectangleWithOutline(gateRect, Color.clear, gateColor); // draws framioing

  
        GUI.skin.label.normal.textColor = gateColor;  //lable to confirm  gate
        GUI.Label(new Rect(xOffset + 10, yOffset + 10, 100, 20), "16:9 Gate"); //lable to confirm  gate

        Handles.EndGUI();
    }
}