using System;
using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndMenuController : MonoBehaviour
{
    [SerializeField] Image m_Image;
    [SerializeField] Image otherImage;
    [SerializeField] TextMeshProUGUI tipsScore;
    [SerializeField] TextMeshProUGUI timeScore;

    private void Start()
    {
        EventBus.RequestEvent("PlayerWon", true).ping += EndGame;
    }
    public void EndGame()
    {
        ServiceHub.Instance.gameState.AttemptEnterMenu();
        StartCoroutine(ScoreCutscene());
    }

 
    public void TotalHardReset()
    {
        Time.timeScale = 1f;
        if (Application.isEditor)
        {
            UnityEngine.Debug.Log("Editor detected: Simulating hard reset via SceneManager.");
            SceneManager.LoadScene(0);
        }
        else
        {
            try
            {
                Process.Start(Application.dataPath.Replace("_Data", ".exe"));
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError("Failed to start new process: " + e.Message);
            }
            Application.Quit();
        }
    }
    

    private IEnumerator ScoreCutscene()
    {
        tipsScore.text = "";
        timeScore.text = "";
        Color color = m_Image.color;
        color.a = 0f;
        m_Image.color = color;

        for (float t = 0; t < 5; t += Time.deltaTime)
        {
            color.a = t / 5;
            m_Image.color = color;

            yield return null;
        }

        color.a = 1f;
        m_Image.color = color;

        yield return new WaitForSecondsRealtime(1);

        tipsScore.text = "Tips: " + ServiceHub.Instance.tips.totalTips;
        yield return new WaitForSecondsRealtime(1);
        timeScore.text = "+ CompletionBonus!";
        tipsScore.text = "Tips: " + ServiceHub.Instance.tips.totalTips * 3;

        Color color2 = otherImage.color;
        color2.a = 0f;
        otherImage.color = color2;

        for (float t = 0; t < 5; t += Time.deltaTime)
        {
            color2.a = t / 5;
            otherImage.color = color2;

            yield return null;
        }

        color2.a = 1f;
        otherImage.color = color2;

        TotalHardReset();
    }
}
