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

    void Awake()
    {
        var dataIO = new JsonMasterySaver();
        var inventory = dataIO.Load();
        if (inventory == null)
            inventory = new MasteryProfile(startPoints: 15);

        uI_StageSelection.Init(new StageProgressPresenter(new PlayerPrefsStageStorage()), EnterBattle);
        uI_MasteryPoint.Init(new MasteryPointPresenter(inventory, new ChampionTextBuilder(GetProfile, skillTextSO.CreateSkillTextBuilder(), new ChampionStatusTextBuilder()), uI_MasteryPoint, dataIO), inventory);
        tutorialTrigger.StartTutorialOneTime(TutorialType.GameStart);

        ChampionProfile GetProfile(int id)
        {
            var so = ChampionDataLoder.GetChampionData(id);
            return new ChampionProfile(id, so.ChampionName, so.StatData, so.Skill);
        }
    }

    void EnterBattle(int stage) => new BattleInitializer(matchConfigSO.TargetWinCount).Resolve(new MatchData(matchConfigSO.UserId, stage));
}