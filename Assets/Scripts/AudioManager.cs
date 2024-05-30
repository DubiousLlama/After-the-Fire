using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public Sound[] music, sfx, ambiance;
        public AudioSource musicSource, sfxSource, ambianceSource;

        public static AudioManager instance;

        string currentlyPlaying;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            StartMusicNow("Pensive");
        }

        private void Update()
        {

            // When the player presses K, start the music "Peaceful" with a fade.Only if the game is running in the editor.
            if (Input.GetKeyDown(KeyCode.K) && Application.isEditor)
            {
                StartMusicFade("Peaceful");
            }

            // Loop the current music and ambiance tracks when they run out
            if (!musicSource.isPlaying)
            {
                StartMusicNow(currentlyPlaying);
            }

            // Keep the AudioManager centered on the player
            transform.position = Camera.main.transform.position;
        }

        public void StartMusicNow(string name)
        {
            Sound s = System.Array.Find(music, sound => sound.name == name);

            currentlyPlaying = name;

            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            musicSource.clip = s.clip;
            musicSource.volume = s.volume;
            musicSource.Play();
        }

        public void StartAmbiance(string name)
        {
            Sound s = System.Array.Find(ambiance, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            if (ambianceSource.isPlaying)
            {
                ambianceSource.Stop();
            }

            ambianceSource.clip = s.clip;
            ambianceSource.volume = s.volume;
            ambianceSource.Play();
        }

        public void TriggerSFX(string name)
        {
            Sound s = System.Array.Find(sfx, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            sfxSource.PlayOneShot(s.clip, s.volume);
        }

        // Write a version of StartMusic that, when a sound is already playing, fade out the exising sound and fade in the new sound
        public void StartMusicFade(string name, float fadeOutTime=3f, float fadeInTime=-1)
        {
            Sound s = System.Array.Find(music, sound => sound.name == name);

            currentlyPlaying = name;

            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            if (fadeInTime == -1)
            {
                fadeInTime = fadeOutTime / 2;
            }

            StartCoroutine(FadeOut(musicSource, 1.5f));
            
            // Instantiate a new AudioSource to play the new music
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.clip = s.clip;
            musicSource.volume = s.volume;
            StartCoroutine(FadeIn(musicSource, 3f));
        }

        private IEnumerator FadeOut(AudioSource source, float fadeTime)
        {
            float startVolume = source.volume;

            while (source.volume > 0)
            {
                source.volume -= startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }

            source.Stop();
            source.volume = startVolume;
            Destroy(source);
        }

        private IEnumerator FadeIn(AudioSource source, float fadeTime)
        {
            float startVolume = source.volume;
            source.volume = 0;
            source.Play();

            while (source.volume < startVolume)
            {
                source.volume += startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }

            source.volume = startVolume;
        }
    }
}

