using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] TutorialTriggerSO tutorialTrigger;
    [SerializeField] MatchConfigSO matchConfigSO;
    [SerializeField] SkillTextSO skillTextSO;
    [SerializeField] CardListSO cardListSO;
    DeckBuildStore store;
    void Awake()
    {
        var dataIO = new JsonMasterySaver();
        var inventory = dataIO.Load();
        if (inventory == null)
            inventory = new MasteryProfile(startPoints: 15);

        FindMono<UI_StageSelection>().Init(new StageProgressPresenter(new PlayerPrefsStageStorage()), EnterBattle);
        FindMono<UI_MasteryPoint>().Init(new MasteryPointPresenter(inventory, new ChampionTextBuilder(GetProfile, skillTextSO.CreateSkillTextBuilder(), new ChampionStatusTextBuilder()), FindMono<UI_MasteryPoint>(), dataIO), inventory);
        tutorialTrigger.StartTutorialOneTime(TutorialType.GameStart);

        store = new DeckBuildStore(new DeckBuildState(20, new (), new HashSet<int>(cardListSO.CardList.Select(x => x.Id))));
        store.OnStateChanged += Change;
        FindMono<UI_DeckBuilder>().Init(store);

        ChampionProfile GetProfile(int id)
        {
            var so = ChampionDataLoder.GetChampionData(id);
            return new ChampionProfile(id, so.ChampionName, so.StatData, so.Skill);
        }
    }

    T FindMono<T>() where T : MonoBehaviour => FindAnyObjectByType<T>(FindObjectsInactive.Include);

    void EnterBattle(int stage) => new BattleInitializer(matchConfigSO.TargetWinCount).Resolve(new MatchData(matchConfigSO.UserId, stage));

    void OnDestroy()
    {
        if(store != null)
            store.OnStateChanged -= Change;
    }

    void Change(DeckBuildState state)
    {
        print("Change");
    }
}