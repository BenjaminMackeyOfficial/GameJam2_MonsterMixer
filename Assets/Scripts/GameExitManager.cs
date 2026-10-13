using System.Collections;
using TMPro;
using UnityEngine;
#region coder & project
/// <summary>
/// NSCC GAME2025 / 4086 / Game Programming III(B)/ Doucette,Matthew
/// Unity: Game Manager & Persistence
/// Coder current script: Chris French Second Year NSCC Game Programming 
/// Additions / annotations:
/// 
/// </summary>
#endregion
public class GameExitManager : MonoBehaviour
{
    public static GameExitManager Instance { get; private set; }
    public float ExitDelay = 5f;
    public TextMeshProUGUI exitCountdownText;
    private float Countdown;
    private bool isExiting = false;
     void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // keeps script accessable accross scene changes
        ResetCountdown();                    
    }
    private void Update()
    {
        if (exitCountdownText == null)   {   exitCountdownText = GameObject.Find("exitCountdownText")?.GetComponent<TextMeshProUGUI>();   }
    }
    public void Ongameexit()
    {
        if (!isExiting)
        {
            isExiting = true;
            StartCoroutine(StartExitCountdown());
        }
    }
    public IEnumerator StartExitCountdown()
    {
        while (Countdown > 0)
        {
            exitCountdownText.text = "Game Exit in: " + Mathf.Ceil(Countdown).ToString();// displays the countdown output in an always rounded up to whole int
            yield return null;
            Countdown -= Time.deltaTime;
        }
        exitCountdownText.text = "Exiting...";
        ExitGame();
    }
    private void ResetCountdown()
    {
        Countdown = ExitDelay;
        if (exitCountdownText != null)
        {
            exitCountdownText.text = "";
        }
    }
    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
         // Works in a built application
        Application.Quit();
         // Works inside the Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}