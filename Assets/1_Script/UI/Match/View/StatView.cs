using TMPro;
using UnityEngine;

public class StatView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI attackText;
    [SerializeField] TextMeshProUGUI defenseText;
    [SerializeField] TextMeshProUGUI speedText;

    void Start() => ClearDisplay();

    readonly StatTextBuilder StatTextBuilder = new StatTextBuilder();
    
    public void UpdateStat(ChampionStatData statData)
    {
        StatViewModel viewModel = StatTextBuilder.CreateStatViewModel(statData);
        attackText.text = viewModel.Attack;
        defenseText.text = viewModel.Defense;
        speedText.text = viewModel.Speed;
    }

    public void ClearDisplay()
    {
        attackText.text = "공격 : -";
        defenseText.text = "방어 : -";
        speedText.text = "속도 : -";
    }
}
