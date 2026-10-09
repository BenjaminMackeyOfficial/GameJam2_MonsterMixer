//using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.Rendering;
using UnityEngine.UI;

#region coder & project
/// <summary>
/// NSCC GAME2065/4087/Game Development III(B)/Cameron,Jordan
/// Jam 2 :Monster Mixer
/// team: Chris French, Benjamin Mackey, Arianna Cutler
/// Coder current script: Chris French Second Year NSCC Game Programming 
/// Additions / annotations:
/// code review: 
/// </summary>
#endregion

public class AddAudio : MonoBehaviour
{// Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("audio clip")]
    [SerializeField] public AudioClip Audio1;// defines clip 
    [SerializeField] public AudioClip Audio2;// defines clip 
    [SerializeField] public AudioClip Audio3;// defines clip 
    [SerializeField] public AudioClip Audio4;// defines clip 
    [SerializeField] public AudioClip Audio5;// defines clip 
    [SerializeField] public AudioClip Audio6;
    private AudioSource source;// defines audio source
    private static AudioSource currentlyPlayingSource; //defines any currently playing for audio checks
    [SerializeField] private float startingVolume = .35f;


    [Header("UI Elements")]
    [SerializeField] public Slider volumeSlider; // defines  the slider ui element being used

    private void Awake()
    {
        GameObject sliderObject = GameObject.Find("Volume");
        if (sliderObject != null)
        {
            volumeSlider = sliderObject.GetComponent<Slider>();
        }

        if (source == null) source = GetComponent<AudioSource>();
        if (source == null) source = gameObject.AddComponent<AudioSource>();

        source.spatialBlend = 0f;
        source.playOnAwake = false;
        source.mute = false;
        source.clip = Audio6;
    }
    private void Start()
    {
        StartupAudio();
        On2();
    }
    public void StartupAudio()
    {
        source.volume = startingVolume; // Safely sets 0.35f fallback

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;


            if (volumeSlider.value != 1f && volumeSlider.value != 0f)// If another script already adjusted the slider from 1.0, match it
            {
                source.volume = volumeSlider.value;
            }
            else
            {
                volumeSlider.value = source.volume; // Handshake 0.35f to UI
            }

            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        }
    }

    public void OnVolumeChanged(float newVolume)
    {
        if (source != null)  //allose for volumme to be changedc based on slider valuie during play 
        {
            source.volume = newVolume;
        }
    }

    public void On1()
    {
        if (volumeSlider != null && source != null)// presets volume per new govenrences above
        {
            source.volume = volumeSlider.value;
        }

        if (source != null && source.clip != null)
        {
            source.PlayOneShot(Audio1, 1.0f);//plays defined audio clip on ente of collider zone
        }
    }

    public void On2()
    {
        if (volumeSlider != null && source != null)// presets volume per new govenrences above
        {
            source.volume = volumeSlider.value;
        }

        if (source != null && source.clip != null)
        {
            source.PlayOneShot(Audio2, 1.0f);//plays defined audio clip on ente of collider zone
        }
    }

    public void On3()
    {
        if (volumeSlider != null && source != null)// presets volume per new govenrences above
        {
            source.volume = volumeSlider.value;
        }

        if (source != null && source.clip != null)
        {
            source.PlayOneShot(Audio3, 1.0f);//plays defined audio clip on ente of collider zone
        }
    }

    public void On4()
    {
        if (volumeSlider != null && source != null)// presets volume per new govenrences above
        {
            source.volume = volumeSlider.value;
        }

        if (source != null && source.clip != null)
        {
            source.PlayOneShot(Audio4, 1.0f);//plays defined audio clip on ente of collider zone
        }
    }


}
