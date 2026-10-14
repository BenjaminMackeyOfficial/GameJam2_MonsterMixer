using System;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [Serializable]
    public enum GameplayState
    {
        Menu, 
        DownTime, //this is like for any tutorials, basically just the vampire controller wont send anything
        Playing,
        Paused,
    }

    public GameplayState gameplayState{ get; private set;}
    
    public void Initialize()
    {
        gameplayState = GameplayState.Menu;
        EventBus.RequestEvent("RequestPlayMode", true).ping += AttemptEnterPlay;
        EventBus.RequestEvent("RequestMenuMode", true).ping += AttemptEnterMenu;
        EventBus.RequestEvent("RequestDownTimeMode", true).ping += AttemptEnterDownTime;
        EventBus.RequestEvent("RequestPauseMode", true).ping += AttemptEnterPause;
    }

    public void AttemptEnterPlay()
    {
        if(gameplayState == GameplayState.Playing) return;
        gameplayState = GameplayState.Playing;
        EventBus.RequestEvent("EnterdPlayMode");
    }
    public void AttemptEnterMenu()
    {
        if(gameplayState == GameplayState.Menu) return;
        gameplayState = GameplayState.Menu;
        EventBus.RequestEvent("EnterdMenuMode");
    }
    public void AttemptEnterDownTime()
    {
        if(gameplayState == GameplayState.DownTime) return;
        gameplayState = GameplayState.DownTime;
        EventBus.RequestEvent("EnterdDownTimeMode");
    }
    public void AttemptEnterPause()
    {
        if(gameplayState == GameplayState.Paused) return;
        gameplayState = GameplayState.Paused;
        EventBus.RequestEvent("EnterdPauseMode");
    }

}
