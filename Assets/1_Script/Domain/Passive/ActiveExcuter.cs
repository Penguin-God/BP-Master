using System;
using UnityEngine;

public class ActiveExcuter
{
    private int count;

    public ActiveExcuter(int v)
    {
        this.count = v;
    }

    public bool IsDone => count <= 0;

    public void Do()
    {
        count--;
    }
}
