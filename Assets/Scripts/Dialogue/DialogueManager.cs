using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance {get; private set;}

    [Header("Dialogue Components")]
    public GameObject dialoguePanel;
    public Text speakerText;
    public Text dialogueText;

    //The lines to queue during the dialogue sequence
    Queue<DialogueLine> dialogueQueue;
    Action onDialogueEnd = null;

    bool isTyping = false;

    private void Awake()
    {
        //If there is more than 1 instance, destroy the extra
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            //Set the static instance to this instance
            Instance = this;
        }
    }

    //Instantiate the dialogue
    public void StartDialogue(List<DialogueLine> dialogueLinesToQueue)
    {
        //Convert the list to a queue
        dialogueQueue = new Queue<DialogueLine>(dialogueLinesToQueue);

        UpdateDialogue();
    }

    //Execute action after finishing initialise the dialogue
    public void StartDialogue(List<DialogueLine> dialogueLinesToQueue, Action onDialogueEnd)
    {
        StartDialogue(dialogueLinesToQueue);
        this.onDialogueEnd = onDialogueEnd;
    }

    //Cycle through the dialogue lines
    public void UpdateDialogue()
    {
        if (isTyping)
        {
            isTyping = false;
            return;
        }

        //Reset dialogue
        dialogueText.text = string.Empty;
        
        //Check if there are any more lines in queue. If not, end dialouge
        if(dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        //The current dialogue line to put in
        DialogueLine line = dialogueQueue.Dequeue();

        Talk(line.speaker, line.message);
    }

    //Close the dialogue
    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        //Invoke whatever Action queued on dialogue end (if any)
        onDialogueEnd?.Invoke();

        //Reset the action
        onDialogueEnd = null;
    }

    public void Talk(string speaker, string message)
    {
        //Set the dialogue panel active
        dialoguePanel.SetActive(true);

        //Set the speaker text to the speaker
        speakerText.text = speaker;

        //If there's no speaker, don't show the speaker text panel
        speakerText.transform.parent.gameObject.SetActive(speaker != "");

        //Set the dialogue text to the message
        //dialogueText.text = message;
        StartCoroutine(TypeText(message));
    }

    IEnumerator TypeText(string textToType)
    {
        isTyping = true;

        //Convert string tio an array of chars
        char[] charsToType = textToType.ToCharArray();

        for(int i = 0;  i < charsToType.Length; i++)
        {
            dialogueText.text += charsToType[i];
            yield return new WaitForEndOfFrame();

            //Skip the typing sequence and show full text
            if (!isTyping)
            {
                dialogueText.text = textToType;
                break;
            }
        }

        //Typing sequence complete
        isTyping = false;
    }
}
