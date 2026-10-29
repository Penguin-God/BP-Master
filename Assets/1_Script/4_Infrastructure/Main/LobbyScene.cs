using Match;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] MatchConfigSO matchConfigSO;
    [SerializeField] SkillTextSO skillTextSO;
    [SerializeField] UI_StageRewardDisplayer stageRewardDisplayer;
    [SerializeField] MasteryRegistryFactorySO mastery;
    void Awake()
    {
        var dataIO = new JsonMasterySaver();
        var inventory = dataIO.Load();
        if (inventory == null)
            inventory = new MasteryProfile(startPoints: 15);

        var stageStorage = new PlayerPrefsStageStorage();
        FindMono<UI_StageSelection>().Init(stageStorage.LoadUnlockedStage, EnterBattle);
        var builedr = new ChampionTextBuilder(id => ChampionDataLoder.GetChampionData(id).CreateProfile(), skillTextSO.CreateSkillTextBuilder().BuildGroupedSkillText);
        FindMono<UI_MasteryPoint>().Init(new MasteryPointPresenter(inventory, builedr, dataIO, mastery.CreateMultilpe()));

        StageRewardPresenter.ShowStageReward(stageRewardDisplayer, stageStorage.LoadUnlockedStage());
    }

    T FindMono<T>() where T : MonoBehaviour => FindAnyObjectByType<T>(FindObjectsInactive.Include);

    void EnterBattle(int stage)
    {
        var stageCardIds = CardListLoder.LoadAllStageCardIds(new PlayerPrefsStageStorage().LoadUnlockedStage());
        MatchContext.MatchInit(new MatchData(matchConfigSO.UserId, stage), matchConfigSO.TargetWinCount, stageCardIds);
        SceneLoadHelper.LoadScene(SceneType.Battle);
    }
}
