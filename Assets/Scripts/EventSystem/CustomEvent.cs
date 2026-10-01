using System;
using System.Collections.Generic;
using UnityEngine;


public class CustomEvent
{
    public CustomEvent(string name)
    {
        EventName = name;
    }

    public List<Data> data = new List<Data>();

    public string EventName {get ; private set;}
    public event Action ping;

    public void Invoke()
    {
        ping?.Invoke();
    }

    public void AddData(Data inputtedDat)
    {
        foreach (Data dat in data)
        {
            if (dat.GetType() == inputtedDat.GetType())
                return;
        }

        data.Add(inputtedDat);
    }

    public T GetData<T>() where T : Data, new()
    {
        foreach (Data dat in data)
        {
            if (dat is T match)
                return match;
        }

        return new T();
    }

}

public abstract class Data
{
    
}

public class GameObjData : Data
{
    public GameObjData(GameObject gameObject)
    {
        obj = gameObject;
    }
    public GameObject obj;
}
public class Vector2Data : Data
{
    public Vector2Data(Vector2 vector2)
    {
        vec2 = vector2;
    }
    public Vector2 vec2;
}

//feel free to extend off data for anything