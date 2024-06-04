using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPrinter : MonoBehaviour
{
    string textToType = "This is the line to print.";
    public float delay = 0.1f;
    TMP_Text dialogueTextMesh;
    void Awake(){
        dialogueTextMesh = GetComponent<TMP_Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TypeTextCO());
    }
    IEnumerator TypeTextCO(){
        dialogueTextMesh.text = string.Empty;
        for (int i=0; i< textToType.Length; i++){
            Debug.Log("index:" + i);
            Debug.Log("letter:" + textToType[i]);
            Debug.Log(dialogueTextMesh.text);
            dialogueTextMesh.text += textToType[i];
            Debug.Log(dialogueTextMesh.text);
            yield return new WaitForSeconds(delay);
        }
        Debug.Log("exited");
        yield return null;
    }

    
}
