
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
/*  Removing cause this was all very quick shitty spaghetti code
public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject Sign;
    [SerializeField] GameObject notepad;
    [SerializeField] Image panel;
    void Start()
    {
        EventBus.RequestEvent("StartGame", true).ping += CloseMainMenu;
        signUp = Sign.transform.position + Vector3.up * 5;
        notepadDown = notepad.transform.position - Vector3.up * 5;
        LoadMainMenu();
    }
    public void LoadMainMenu()
    {
        StartCoroutine(FadeIn());
    }
    public IEnumerator FadeIn()
    {
        Color color = panel.color;
        Debug.Log(panel.color.a);
        color.a = 1f;
        panel.color = color;

        for (float t = 0; t < 5000; t += Time.deltaTime)
        {
            color.a = t / 1000;
            panel.color = color;

            yield return null;
        }

        color.a = 0f;
        panel.color = color;
    }
    public void CloseMainMenu()
    {
        ServiceHub.Instance.gameState.AttemptEnterPlay();
        closing = true;
        
    }
    private bool closing = false;
    private Vector3 signUp;
    private Vector3 notepadDown;
    private float destroyIn = 0;
    private void Update()
    {
        if(closing)
        {
            destroyIn += Time.deltaTime;
            Sign.transform.position = Vector3.Lerp(Sign.transform.position, signUp, Time.deltaTime);
            notepad.transform.position = Vector3.Lerp(notepad.transform.position, notepadDown, Time.deltaTime);
            if (destroyIn > 4) gameObject.SetActive(false);
        }
    }
}
*/

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject Sign;
    [SerializeField] GameObject notepad;
    [SerializeField] Canvas canvas;

    [SerializeField] Animator animator;

    private Vector3 signStartPos;
    private Vector3 notepadStartPos;

    public float introCutsceneLength;
    void Start()
    {
        introCutsceneLength = animator.GetCurrentAnimatorStateInfo(0).length;
        signStartPos = Sign.transform.position;
        notepadStartPos = notepad.transform.position;
        EventBus.RequestEvent("LevelReseting", true).ping += RunStartSequenceNoTitle;
    }
    public void LoadMainMenu()
    {
        notepad.transform.position = notepadStartPos;
        Sign.transform.position = signStartPos;
        RunStartSequence();
    }
    public void HideMainMenu()
    {
        StartCoroutine(HideMainMenuAssets());
    }
    public void RunStartSequence()
    {
        canvas.enabled = true;
        animator.enabled = true;
    }
    public void RunStartSequenceNoTitle()
    {
        notepad.transform.position = notepadStartPos;
        Sign.transform.position = signStartPos;

        animator.enabled = true;
        canvas.enabled = true;
        float targTime = animator.runtimeAnimatorController.animationClips[0].events[0].time / animator.runtimeAnimatorController.animationClips[0].length;
        animator.Play("InitialOpenCutscene", 0, targTime);
    }
    public void StopStartSequence()
    {
        animator.enabled = false;
        canvas.enabled = false;
        ServiceHub.Instance.roundManager.CutsceneOverBeginOtherSystems();
    }
    private IEnumerator HideMainMenuAssets()
    {
        Vector3 signUp = Sign.transform.position + Vector3.up * 7;
        Vector3 notepadDown = notepad.transform.position + Vector3.down * 4;
        for (float i = 0; i < 5; i += Time.deltaTime)
        {
            Sign.transform.position = Vector3.MoveTowards(Sign.transform.position, signUp, Time.deltaTime);
            notepad.transform.position = Vector3.MoveTowards(notepad.transform.position, notepadDown, Time.deltaTime);
            yield return null;
        }
    }
    private void Nothing() { } //so timeline events can do nothing and not error
}
