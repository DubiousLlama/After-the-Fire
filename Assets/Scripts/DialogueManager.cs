using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox1; // Reference to the first type of dialogue box
    public TMP_Text dialogueText1; // Reference to the first type of dialogue text

    public GameObject dialogueBox2; // Reference to the second type of dialogue box
    public TMP_Text dialogueText2; // Reference to the second type of dialogue text

    public GameObject dialogueSpaceBar; // Reference to the space bar indicator

    void Start()
    {
        HideDialogue();
    }

    public void ShowDialogue(string dialogue, int dialogueType)
    {
        if (dialogueType == 1)
        {
            dialogueBox1.SetActive(true);
            dialogueText1.text = dialogue;
        }
        else if (dialogueType == 2)
        {
            dialogueBox2.SetActive(true);
            dialogueText2.text = dialogue;
        }
        dialogueSpaceBar.SetActive(false);
    }

    public void ShowSpaceBar()
    {
        dialogueSpaceBar.SetActive(true);
    }

    public void HideDialogue()
    {
        dialogueBox1.SetActive(false);
        dialogueBox2.SetActive(false);
        dialogueSpaceBar.SetActive(false);
    }

    public void ShowInitialMessage(string message, int dialogueType)
    {
        ShowDialogue(message, dialogueType);
    }
}
