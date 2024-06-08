using System.Collections;
using UnityEngine;
using GridHandler; // Ensure this namespace matches the one used in PuzzleGridHandler

public class InteractivePuzzle : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueMessage
    {
        public string message;
        public int dialogueType; // 1 for dialogueBox1, 2 for dialogueBox2
    }

    public DialogueMessage[] messages; // Array of dialogue messages and their types
    private int currMessageInd = 0;
    public float textSpeed = 0.05f; // Speed at which text appears
    private Transform player;

    private DialogueManager dialogueManager;
    private PuzzleGridHandler puzzleHandler; // Reference to the puzzle handler
    private bool dialogueActivated = false;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager not found in the scene.");
        }

        puzzleHandler = GetComponent<PuzzleGridHandler>();
        if (puzzleHandler == null)
        {
            Debug.LogError("PuzzleGridHandler not found on this GameObject.");
        }

        player = GameObject.Find("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
        }

        Debug.Log("InteractivePuzzle script initialized.");
    }

    void Update()
    {
        // Check if the puzzle is solved and dialogue has not been activated yet
        if (puzzleHandler.isSolved && !dialogueActivated)
        {
            dialogueActivated = true; // Prevents the dialogue from being activated more than once
            ActivateDialogue();
        }
    }

    void ActivateDialogue()
    {
        player.GetComponent<AnimatedMovement>().setDialogueState(true);
        dialogueManager.ShowDialogue("", messages[currMessageInd].dialogueType);
        Debug.Log("Dialogue started.");

        StopAllCoroutines(); // Stop any ongoing typing effect
        StartCoroutine(TypeText(messages[currMessageInd].message, messages[currMessageInd].dialogueType));
    }

    IEnumerator TypeText(string text, int dialogueType)
    {
        if (dialogueType == 1)
        {
            dialogueManager.dialogueText1.text = ""; // Clear existing text
            foreach (char letter in text.ToCharArray())
            {
                dialogueManager.dialogueText1.text += letter;
                yield return new WaitForSeconds(textSpeed); // Wait before showing the next character
            }
        }
        else if (dialogueType == 2)
        {
            dialogueManager.dialogueText2.text = ""; // Clear existing text
            foreach (char letter in text.ToCharArray())
            {
                dialogueManager.dialogueText2.text += letter;
                yield return new WaitForSeconds(textSpeed); // Wait before showing the next character
            }
        }
        
        dialogueManager.ShowSpaceBar();
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (++currMessageInd >= messages.Length)
        {
            DeactivateDialogue();
        }
        else
        {
            StartCoroutine(TypeText(messages[currMessageInd].message, messages[currMessageInd].dialogueType));
        }
    }

    void DeactivateDialogue()
    {
        dialogueManager.HideDialogue();
        currMessageInd = 0; // Reset to 0 after conversation is over.
        player.GetComponent<AnimatedMovement>().setDialogueState(false);
    }

    // Method to trigger dialogue from the PuzzleGridHandler
    public void TriggerDialogue()
    {
        ActivateDialogue();
    }
}
