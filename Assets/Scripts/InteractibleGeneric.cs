using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractibleGeneric : MonoBehaviour
{
    public Transform player;
    public GameObject dialogueCanvas;
    public TMP_Text dialogueText;

    //Text info:
    public string[] messages;

    private int currMessageInd = 0;
    public float textSpeed = 0.05f; // Speed at which text appears
    // Start is called before the first frame update


    void Start()
    {
        dialogueCanvas.SetActive(false);
    }


    public void ToggleDialogue()
    {
        dialogueCanvas.SetActive(!dialogueCanvas.activeSelf);
        if (dialogueCanvas.activeSelf)
        {
            StopAllCoroutines(); // Stop any ongoing typing effect
            StartCoroutine(TypeText(messages[currMessageInd]));
        }
    }
    IEnumerator TypeText(string text)
    {
        dialogueText.text = ""; // Clear existing text
        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed); // Wait before showing the next character
        }
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (++currMessageInd >= messages.Length)
        {
            DeactivateDialogue();
        }
        else if (currMessageInd == 3)
        { //Shows the book page: 
            dialogueCanvas.SetActive(false);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        }
        else
        {
            StartCoroutine(TypeText(messages[currMessageInd]));
        }
    }
    public void DeactivateDialogue()
    {
        dialogueCanvas.SetActive(false);
        currMessageInd = 0; // Reset to 0 after conversation is over. 

    }
}
