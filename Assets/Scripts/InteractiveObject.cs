using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GridHandler;

public class InteractiveObject : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueMessage
    {
        public string message;
        public int dialogueType; // 1 for dialogueBox1, 2 for dialogueBox2
    }

    public Transform player;
    public GameObject interactiveObject;
    public Image E_key; // Change from GameObject to Image
    public float activationRadius = 5.0f;
    public DialogueMessage[] messages; // Array of dialogue messages and their types
    private int currMessageInd = 0;
    public float textSpeed = 0.05f; // Speed at which text appears
    public GameObject dialogueSpaceBar; // Space bar indicator for dialogue
    public PuzzleGridHandler puzzleHandler; // Reference to the puzzle handler
    public bool disableInteractionAfterPuzzleSolved = false; // Flag to disable interaction after puzzle is solved

    private DialogueManager dialogueManager;

    void Start()
    {
        E_key.gameObject.SetActive(false); // Set the Image's GameObject inactive
        dialogueSpaceBar.SetActive(false);

        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager not found in the scene.");
        }

        Debug.Log("InteractiveObject script initialized.");
    }

    void Update()
    {
        if (disableInteractionAfterPuzzleSolved && puzzleHandler != null && puzzleHandler.isSolved)
        {
            if (E_key.gameObject.activeSelf)
            {
                E_key.gameObject.SetActive(false);
            }
            return; // Skip interaction if the puzzle is solved
        }

        Vector3 playerPosition2D = new Vector3(player.position.x, player.position.y, 0);
        Vector3 interactiveObjectPosition2D = new Vector3(interactiveObject.transform.position.x, interactiveObject.transform.position.y, 0);
        float distance = Vector3.Distance(playerPosition2D, interactiveObjectPosition2D);

        // Debug.Log("Distance to player: " + distance);
        // Debug.Log("Player position: " + player.position);
        // Debug.Log("Interactive Object position: " + interactiveObject.transform.position);

        if (distance <= activationRadius)
        {
            if (!E_key.gameObject.activeSelf)
            {
                E_key.gameObject.SetActive(true);
                Debug.Log("E_key activated.");
            }
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(interactiveObject.transform.position + Vector3.up * 1f); // Adjust the offset as needed
            E_key.transform.position = screenPosition;

            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleDialogue();
            }
        }
        else
        {
            if (E_key.gameObject.activeSelf)
            {
                E_key.gameObject.SetActive(false);
            }
        }
    }

    void ToggleDialogue()
    {
        if (!dialogueManager.dialogueBox1.activeSelf && !dialogueManager.dialogueBox2.activeSelf)
        {
            player.GetComponent<AnimatedMovement>().setDialogueState(true);
            dialogueManager.ShowDialogue("", messages[currMessageInd].dialogueType);
            Debug.Log("Dialogue started.");

            StopAllCoroutines(); // Stop any ongoing typing effect
            StartCoroutine(TypeText(messages[currMessageInd].message, messages[currMessageInd].dialogueType));
        }
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
        dialogueSpaceBar.SetActive(false);
        currMessageInd = 0; // Reset to 0 after conversation is over.
        player.GetComponent<AnimatedMovement>().setDialogueState(false);
    }

    public void TriggerDialogue(DialogueMessage[] newMessages)
    {
        messages = newMessages;
        currMessageInd = 0;
        ToggleDialogue();
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position to visualize the activation radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactiveObject.transform.position, activationRadius);
    }
}
