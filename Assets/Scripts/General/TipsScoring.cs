using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class TipsScoring : MonoBehaviour // should be called scoring... a little confusing
{
    public int totalTips {get; private set;} = 0;
    [SerializeField] TextMeshProUGUI tipsLabel;

    public void Start()
    {
        EventBus.RequestEvent("BonusTip", true).ping += AddBonusTip;
        EventBus.RequestEvent("LevelReseting", true).ping += ResetScore;
    }
    
    public void AddTipAmount(int ammount)
    {
        totalTips += math.clamp(ammount, int.MinValue, int.MaxValue);
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
        tipsLabel.text = "Tips: " + totalTips;
    }
    private void BonusIcon()
    {
        
    }
    private void ResetScore()
    {
        ServiceHub.Instance.fileManager.AlertDataOfNewScore(totalTips);
        totalTips = 0;
    }

}
