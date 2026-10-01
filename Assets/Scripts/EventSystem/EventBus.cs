using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    public static List<CustomEvent> events = new List<CustomEvent>(); //all events get added here the minute they come into existence
    private static List<CustomEvent> duplicateEvents = new List<CustomEvent>(); //this is if theres accidentally any events with the same name
    //util
    public static CustomEvent RequestEvent(string eventName)
    {
        for (int i = 0; i < events.Count; i++)
        {
            if(events[i].EventName.ToLower() == eventName.ToLower()) return events[i];
        }
        return null;
    }

    public static CustomEvent RequestEvent(string eventName, bool createIfNone)
    {
        for (int i = 0; i < events.Count; i++)
        {
            if(events[i].EventName.ToLower() == eventName.ToLower()) return events[i];
        }
        if(createIfNone)
        {
            CustomEvent newEvent = new CustomEvent(eventName);
            events.Add(newEvent);
            return newEvent;
        }
        return null;
    }
}
