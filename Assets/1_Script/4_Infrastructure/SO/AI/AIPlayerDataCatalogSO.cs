using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "AIDataCatalog", menuName = "AI/Catalog")]
public class AIPlayerDataCatalogSO : ScriptableObject, IPlayerDataLoader
{
    [SerializeField] PlayerDataInspector[] aiPlayers;

    public PlayerData LoadPlayer(int id) => aiPlayers.First(x => x.Id == id).ToData();
}