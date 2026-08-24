using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerDataLoader : IPlayerDataLoader
{
    readonly string playerName;
    readonly JsonMasterySaver saver;

    public LocalPlayerDataLoader(string playerName, JsonMasterySaver saver)
    {
        this.playerName = playerName;
        this.saver = saver;
    }

    public PlayerData LoadPlayer(int id)
    {
        var inventory = saver.Load();
        if(inventory == null)
            inventory = new MasteryProfile(0);
        return new PlayerData(id, playerName, inventory.BoardCollection);
    }
}

public class LobbyScene : MonoBehaviour
{
    [SerializeField] TutorialTriggerSO tutorialTrigger;
    [SerializeField] UI_StageSelection uI_StageSelection;
    [SerializeField] MatchConfigSO matchConfigSO;
    [SerializeField] UI_MasteryPoint uI_MasteryPoint;
    [SerializeField] SkillTextSO skillTextSO;

    DeckBuildStore store;
    void Awake()
    {
        var dataIO = new JsonMasterySaver();
        var inventory = dataIO.Load();
        if (inventory == null)
            inventory = new MasteryProfile(startPoints: 15);

        uI_StageSelection.Init(new StageProgressPresenter(new PlayerPrefsStageStorage()), EnterBattle);
        uI_MasteryPoint.Init(new MasteryPointPresenter(inventory, new ChampionTextBuilder(GetProfile, skillTextSO.CreateSkillTextBuilder(), new ChampionStatusTextBuilder()), uI_MasteryPoint, dataIO), inventory);
        tutorialTrigger.StartTutorialOneTime(TutorialType.GameStart);

        store = new DeckBuildStore(new DeckBuildState(20, new HashSet<int> { 1, 2, 3 }, new()));
        store.OnStateChanged += Change;
        FindAnyObjectByType<UI_DeckBuilder>().Init(store);

        ChampionProfile GetProfile(int id)
        {
            var so = ChampionDataLoder.GetChampionData(id);
            return new ChampionProfile(id, so.ChampionName, so.StatData, so.Skill);
        }
    }

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