using System.Collections;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{

  
    private AddAudio addAudio;
    private AudioSource musicSource; // returns as the active output
    [SerializeField] public AudioSource musicSource1; // same thing.. 
    [SerializeField] public AudioSource musicSource2; // same thing.. 
    private bool trackSwitch; // true is source2, false is source1

    [SerializeField] public AudioClip backgroundTrack;
    [SerializeField] public AudioClip MorningEscape;

    [SerializeField] private float fadeTime;
    void Start()
    {
        trackSwitch = false;
        SwapChanel();
    }

    public void PlayAlwaysOnTrack()
    {
        if (musicSource != null && backgroundTrack != null)
        {
            SwapChanel();
            musicSource.clip = backgroundTrack;
            musicSource.loop = true; // Ensures it never stops
            musicSource.playOnAwake = false; // Optional: plays instantly when scene loads
            //musicSource.volume = 2f;

            musicSource.Play();
           
        }
    }

    public void PlayMorningAfter()
    {
        if (musicSource != null && MorningEscape != null)
        {
            SwapChanel();
            musicSource.clip = MorningEscape;
            musicSource.loop = true; // Ensures it never stops
            musicSource.playOnAwake = false; // Optional: plays instantly when scene loads
            //musicSource.volume = 2f;
            musicSource.Play();

        }
    }
    private void SwapChanel()
    {
        if(trackSwitch)
        {
            musicSource = musicSource2;
            StartCoroutine(Fade1(0));
            StartCoroutine(Fade2(1));
        }
        else
        {
            musicSource = musicSource1;
            StartCoroutine(Fade2(0));
            StartCoroutine(Fade1(1));
        }
    }

    public void StopMusic()//"stop music" takes off the mask... its really "mute music" !!!!
    {
        StartCoroutine(Fade1(0));
        StartCoroutine(Fade2(0));
    }

    private IEnumerator Fade1(float to)
    {
        for (int i = 0; i < fadeTime * 1000; i++)
        {
            musicSource1.volume = Mathf.MoveTowards(musicSource1.volume, to, i);
            yield return new WaitForSeconds(0.001f);
        }
    }
    private IEnumerator Fade2(float to)
    {
        for (int i = 0; i < fadeTime * 1000; i++)
        {
            musicSource2.volume = Mathf.MoveTowards(musicSource2.volume, to, i);
            yield return new WaitForSeconds(0.001f);
        }
    }
}