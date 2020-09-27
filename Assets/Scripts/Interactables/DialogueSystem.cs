using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public Button ContinueButton;
    public Text NameText, DialogueText;
    private static DialogueSystem instance = null;
    List<string> dialogueLines;
    int dialogueIndex;

    public static DialogueSystem Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        gameObject.SetActive(false);
    }

    public void AddNewDialogue(string name, string[] lines)
    {
        NameText.text = name;
        dialogueLines = new List<string>(lines);
        dialogueIndex = -1;
        gameObject.SetActive(true);

        ContinueDialogue();
    }

    public void ContinueDialogue()
    {
        if (++dialogueIndex < dialogueLines.Count)
            DialogueText.text = dialogueLines[dialogueIndex];
        else
            gameObject.SetActive(false);
    }
}