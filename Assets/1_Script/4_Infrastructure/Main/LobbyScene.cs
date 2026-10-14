using Match;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] MatchConfigSO matchConfigSO;
    [SerializeField] SkillTextSO skillTextSO;
    [SerializeField] UI_StageRewardDisplayer stageRewardDisplayer;

    void Awake()
    {
        var dataIO = new JsonMasterySaver();
        var inventory = dataIO.Load();
        if (inventory == null)
            inventory = new MasteryProfile(startPoints: 15);

        var stageStorage = new PlayerPrefsStageStorage();
        FindMono<UI_StageSelection>().Init(stageStorage.LoadUnlockedStage, EnterBattle);
        FindMono<UI_MasteryPoint>().Init(new MasteryPointPresenter(inventory, new ChampionTextBuilder(GetProfile, skillTextSO.CreateSkillTextBuilder()), FindMono<UI_MasteryPoint>(), dataIO), inventory);

        StageRewardPresenter.ShowStageReward(stageRewardDisplayer, stageStorage.LoadUnlockedStage());
    }

    ChampionProfile GetProfile(int id)
    {
        var so = ChampionDataLoder.GetChampionData(id);
        return new ChampionProfile(id, so.ChampionName, so.StatData, so.Skill);
    }

    T FindMono<T>() where T : MonoBehaviour => FindAnyObjectByType<T>(FindObjectsInactive.Include);

    void EnterBattle(int stage)
    {
        var stageCardIds = CardListLoder.LoadAllStageCardIds(new PlayerPrefsStageStorage().LoadUnlockedStage());
        MatchContext.MatchInit(new MatchData(matchConfigSO.UserId, stage), matchConfigSO.TargetWinCount, stageCardIds);
        SceneLoadHelper.LoadScene(SceneType.Battle);
    }
}