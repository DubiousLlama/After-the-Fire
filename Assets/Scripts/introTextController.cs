using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Audio;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class introTextController : MonoBehaviour
{
    [Header("Config")]
    public GameObject introTextField;
    public bool typeTextAnimation = false;

    public introTextObj[] introText;
    public introSoundObj[] introSounds;
    
    AudioManager audioManager;
    float currentTime = 0;
    introTextObj currentText;
    introSoundObj currentSound = null;
    bool fadingOut = false;


    void Start()
    {
        currentTime = 0;
        audioManager = FindObjectOfType<AudioManager>();
        currentText = null;

    }

    // Update is called once per frame
    void Update()
    {
        introTextObj newText = GetIntroText();

        if (newText != null)
        {
            if (newText.isEnd)
            {
                AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
                foreach (AudioSource audioS in allAudioSources)
                {
                    audioS.Stop();
                }
                SceneManager.LoadScene("Tutorial");
                // Get all aduiosources and stop them
                introTextField.GetComponent<TextMeshProUGUI>().text = "";
                return;
            }
            else if (currentText != newText && !typeTextAnimation)
            {
                introTextField.GetComponent<TextMeshProUGUI>().text = newText.text;
            }

            if (typeTextAnimation)
            {
                introTextField.GetComponent<TextMeshProUGUI>().text = newText.text.Substring(0, Mathf.Min((int)((currentTime - newText.time)*15f), newText.text.Length));
            }

            currentText = newText;
        }

        introSoundObj newSound = GetIntroSound();

        if (newSound != null && newSound != currentSound)
        {
            currentSound = newSound;

            if (newSound.soundType == introSoundType.music)
            {
                audioManager.StartMusicNow(newSound.sound);
                fadingOut = false;
            }
            else if (newSound.soundType == introSoundType.ambiance)
            {
                audioManager.StartAmbiance(newSound.sound);
            }
            else if (newSound.soundType == introSoundType.sfx)
            {
                audioManager.TriggerSFX(newSound.sound);
            }
        }

        // Currently not working
        if (newSound.playForTime > 0 && !fadingOut)
        {
            if (newSound.playAtTime + newSound.playForTime <= currentTime)
            {
                audioManager.FadeOutMusic(2f);
                fadingOut = true;
            }
        }

        currentTime += Time.deltaTime;

        #if UNITY_EDITOR
        // If the player presses the spacebar, make the current time equal to the time of the next intro text object
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (introTextObj text in introText)
            {
                if (text.time > currentTime)
                {
                    currentTime = text.time;
                    break;
                }
            }
        }
        #endif

    }
    private introSoundObj GetIntroSound()
    {
        if (introSounds.Length == 0)
        {
            Debug.LogError("No intro sound objects found");
            return null;
        }
        float currentHighestTime = 0;
        introSoundObj currentHighest = null;

        foreach (introSoundObj sound in introSounds)
        {
            if (currentTime >= sound.playAtTime && sound.playAtTime >= currentHighestTime)
            {
                currentHighest = sound;
                currentHighestTime = sound.playAtTime;

            }
        }
        return currentHighest;
    }

    // Determines which introTextObj to display based on the currentTime.Returns the introTextObj that has the largest time value that is less than or equal to the currentTime.
    private introTextObj GetIntroText()
    {
        if (introText.Length == 0)
        {
            Debug.LogError("No intro text objects found");
            return null;
        }

        float currentHighestTime = 0;
        introTextObj currentHighest = null;

        foreach (introTextObj text in introText)
        {
            if (currentTime >= text.time && text.time >= currentHighestTime)
            {
                currentHighest = text;
                currentHighestTime = text.time;
            }
        }
        return currentHighest;
    }
}


