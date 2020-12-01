using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private Button ContinueButton;
    [SerializeField] private Text NameText, DialogueText;

    private List<string> dialogueLines;
    private int dialogueIndex;

    public static DialogueSystem Instance { get; private set; } = null;

    //Initializes singleton and hides dialogue box
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);

        Instance = this;
        gameObject.SetActive(false);
    }

    //Adds new dialogue
    public void AddNewDialogue(string name, string[] lines)
    {
        NameText.text = name;
        dialogueLines = new List<string>(lines);
        dialogueIndex = -1;
        gameObject.SetActive(true);

        ContinueDialogue();
    }

    //Continues displaying dialogue
    public void ContinueDialogue()
    {
        if (++dialogueIndex < dialogueLines.Count)
            DialogueText.text = dialogueLines[dialogueIndex];
        else
            gameObject.SetActive(false);
    }
}