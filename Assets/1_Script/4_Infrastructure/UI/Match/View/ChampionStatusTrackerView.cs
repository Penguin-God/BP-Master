using UnityEngine;

public class ChampionStatusTrackerView : MonoBehaviour
{
    [SerializeField] StatChangeView statChangeView;
    public void Init(ChampionStatus target)
    {
        target.OnStatChanged += (be, af) => statChangeView.ChangeStat(new StatChangeData(be, af));
    }
}
