
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
