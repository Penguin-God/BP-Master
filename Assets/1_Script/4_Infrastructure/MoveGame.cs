using UnityEngine;
using UnityEngine.UI;

public class MoveGame : MonoBehaviour
{
    [SerializeField] Button moveButton;

    LeagueScheduleUsecase _leagueScheduleUsecase;
    public void Inject(LeagueScheduleUsecase leagueScheduleUsecase)
    {
        _leagueScheduleUsecase = leagueScheduleUsecase;
    }
    void Start()
    {
        moveButton.onClick.AddListener(_leagueScheduleUsecase.ProcessNextMatch);
    }
}
