using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    [SerializeField] private HudUIManager uiManager;
    [SerializeField] private PlayerController playerMovement;
    
    public bool isDialogue = false;
    private Queue<string> dialogueQueue;

    private void Awake()
    {
  
        dialogueQueue = new Queue<string>();
    }


    public void StartDialogue(string[] sentences)
    {
        isDialogue = true;
      

        uiManager.ShowDialoguePanel();

        foreach (string currentstring in sentences)
        {
            dialogueQueue.Enqueue(currentstring);
        }
        DisplayNextString();
    }

    public void DisplayNextString()
    {
        if (uiManager.IsTypewriterActive())
        {
            uiManager.SkipTypewriter();
            return;
        }

        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
        else if (dialogueQueue.Count > 0)
        {
            uiManager.SetDialogueText(dialogueQueue.Dequeue());
        }
    }

    void EndDialogue()
    {
        dialogueQueue.Clear();
        uiManager.HideDialoguePanel();

        isDialogue = false;
       
    }
}
