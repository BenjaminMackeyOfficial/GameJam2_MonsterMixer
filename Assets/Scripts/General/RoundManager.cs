using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour //deals with switching, like when the player dies it handles all state resets and whatnot
{
    private ServiceHub serviceHub;
    void Start()
    {
        serviceHub = ServiceHub.Instance;
        EventBus.RequestEvent("StartTutorial", true).ping += EnterTutorial;
        EventBus.RequestEvent("StartGame", true).ping += GameBeginning;
        EventBus.RequestEvent("PlayerWon", true).ping += PlayerWon;
    }

    private void StartBackgroundMusic()
    {

    }
    private void StartBirdsChirping()
    {

    }
    private void FadeBackgroundAudioOut()
    {

    }
    private void FadeBackgroundAudioIn()
    {

    }
    private void EnterTutorial()
    {
        Debug.Log("Entering Tutorial...");
        serviceHub.mainMenuController.HideMainMenu();
        serviceHub.gameState.AttemptEnterDownTime();
    }
    private void GameBeginning()
    {
        Debug.Log("Entering Play...");
        serviceHub.gameState.AttemptEnterPlay();
    }

    public void CutsceneOverBeginOtherSystems()
    {
        serviceHub.backgroundMusicManager.PlayAlwaysOnTrack();
    }

    public void InitializeMainScene(bool openingCutscene)
    {
        if (openingCutscene) serviceHub.mainMenuController.RunStartSequence();
        else CutsceneOverBeginOtherSystems();
    }

    public void PlayerDied() //includes loading the end screen
    {

    }
    public void PlayerWon() //includes loading the end screen
    {
        serviceHub.gameEndMenuController.EndGame();
        serviceHub.gameState.AttemptEnterDownTime();
    }
    public void ResetLevel()
    {
        EventBus.RequestEvent("LevelReseting", true).Invoke();
    }
}
