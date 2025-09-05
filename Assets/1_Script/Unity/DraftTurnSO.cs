using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DraftTurnSO", menuName = "BP Master/DraftTurnSO")]
public class DraftTurnSO : ScriptableObject
{
    [SerializeField] Team[] turns;
    public IReadOnlyList<Team> Turns => turns;
}
