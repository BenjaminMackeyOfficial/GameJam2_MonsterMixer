using System;
using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
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
*/
public class GameEndMenuController : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] Image m_Image;
    [SerializeField] Image otherImage;
    [SerializeField] TextMeshProUGUI tipsScore;
    [SerializeField] TextMeshProUGUI timeScore;
    [SerializeField] RectTransform littleHand;
    [SerializeField] RectTransform bigHand;
    [SerializeField] GameObject continueText;
    private Animator animator;


    private void Start()
    {
        //EventBus.RequestEvent("PlayerWon", true).ping += EndGame;
        EventBus.RequestEvent("LevelReseting", true).ping += UnloadEndSequence;
        EventBus.RequestEvent("SpacebarPressed", true).ping += WantsToSkipSet;
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }
    public void EndGame()
    {
        ScoreCutscene();
    }

    public void CountUpScore()
    {
        animator.enabled = false;
        StartCoroutine(ScoreCountUp());
    }
    public void RotateClockHands()
    {
        spinnin = true;
        StartCoroutine(RotateClockHandCoroutine());
    }
    private bool wantsToSkip = false;
    private IEnumerator ScoreCountUp()//ticks the score up in the middle of the animation, called by an event(well, the thing that calls this is)
    {
        wantsToSkip = false;
        float finalNum = ServiceHub.Instance.tips.totalTips;
        for (int i = 0; i < finalNum; i++)
        {
            if (wantsToSkip) break;
            tipsScore.text = "Score: " + i;
            yield return null;
        }
        tipsScore.text = "Score: " + finalNum;
        continueText.SetActive(true);

        while(!wantsToSkip)
        {
            yield return null;
        }
        continueText.SetActive(false);
        ServiceHub.Instance.roundManager.ResetLevel(); // resets
    }
    private void WantsToSkipSet()
    {
        wantsToSkip = true;
    }

    private bool spinnin;
    private IEnumerator RotateClockHandCoroutine()
    {
        while(spinnin)
        {
            bigHand.rotation *= Quaternion.Euler(0, 0, -1 * Time.deltaTime);
            littleHand.rotation *= Quaternion.Euler(0, 0, -60 * Time.deltaTime);
            yield return null;
        }
    }

    private void ScoreCutscene()
    {
        spinnin = true;
        animator.enabled = true;
        animator.Play("WinScreenAnim", 0, 0f);
        canvas.gameObject.SetActive(true);
    }
    private void UnloadEndSequence()
    {
        spinnin = false;
        wantsToSkip = false;
        animator.enabled = false;
        canvas.gameObject.SetActive(false);
    }
}
