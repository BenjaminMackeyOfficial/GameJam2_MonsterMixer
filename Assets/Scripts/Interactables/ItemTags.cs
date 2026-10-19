
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tag
{
    public static bool ListIncludes(List<Tag> list, Tag tag)
    {
        foreach (Tag item in list)
        {
            if(item._name == tag._name) return true;
        }
        return false;
    }
    public static bool ArrayIncludes(Tag[] array, Tag tag)
    {
        foreach (Tag item in array)
        {
            if(item._name == tag._name) return true;
        }
        return false;
    }
    public static bool CompareTags(Tag tag1, Tag tag2)
    {
        if(tag1._name == tag2._name) return true;
        return false;
    }
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
    [SerializeField] List<Tag> _tags;

    public static bool DoIQualifyForDrinkPng(ItemTags item)
    {
        if (item.HasTag("Glass") != null && item._tags.Count >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ConvertToDrink()
    {
 
        if (TryGetComponent(out MeshRenderer mr)) mr.enabled = false;
        if(TryGetComponent(out SpriteRenderer osr)) osr.enabled = false;

        SpriteRenderer original = ServiceHub.Instance.spriteRendererDrink;


        GameObject spriteObject = new GameObject("DrinkSprite");
        spriteObject.transform.SetParent(transform, false);


        spriteObject.layer = LayerMask.NameToLayer("Hands");
        spriteObject.transform.localScale = new Vector3(
            transform.lossyScale.x != 0 ? 1f / transform.lossyScale.x : 1f,
            transform.lossyScale.y != 0 ? 1f / transform.lossyScale.y : 1f,
            transform.lossyScale.z != 0 ? 1f / transform.lossyScale.z : 1f
        );

        SpriteRenderer sr = spriteObject.AddComponent<SpriteRenderer>();

        sr.sprite = original.sprite;
        sr.color = original.color;
        sr.sharedMaterial = original.sharedMaterial;
        sr.sortingLayerID = original.sortingLayerID;
        sr.sortingOrder = original.sortingOrder;
        sr.flipX = original.flipX;
        sr.flipY = original.flipY;
    }

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
            EventBus.RequestEvent("TagAltered", true).Invoke();
            return true;
        }

        _tags.Add(new Tag(tagName, canHaveMulti));

        if (DoIQualifyForDrinkPng(this)) ConvertToDrink();
        EventBus.RequestEvent("TagAltered", true).Invoke();
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
            EventBus.RequestEvent("TagAltered", true).Invoke();
            return true;
        }
        _tags.Add(handedTag);

        if (DoIQualifyForDrinkPng(this)) ConvertToDrink();
        EventBus.RequestEvent("TagAltered", true).Invoke();
        return true;
    }
    public void RemoveTag() // add iff nessecary, we flyin through!!
    {
        
    }

}
