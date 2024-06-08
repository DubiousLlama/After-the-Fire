using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;

public class TriggerStartMusic : MonoBehaviour
{
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();

        audioManager.StartMusicFade("Pensive");
    }

}
