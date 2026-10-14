using UnityEngine;
using UnityEngine.Rendering;

public class BackgroundMusicManager : MonoBehaviour
{

  
    private AddAudio addAudio;
    public AudioSource musicSource; // Dedicated purely to the background music
    [SerializeField] public AudioClip backgroundTrack;
    [SerializeField] public AudioClip MorningEscape;
    void Start()
    {
        PlayAlwaysOnTrack();
        EventBus.RequestEvent("PlayerWon", true).ping += PlayMorningAfter;
        PlayAlwaysOnTrack();
    }

    public void PlayAlwaysOnTrack()
    {
        if (musicSource != null && backgroundTrack != null)
        {
            musicSource.clip = backgroundTrack;
            musicSource.loop = true; // Ensures it never stops
            musicSource.playOnAwake = true; // Optional: plays instantly when scene loads
            musicSource.volume = 2f;
            if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }
    }

    public void PlayMorningAfter()
    {
        if (musicSource != null && MorningEscape != null)
        {
            musicSource.clip = MorningEscape;
            musicSource.loop = true; // Ensures it never stops
            musicSource.playOnAwake = true; // Optional: plays instantly when scene loads
            musicSource.volume = 2f;
            if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }
    }
}