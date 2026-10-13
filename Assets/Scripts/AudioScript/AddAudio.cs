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
{
    [Header("Audio Clips - SFX (One-Shots)")]
    [SerializeField] public AudioClip SlicingLemonLimes;
    [SerializeField] public AudioClip Cheers;
    [SerializeField] public AudioClip GlassClinks;
    [SerializeField] public AudioClip SingleVGlassClink;
    [SerializeField] public AudioClip PourDrink;
    [SerializeField] public AudioClip MixingDrinks;
    [SerializeField] public AudioClip OtherSlicing;
    [SerializeField] public AudioClip IceIntoGlass;

    [Header("Audio Clips - Dialogue (Button Events)")]
    [SerializeField] public AudioClip BatGirlAudioclip;
    [SerializeField] public AudioClip BluegirlAudioclip;
    [SerializeField] public AudioClip PigtailsAudioclip;
    [SerializeField] public AudioClip NurseAudioclip;
    [SerializeField] public AudioClip VampFabioAudioclip;
    [SerializeField] public AudioClip ZombusterAudioclip;

    [Header("Audio Setup")]
    [SerializeField] public AudioSource sourceSFXandDialogue;
    [SerializeField] private float startingVolume = 0.35f;

    [Header("UI Elements")]
    [SerializeField] public Slider volumeSlider;

    private void Awake()
    {
        GameObject sliderObject = GameObject.Find("Volume");
        if (sliderObject != null && volumeSlider == null)
        {
            volumeSlider = sliderObject.GetComponent<Slider>();
        }

        if (sourceSFXandDialogue == null) sourceSFXandDialogue = GetComponent<AudioSource>();
        if (sourceSFXandDialogue == null) sourceSFXandDialogue = gameObject.AddComponent<AudioSource>();

        sourceSFXandDialogue.spatialBlend = 0f; // 2D Sound
        sourceSFXandDialogue.playOnAwake = false;
        sourceSFXandDialogue.mute = false;
    }

    private void Start()
    {
        StartupAudio();
    }

    public void StartupAudio()
    {
        sourceSFXandDialogue.volume = startingVolume;

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;


            if (volumeSlider.value != 1f && volumeSlider.value != 0f)
            {
                sourceSFXandDialogue.volume = volumeSlider.value;
            }
            else
            {
                volumeSlider.value = sourceSFXandDialogue.volume;
            }

            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }

    public void OnVolumeChanged(float newVolume)
    {
        if (sourceSFXandDialogue != null)
        {
            sourceSFXandDialogue.volume = newVolume;
        }
    }

    public void OnSlicingLemonLimes() { PlaySFX(SlicingLemonLimes); }
    public void OnCheers() { PlaySFX(Cheers); }
    public void OnGlassClinks() { PlaySFX(GlassClinks); }
    public void OnSingleVGlassClink() { PlaySFX(SingleVGlassClink); }
    public void OnPourDrink()  { PlaySFX(PourDrink); }
    public void OnMixingDrinks() { PlaySFX(MixingDrinks); }
    public void OnIceIntoGlass() { PlaySFX(IceIntoGlass); }
    public void OnOtherSlicing() { PlaySFX(OtherSlicing); }

    public void OnBatGirlAudioclip() { PlayDialogue(BatGirlAudioclip); }
    public void OnVampFabioAudioclip() { PlayDialogue(VampFabioAudioclip); }
    public void OnBluegirlAudioclip() { PlayDialogue(BluegirlAudioclip); }
    public void OnPigtailsAudioclip() { PlayDialogue(PigtailsAudioclip); }
    public void OnNurseAudioclip() { PlayDialogue(NurseAudioclip); }
    public void OnZombusterAudioclip() { PlayDialogue(ZombusterAudioclip); }

    private void PlaySFX(AudioClip clip)
    {
        UpdateVolumeFromSlider();
        if (sourceSFXandDialogue != null && clip != null)
        {
          
            sourceSFXandDialogue.PlayOneShot(clip, 2.0f);
        }
    }
    private void PlayDialogue(AudioClip clip)
    {
     
        if (sourceSFXandDialogue != null && clip != null)
        {
            sourceSFXandDialogue.volume = 2f;
            sourceSFXandDialogue.clip = clip;
            sourceSFXandDialogue.loop = false;
            sourceSFXandDialogue.Play();
        }
    }
    private void UpdateVolumeFromSlider()
    {
        if (volumeSlider != null && sourceSFXandDialogue != null)
        {
            sourceSFXandDialogue.volume = volumeSlider.value;
        }
    }
}
