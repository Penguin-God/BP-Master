using Match;
using System.Linq;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] MatchConfigSO matchConfigSO;
    [SerializeField] SkillTextSO skillTextSO;
    [SerializeField] CardListSO start;
    [SerializeField] CardListSO all;
    DeckBuildStore store;
    void Awake()
    {
        var dataIO = new JsonMasterySaver();
        var inventory = dataIO.Load();
        if (inventory == null)
            inventory = new MasteryProfile(startPoints: 15);

        FindMono<UI_StageSelection>().Init(new PlayerPrefsStageStorage().LoadUnlockedStage, EnterBattle);
        FindMono<UI_MasteryPoint>().Init(new MasteryPointPresenter(inventory, new ChampionTextBuilder(GetProfile, skillTextSO.CreateSkillTextBuilder(), new ChampionStatusTextBuilder()), FindMono<UI_MasteryPoint>(), dataIO), inventory);
        
        store = new DeckBuildStore(new DeckBuildState(matchConfigSO.BattleCardCount, all.CardIdSet, start.CardIdSet));
        FindMono<UI_DeckBuilder>().Init(store);

        int clearStage = new PlayerPrefsStageStorage().LoadUnlockedStage();
        if (StageRewardPresenter.ShouldShowStageReward(clearStage))
        {
            foreach (var item in CardListLoder.LoadStageCard(clearStage).Select(x => x.ChampionName))
            {
                print(item);
            }
        }
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