using System.Collections.Generic;
using UnityEngine;

public class RoundProgressionManager : MonoBehaviour
{
    public List<Tag> haveRecieved;
    private List<Tag> totalLookingFor;
    public void Initialize()
    {
        totalLookingFor = new List<Tag>();
        
    }

    public void RecieveKeyItem(ItemTags obj)
    {
        
    }
}
