using Unity.Mathematics;
using UnityEngine;

public class TipsScoring : MonoBehaviour
{
    public int totalTips {get; private set;} = 0;

    public void Start()
    {
        EventBus.RequestEvent("BonusTip", true).ping += AddBonusTip;
    }
    
    public void AddTipAmount(int ammount)
    {
        totalTips += math.clamp(ammount, 0, int.MaxValue);
        TipAddEffect();
    }
    public void AddTipAmount(float ammount)
    {
        AddTipAmount((int)ammount);
    }

    private void AddBonusTip()
    {
        AddTipAmount(EventBus.RequestEvent("BonusTip", true).GetData<IntData>().intVal);
        BonusIcon();
    }

    private void TipAddEffect() //whatever this needs to be
    {
        Debug.Log("You're at " + totalTips + " Tips!");
    }
    private void BonusIcon()
    {
        
    }
    
}
