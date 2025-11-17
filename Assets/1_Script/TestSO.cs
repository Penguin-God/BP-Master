using System;
using UnityEngine;

public enum AmountType { None, Value, Percent, Fix }

[Serializable]
public class SkillAmount
{
    public AmountType Type;
    public int ValueAmount;
    public float PercentValue;
    public int FixValue;
}

[CreateAssetMenu(fileName = "TestSO", menuName = "Scriptable Objects/TestSO")]
public class TestSO : ScriptableObject
{
    [SerializeField] SkillAmount skillAmount;
}
