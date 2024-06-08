using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartDialogue : MonoBehaviour
{
    public GameObject StartDialogueCanvas;
    public GameObject player;
    public TMP_Text dialogueText;
    public float textSpeed = 0.005f; // Speed at which text appears
    public GameObject dialogueSpaceBar; //SpaceTab for dialogue:
    string[] messages = new string[] {
        "… how long has it been?  is the fire out?",
        "I can’t feel the other spirits. I can’t feel any magic. Even the Book feels quiet.",
        "Maybe I can approach it now."
        
    };
    private int currMessageInd = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Activate the dialogue box:
        StartDialogueCanvas.SetActive(true);
        //Stop player from moving: 
        AnimatedMovement animatedMovementScript = player.GetComponent<AnimatedMovement>();
        animatedMovementScript.setDialogueState(true); 
        ToggleDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        // if (!StartDialogueCanvas.activeSelf){
        //     ToggleDialogue();
        // }
        
    }
    void ToggleDialogue()
    {
        Debug.Log("We Talking");
        // player.GetComponent<AnimatedMovement>().setDialogueState(true);
        // StartDialogueCanvas.SetActive(!StartDialogueCanvas.activeSelf);
        if (StartDialogueCanvas.activeSelf)
        {
            dialogueSpaceBar.SetActive(false);
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
        dialogueSpaceBar.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        
        if (++currMessageInd >= messages.Length) {
            DeactivateDialogue();
        }else{
            StartCoroutine(TypeText(messages[currMessageInd]));
        }
    }
    void DeactivateDialogue(){
        StartDialogueCanvas.SetActive(false);
        dialogueSpaceBar.SetActive(false);
        currMessageInd = 0; // Reset to 0 after conversation is over. 
        player.GetComponent<AnimatedMovement>().setDialogueState(false);
    }
}
