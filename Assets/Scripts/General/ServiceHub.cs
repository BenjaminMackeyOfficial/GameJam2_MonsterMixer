using UnityEngine;

public class ServiceHub : MonoBehaviour
{
    public static ServiceHub Instance
    {
        get{return _instance;}
    }
    private static ServiceHub _instance;

    //referances
    public VampireManager vampireManager;
    public GameState gameState;
    public GameObject player;
    public RoundProgressionManager roundProgressionManager;
    public PopupManager popupManager;
    public TipsScoring tips;
    public CameraDataHolder cameras;
    public AddAudio audioManager;
    public MainMenuController mainMenuController;
    public GameEndMenuController gameEndMenuController;
    public PauseMenuController pauseMenuController;
    public Material generalOutlineMat;
    public SpriteRenderer spriteRendererDrink;
    //

    void Awake()
    {
        if(_instance == null) _instance = this;
        else 
        {
            Destroy(this);
            return;
        }

        //DontDestroyOnLoad(_instance);

        vampireManager.Initialize();
        gameState.Initialize();
        
        //EventBus.RequestEvent("RequestPlayMode", true).Invoke();
    }
}
