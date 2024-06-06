using Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusicTutorial : MonoBehaviour
{
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;
        audioManager.StartMusicFade("Pensive");
    }
}
