using Match;
using System.Linq;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] MatchConfigSO matchConfigSO;
    [SerializeField] SkillTextSO skillTextSO;
    [SerializeField] CardListSO start;
    [SerializeField] CardListSO all;
    [SerializeField] UI_StageRewardDisplayer stageRewardDisplayer;

    DeckBuildStore store;
    void Awake()
    {
        var dataIO = new JsonMasterySaver();
        var inventory = dataIO.Load();
        if (inventory == null)
            inventory = new MasteryProfile(startPoints: 15);

        var stageStorage = new PlayerPrefsStageStorage();
        FindMono<UI_StageSelection>().Init(stageStorage.LoadUnlockedStage, EnterBattle);
        FindMono<UI_MasteryPoint>().Init(new MasteryPointPresenter(inventory, new ChampionTextBuilder(GetProfile, skillTextSO.CreateSkillTextBuilder(), new ChampionStatusTextBuilder()), FindMono<UI_MasteryPoint>(), dataIO), inventory);

        // store = new DeckBuildStore(new DeckBuildState(matchConfigSO.BattleCardCount, all.CardIdSet, start.CardIdSet));
        // FindMono<UI_DeckBuilder>().Init(store);

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
        MatchContext.MatchInit(new MatchData(matchConfigSO.UserId, stage), matchConfigSO.TargetWinCount, ChampionDataLoder.AllId);
        SceneLoadHelper.LoadScene(SceneType.Battle);
    }
}