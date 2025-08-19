using UnityEngine;

public class Champion
{
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public Champion(int attack, int defense, int range, int speed)
    {
        Attack = attack; 
        Defense = defense;
    }
}
