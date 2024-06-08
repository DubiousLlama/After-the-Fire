using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;
using UnityEngine.SceneManagement;

public class OpeningScene : MonoBehaviour
{
    AudioManager audioManager;
    public string[] musics;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;
        // Get a random music from the list
        string music = musics[Random.Range(0, musics.Length)];
        audioManager.StartMusicNow(music);
    }
}
