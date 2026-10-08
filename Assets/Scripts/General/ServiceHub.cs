using UnityEngine;

public class ServiceHub : MonoBehaviour
{
    public static ServiceHub Instance
    {
        get{return _instance;}
    }
    private static ServiceHub _instance;

    //referances
    [SerializeField] VampireManager vampireManager;
    [SerializeField] GameState gameState;
    //

    void Awake()
    {
        if(_instance == null) _instance = this;
        else 
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(_instance);
    }
}
