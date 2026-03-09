using UnityEngine;
using TMPro;

public class ChampionStatusTrackerView : MonoBehaviour
{
    [SerializeField] StatChangeView statChangeView;
    [SerializeField] TextMeshProUGUI increaseRateText;
    [SerializeField] TextMeshProUGUI decreaseRateText;

    ChampionStatus target;
    readonly ChampionStatusTextBuilder StatusTextBuilder = new ChampionStatusTextBuilder();
    public void Init(ChampionStatus target)
    {
        this.target = target;
        target.OnStatChanged += (be, af) => statChangeView.ChangeStat(new StatChangeData(be, af));
    }

    //void Update()
    //{
    //    if (target == null) return;
    //    var combatModifierTextModel = StatusTextBuilder.BuildCombatModel(target.UpRate, target.DownRate);
    //    increaseRateText.text = combatModifierTextModel.IncreaseRateText;
    //    decreaseRateText.text = combatModifierTextModel.DecreaseRateText;
    //}
}
