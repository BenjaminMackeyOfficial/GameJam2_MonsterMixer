using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

public class HudUIManager : MonoBehaviour
{
    [SerializeField] bool debugEnabled = false;

    [Header("Interact Prompt")]
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private string prompt;

    [Header("Interact Message")]
    [SerializeField] float messageDuration = 1.0f;
    [SerializeField] float fadeOutTime = 0.5f;

    [SerializeField] private TMP_Text messageText;
    private Coroutine messageFadeCoroutine;

    [Header("Dialoge")]

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private bool usingTypewriterEffect = true;

    private float typewriterSpeed = 0.01f;
    private string currentDialogueString = "";
    private Coroutine typewriterCoroutine;
    private bool isTypewriterActive = false;

    private void Awake()
    {
        DisplayMessage("");
   
        HidePrompt();

        HideDialoguePanel();
    }

    public void ShowPrompt()
    {
        promptText.text = prompt;
        ClearMessage();
    }

    public void HidePrompt()
    {
        promptText.text = "";
    }

    public void DisplayMessage(string message)
    {

    
        if (messageFadeCoroutine != null)
        {
            StopCoroutine(messageFadeCoroutine);
        }

        messageFadeCoroutine = StartCoroutine(DisplayMessageAndFade(message));
    }

      public void ClearMessage()
    {
        if (messageFadeCoroutine != null)
        {
            StopCoroutine(messageFadeCoroutine);
        }

        messageText.text = "";
    }

    private IEnumerator DisplayMessageAndFade(string message)
    {
        messageText.text = message;
        messageText.alpha = 1;

        float elapsedTime = 0f;

        yield return new WaitForSeconds(messageDuration); // Wait before fading out

        Color originalColor = messageText.color;

        while (elapsedTime < messageDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutTime);

            messageText.alpha = alpha;

            yield return null;
        }
        messageText.text = "";
    }

    public void ShowDialoguePanel()
    {
        dialoguePanel.SetActive(true);
    }

    public void HideDialoguePanel()
    {
        dialoguePanel.SetActive(false);
    }

    public void SetDialogueText(string dialogueString)
    {
        dialogueText.text = dialogueString;

        if (usingTypewriterEffect == true)
        {
            if (typewriterCoroutine != null)
            {
                StopCoroutine(typewriterCoroutine);
            }
            typewriterCoroutine = StartCoroutine(TypewriterEffect(dialogueString));

        }
        else
        {
            dialogueText.text = dialogueString;
        }

    }

    public bool IsTypewriterActive()
    {
        return isTypewriterActive;
    }

    public IEnumerator TypewriterEffect(string dialogueString)
    {
        dialogueText.text = "";
        isTypewriterActive = true;

        foreach (char letter in dialogueString.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typewriterSpeed);
        }
        isTypewriterActive = false;

    }

    public void SkipTypewriter()
    {
        if (isTypewriterActive == true)
        {

            if (typewriterCoroutine != null)
            {

                StopCoroutine(typewriterCoroutine);
            }
            dialogueText.text = currentDialogueString;
            isTypewriterActive = false;

        }
    }

}
