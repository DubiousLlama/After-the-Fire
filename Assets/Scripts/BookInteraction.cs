using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BookInteraction : MonoBehaviour
{
    public Transform player;
    public GameObject book;
    public GameObject E_key;
    public float activationRadius = 5.0f; //Radius where spacteTab will apear
    public GameObject dialogueCanvas;
    public GameObject bookDialogueCanvas; // Canvas for book dialogue when currMessageInd == 2
    public TMP_Text dialogueText;
    public TMP_Text bookDialogueText;
    public GameObject bookPageCanvas; 
    //Text info:
    string[] messages = new string[] {
        "The Book must have used all its magic to save us. I should bring it into the village. Maybe someone can restore it.",
        "The pages are tattered and unreadable. There’s only some text legible:",
        "Life Begets Life. As plants use their magic to cast spells, they will gift the spirit who planted them with more seeds to plant to continue their spread.",
        "Tucked behind this page is a single seed to cast a simple spell in times of struggle.",
        "...",
        "So using this seed for a spell by planting it should give me more seeds I can use to get out of here. I should be able to plant it right under this stump.",
        
    };
    private int currMessageInd = 0;
    public float textSpeed = 0.05f; // Speed at which text appears
    public GameObject dialogueSpaceBar; //SpaceTab for dialogue:
    private bool deactivateEkey; //Deactivates E key at the end of dialogue
    public GameObject removeInstructionsCanvas; //canvas for "press R to remove stump"
    public GameObject plantInstructionsCanvas;//canvas for "move to the tile and press space to plant"
    private bool hideRemoveInstructions;
    public GameObject stump;
    private float fadeDuration = 2.0f;
    // Start is called before the first frame update
    
    
    void Start()
    {
        E_key.SetActive(false);
        
        dialogueCanvas.SetActive(false);
        bookDialogueCanvas.SetActive(false);
        dialogueSpaceBar.SetActive(false);
        bookPageCanvas.SetActive(false);
        removeInstructionsCanvas.SetActive(false);
        plantInstructionsCanvas.SetActive(false);
      
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.position, book.transform.position);
        if (distance <= activationRadius){
            if (!deactivateEkey){
                E_key.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E) && !dialogueCanvas.activeSelf)
                {
                    ToggleDialogue();
                }
            }else{
                E_key.SetActive(false);
                if (!hideRemoveInstructions){
                    removeInstructionsCanvas.SetActive(true);
                }else{
                    removeInstructionsCanvas.SetActive(false);
                }
                RemoveStump();
               
            }
        }else{
            E_key.SetActive(false);
            dialogueCanvas.SetActive(false);
            dialogueSpaceBar.SetActive(false);
        }
    }

    void ToggleDialogue()
    {
        player.GetComponent<AnimatedMovement>().setDialogueState(true);
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
        dialogueText.text = "";
        bookDialogueText.text = ""; // Clear existing text
        if (currMessageInd == 2 || currMessageInd == 3){
            dialogueCanvas.SetActive(false);
            bookDialogueCanvas.SetActive(true);
        }else{
            dialogueCanvas.SetActive(true);
            bookDialogueCanvas.SetActive(false);
        }
        foreach (char letter in text.ToCharArray())
        {
            
            if (currMessageInd == 2 || currMessageInd == 3){//for book dialogue:
                bookDialogueText.text += letter;
            }else{
                dialogueText.text += letter;
            }
            
            
            yield return new WaitForSeconds(textSpeed); // Wait before showing the next character
        }
        dialogueSpaceBar.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        
        if (++currMessageInd >= messages.Length) {
            DeactivateDialogue();
        
        }else if (currMessageInd == 4){ //Shows the book page: 
            dialogueCanvas.SetActive(false);
            bookDialogueCanvas.SetActive(false);
            dialogueSpaceBar.SetActive(false);
            bookPageCanvas.SetActive(true);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        }else{
            StartCoroutine(TypeText(messages[currMessageInd]));
        }
    }
    void DeactivateDialogue(){
        dialogueCanvas.SetActive(false);
        dialogueSpaceBar.SetActive(false);
        currMessageInd = 0; // Reset to 0 after conversation is over. 
        book.SetActive(false);
        deactivateEkey = true;
        player.GetComponent<AnimatedMovement>().setDialogueState(false);
    }
    void RemoveStump(){
        if (Input.GetKeyDown(KeyCode.R)){
            Debug.Log("R pressed, begin removal");
            hideRemoveInstructions = true;
            StartCoroutine(FadeOut(stump, fadeDuration, () => {plantInstructionsCanvas.SetActive(true);}));

        }
    }
    IEnumerator FadeOut(GameObject obj, float duration, System.Action onComplete = null){
        if (obj != null){
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null){
                Material mat = renderer.material;
                Color initialColor = mat.color;
                float counter = 0;
                while (counter < duration){
                    float alpha = Mathf.Lerp(1.0f, 0.0f, counter / duration); //Transparency
                    mat.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
                    counter += Time.deltaTime;
                    yield return null;
                }
                mat.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0); //becomes transparent
                obj.SetActive(false);
                if (onComplete != null) onComplete(); //once stump fades out, show instructions to start planting.
            }
        }
    }
     
    
}
