using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "AIPlayerDataCatalog", menuName = "SO/Match/AIPlayerDataCatalog")]
public class AIPlayerDataCatalogSO : ScriptableObject, IPlayerDataLoader
{
    [SerializeField] PlayerDataInspector[] aiPlayers;

    public PlayerData LoadPlayer(int id) => aiPlayers.First(x => x.Id == id).ToData();
}