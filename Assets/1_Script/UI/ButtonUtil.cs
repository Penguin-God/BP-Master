using UnityEngine;
using UnityEngine.UI;

public static class ButtonUtil
{
    public static void InActiveButton(Button button)
    {
        button.enabled = false;
        var colors = button.colors;
        colors.normalColor = new Color(0.5f, 0.5f, 0.5f); // 회색 톤
        button.colors = colors;
    }

    public static void ActiveButton(Button btn)
    {
        btn.enabled = true;
        var colors = btn.colors;
        colors.normalColor = new Color(1f, 1f, 1f);
        btn.colors = colors;
    }
}