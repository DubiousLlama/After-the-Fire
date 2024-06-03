using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BookInteraction : MonoBehaviour
{
    public Transform player;
    public GameObject book;
    public GameObject spaceTab;
    public float activationRadius = 5.0f; //Radius where spacteTab will apear
    public GameObject dialogueCanvas; 
    public TMP_Text dialogueText;
    public GameObject bookPageCanvas; 
    //Text info:
    string[] messages = new string[] {
        "Oh no... The book was damaged during the fire.",
        "The book contains all the seeds in our forest, it protects us in case anything goes wrong.",
        "I wonder if it retained its seeds. Let's open it and see...",
        "...",
        "Only one seed survived... The book lost most of its healing properties.",
        "I need to make more mana to find the missing seeds."
    };
    private int currMessageInd = 0;
    public float textSpeed = 0.05f; // Speed at which text appears
    //SpaceTab for dialogue:
    public GameObject dialogueSpaceBar;
    // Start is called before the first frame update
    private bool deactivateSpaceTab;
    void Start()
    {
        spaceTab.SetActive(false);
        dialogueCanvas.SetActive(false);
        dialogueSpaceBar.SetActive(false);
        bookPageCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, book.transform.position);
        if (distance <= activationRadius){
            if (!deactivateSpaceTab){
                spaceTab.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space) && !dialogueCanvas.activeSelf)
                {
                    ToggleDialogue();
                }
            }else{
                spaceTab.SetActive(false);
            }
            
        }else{
            spaceTab.SetActive(false);
            dialogueCanvas.SetActive(false);
            dialogueSpaceBar.SetActive(false);
        }
    }

    void ToggleDialogue()
    {
        dialogueCanvas.SetActive(!dialogueCanvas.activeSelf);
         if (dialogueCanvas.activeSelf)
        {
            dialogueSpaceBar.SetActive(false);

            StopAllCoroutines(); // Stop any ongoing typing effect
            StartCoroutine(TypeText(messages[currMessageInd]));
        }
    }
    IEnumerator TypeText(string text)
    {
        bookPageCanvas.SetActive(false);
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
        }else if (currMessageInd == 3){ //Shows the book page: 
            dialogueCanvas.SetActive(false);
            dialogueSpaceBar.SetActive(false);
            bookPageCanvas.SetActive(true);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        }else{
            StartCoroutine(TypeText(messages[currMessageInd]));
        }
    }
    void DeactivateDialogue(){
        dialogueCanvas.SetActive(false);
        dialogueSpaceBar.SetActive(false);
        currMessageInd = 0; // Reset to 0 after conversation is over. 
        book.SetActive(false);
        deactivateSpaceTab = true;
        
    }
     
    
}
