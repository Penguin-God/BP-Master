using System;
using UnityEngine;

public enum AmountType { Fix, Percent }

[Serializable]
public class SkillAmount
{
    public AmountType Type;
    public int FixValue;
    public float PercentValue;
}

[CreateAssetMenu(fileName = "TestSO", menuName = "Scriptable Objects/TestSO")]
public class TestSO : ScriptableObject
{
    [SerializeField] SkillAmount skill;
}
