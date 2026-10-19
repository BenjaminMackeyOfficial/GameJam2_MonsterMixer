using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData
{
    public float _highScore;
    public bool _hasPlayed;
    public bool _hasBeaten;
    public DateTime _lastPlayed;
    public PlayerData(float highScore, bool hasPlayed, bool hasBeaten, DateTime lastPlayed)
    {
        _highScore = highScore;
        _hasPlayed = hasPlayed;
        _hasBeaten = hasBeaten;
        _lastPlayed = lastPlayed;
        
    }
    public PlayerData()
    {
        _highScore = 0f;
        _hasPlayed = false;
        _hasBeaten = false;
        _lastPlayed = DateTime.Now;
    }
}

public class FileManager : MonoBehaviour
{
    public PlayerData _playerData;
    private void Awake()
    {
        if (!File.Exists(Application.persistentDataPath + "/playerdata.dat"))
        {
            File.Create(Application.persistentDataPath + "/playerdata.dat");
        }
        _playerData = load();
        Debug.Log(
            "Loaded Highscore: " + _playerData._highScore +
            " | Has beaten: " + _playerData._hasBeaten +
            " | Has Played: " + _playerData._hasPlayed +
            " | Last time played: " + _playerData._lastPlayed
            );

    }
    private void Start()
    {
        EventBus.RequestEvent("LevelReseting", true).ping += SelfSave;
        EventBus.RequestEvent("PlayerWon", true).ping += PlayerWon;
    }
    private void PlayerWon()
    {
        _playerData._hasBeaten = true;
        SelfSave();
    }
    void OnApplicationQuit()
    {
        save(_playerData);
    }
    public void AlertDataOfNewScore(float score) // sends when the round ends
    {
        if(score > _playerData._highScore) _playerData._highScore = score;
    }

    //SAVING===========================================================
    public void SelfSave()
    {
        _playerData._lastPlayed = DateTime.Now;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.OpenWrite(Application.persistentDataPath + "/playerdata.dat");

        PlayerData data = _playerData;
        bf.Serialize(file, data);
        file.Close();
    }    
    public void save(PlayerData handedData)
    {
        _playerData._lastPlayed = DateTime.Now;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.OpenWrite(Application.persistentDataPath + "/playerdata.dat");

        PlayerData data = handedData;
        bf.Serialize(file, data);
        file.Close();
    }
    //=========================================================================

    public PlayerData load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.OpenRead(Application.persistentDataPath + 
            "/playerdata.dat");
        
        PlayerData data;


        if (file == null)
        {
            data = new PlayerData();
        }
        else
        {
            data = (PlayerData)bf.Deserialize(file);
        }
        file.Close();

        return data;
    }
}
