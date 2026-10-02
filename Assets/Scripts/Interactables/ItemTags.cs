using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tag
{
    public Tag(string name, bool canHaveMulti)
    {
        _name = name;
        _canHaveMultiple = canHaveMulti;
    }
    public string _name;
    public bool _canHaveMultiple;
    public int _count;
}
public class ItemTags : MonoBehaviour
{
    private List<Tag> _tags;
    
    public Tag HasTag(string name)
    {
        foreach (Tag tag in _tags)
        {
            if(tag._name == name) return tag;
        }
        return null;
    }
    public Tag[] GetAllTags()
    {
        return _tags.ToArray();
    }
    public bool AddTag(string tagName, bool canHaveMulti)
    {
        if(_tags == null) _tags = new List<Tag>();
        foreach (Tag tag in _tags)
        {
            if(tag._name != tagName) continue; // skipping the other checks if the tag name doesnt match
            if(tag._canHaveMultiple == false) return false; // returning if trying to add single use tag

            tag._count += 1;
            return true;
        }
        _tags.Add(new Tag(tagName, canHaveMulti));
        return true;
    }
    public bool AddTag(Tag handedTag)
    {
        if(_tags == null) _tags = new List<Tag>();
        foreach (Tag tag in _tags)
        {
            if(tag._name != handedTag._name) continue; // skipping the other checks if the tag name doesnt match
            if(tag._canHaveMultiple == false) return false; // returning if trying to add single use tag

            tag._count += handedTag._count;
            return true;
        }
        _tags.Add(handedTag);
        return true;
    }
    public void RemoveTag() // add iff nessecary, we flyin through!!
    {
        
    }
}
