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

    private BackgroundMusicManager backgroundMusicManager;

    [Header("audio clip")]
    [SerializeField] public AudioClip SlicingLemonLimes;// defines clip 
    [SerializeField] public AudioClip Cheers;// defines clip 
    [SerializeField] public AudioClip GlassClinks;// defines clip 
    [SerializeField] public AudioClip SingleVGlassClink;// defines clip 
    [SerializeField] public AudioClip PourDrink;// defines clip 
    [SerializeField] public AudioClip MixingDrinks;

    [SerializeField] public AudioClip OtherSlicing;// defines clip 
    [SerializeField] public AudioClip IceIntoGlass;// defines clip 
    [SerializeField] public AudioClip BatGirlAudioclip;// defines clip 
    [SerializeField] public AudioClip BluegirlAudioclip;// defines clip 
    [SerializeField] public AudioClip PigtailsAudioclip;// defines clip 
    [SerializeField] public AudioClip NurseAudioclip;// defines clip 
    [SerializeField] public AudioClip VampFabioAudioclip;// defines clip 
    [SerializeField] public AudioClip ZombusterAudioclip;// defines clip 
    [SerializeField] public AudioSource sourceSFXandDialogue;// defines audio source
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

        if (sourceSFXandDialogue == null) sourceSFXandDialogue = GetComponent<AudioSource>();
        if (sourceSFXandDialogue == null) sourceSFXandDialogue = gameObject.AddComponent<AudioSource>();

        sourceSFXandDialogue.spatialBlend = 0f;
        sourceSFXandDialogue.playOnAwake = false;
        sourceSFXandDialogue.mute = false;
    
    }
    private void Start()
    {
        StartupAudio();
       
    }
    public void StartupAudio()
    {
        sourceSFXandDialogue.volume = startingVolume; // Safely sets 0.35f fallback

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;


            if (volumeSlider.value != 1f && volumeSlider.value != 0f)// If another script already adjusted the slider from 1.0, match it
            {
                sourceSFXandDialogue.volume = volumeSlider.value;
            }
            else
            {
                volumeSlider.value = sourceSFXandDialogue.volume; // Handshake 0.35f to UI
            }

            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        }
    }

    public void OnVolumeChanged(float newVolume)
    {
        if (sourceSFXandDialogue != null)  //allose for volumme to be changedc based on slider valuie during play 
        {
            sourceSFXandDialogue.volume = newVolume;
        }
    }
 
    public void OnSlicingLemonLimes()
    {
        if (volumeSlider != null && sourceSFXandDialogue != null)// presets volume per new govenrences above
        {
            sourceSFXandDialogue.volume = volumeSlider.value;
        }

        if (sourceSFXandDialogue != null && sourceSFXandDialogue.clip != null)
        {
            sourceSFXandDialogue.PlayOneShot(SlicingLemonLimes, 1.0f);//plays defined audio clip on ente of collider zone
        
        }
    }


    public void OnCheers()
    {
        if (volumeSlider != null && sourceSFXandDialogue != null)// presets volume per new govenrences above
        {
            sourceSFXandDialogue.volume = volumeSlider.value;
        }

        if (sourceSFXandDialogue != null && sourceSFXandDialogue.clip != null)
        {
            sourceSFXandDialogue.PlayOneShot(Cheers, 1.0f);//plays defined audio clip on ente of collider zone
   
        }
    }

  
    public void OnGlassClinks()
    {
        if (volumeSlider != null && sourceSFXandDialogue != null)// presets volume per new govenrences above
        {
            sourceSFXandDialogue.volume = volumeSlider.value;
        }

        if (sourceSFXandDialogue != null && sourceSFXandDialogue.clip != null)
        {
            sourceSFXandDialogue.PlayOneShot(GlassClinks, 1.0f);//plays defined audio clip on ente of collider zone
     
        }
    }

    public void OnSingleVGlassClink()
    {
        if (volumeSlider != null && sourceSFXandDialogue != null)// presets volume per new govenrences above
        {
            sourceSFXandDialogue.volume = volumeSlider.value;
        }

        if (sourceSFXandDialogue != null && sourceSFXandDialogue.clip != null)
        {
            sourceSFXandDialogue.PlayOneShot(SingleVGlassClink, 1.0f);//plays defined audio clip on ente of collider zone
      
        }
    }
    public void OnPourDrink()
    {
        if (volumeSlider != null && sourceSFXandDialogue != null)// presets volume per new govenrences above
        {
            sourceSFXandDialogue.volume = volumeSlider.value;
        }

        if (sourceSFXandDialogue != null && sourceSFXandDialogue.clip != null)
        {
            sourceSFXandDialogue.PlayOneShot(PourDrink, 1.0f);//plays defined audio clip on ente of collider zone
    
        }
    }
    public void OnMixingDrinks()
    {
        if (volumeSlider != null && sourceSFXandDialogue != null)// presets volume per new govenrences above
        {
            sourceSFXandDialogue.volume = volumeSlider.value;
        }

        if (sourceSFXandDialogue != null && sourceSFXandDialogue.clip != null)
        {
            sourceSFXandDialogue.PlayOneShot(MixingDrinks, 1.0f);//plays defined audio clip on ente of collider zone
    
        }
    }
    public void OnIceIntoGlass()
    {
        if (volumeSlider != null && sourceSFXandDialogue != null)// presets volume per new govenrences above
        {
            sourceSFXandDialogue.volume = volumeSlider.value;
        }

        if (sourceSFXandDialogue != null && sourceSFXandDialogue.clip != null)
        {
            sourceSFXandDialogue.PlayOneShot(IceIntoGlass, 1.0f);//plays defined audio clip on ente of collider zone
       
      
        }
        
    }
    public void OnOtherSlicing()
    {
        if (volumeSlider != null && sourceSFXandDialogue != null)// presets volume per new govenrences above
        {
            sourceSFXandDialogue.volume = volumeSlider.value;
        }

        if (sourceSFXandDialogue != null && sourceSFXandDialogue.clip != null)
        {
            sourceSFXandDialogue.PlayOneShot(OtherSlicing, 1.0f);//plays defined audio clip on ente of collider zone
       
     
        }
   
    }
    public void OnBatGirlAudioclip()
    {
        if (volumeSlider != null && sourceSFXandDialogue != null)// presets volume per new govenrences above
        {
            sourceSFXandDialogue.volume = volumeSlider.value;
        }

        if (sourceSFXandDialogue != null && sourceSFXandDialogue.clip != null)
        {
            sourceSFXandDialogue.volume = 2f;
            sourceSFXandDialogue.clip = BatGirlAudioclip;
            sourceSFXandDialogue.loop = false;
            sourceSFXandDialogue.Play();     
                
        }
        
    }
    public void OnVampFabioAudioclip()
    {
        if (volumeSlider != null && sourceSFXandDialogue != null)// presets volume per new govenrences above
        {
            sourceSFXandDialogue.volume = volumeSlider.value;
        }

        if (sourceSFXandDialogue != null && sourceSFXandDialogue.clip != null)
        {
            sourceSFXandDialogue.volume = 2f;
            sourceSFXandDialogue.clip = VampFabioAudioclip;
            sourceSFXandDialogue.loop = false;
            sourceSFXandDialogue.Play();
           
        }
      
    }
    public void OnBluegirlAudioclip()
    {
        if (volumeSlider != null && sourceSFXandDialogue != null)// presets volume per new govenrences above
        {
            sourceSFXandDialogue.volume = volumeSlider.value;
        }

        if (sourceSFXandDialogue != null && sourceSFXandDialogue.clip != null)
        {
            sourceSFXandDialogue.volume = 2f;
            sourceSFXandDialogue.clip = BluegirlAudioclip;
            sourceSFXandDialogue.loop = false;
            sourceSFXandDialogue.Play();
            
        }
       
    }
    public void OnPigtailsAudioclip()
    {
        if (volumeSlider != null && sourceSFXandDialogue != null)// presets volume per new govenrences above
        {
            sourceSFXandDialogue.volume = volumeSlider.value;
        }

        if (sourceSFXandDialogue != null && sourceSFXandDialogue.clip != null)
        {
            sourceSFXandDialogue.volume = 2f;
            sourceSFXandDialogue.clip = PigtailsAudioclip;
            sourceSFXandDialogue.loop = false;
            sourceSFXandDialogue.Play();
         ;
        }
        

    }
    public void OnNurseAudioclip()
    {
        if (volumeSlider != null && sourceSFXandDialogue != null)// presets volume per new govenrences above
        {
            sourceSFXandDialogue.volume = volumeSlider.value;
        }

        if (sourceSFXandDialogue != null && sourceSFXandDialogue.clip != null)
        {
            sourceSFXandDialogue.volume = 2f;
            sourceSFXandDialogue.clip = NurseAudioclip;
            sourceSFXandDialogue.loop = false;
            sourceSFXandDialogue.Play();
           
        }
        
    }
    public void OnZombusterAudioclip()
    {
        if (volumeSlider != null && sourceSFXandDialogue != null)// presets volume per new govenrences above
        {
            sourceSFXandDialogue.volume = volumeSlider.value;
        }

        if (sourceSFXandDialogue != null && sourceSFXandDialogue.clip != null)
        {
            sourceSFXandDialogue.volume = 2f;
            sourceSFXandDialogue.clip = ZombusterAudioclip;
            sourceSFXandDialogue.loop = false;
            sourceSFXandDialogue.Play();
             
        }
     
    }


}
