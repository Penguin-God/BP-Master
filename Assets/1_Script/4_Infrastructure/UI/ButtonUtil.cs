using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

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

    public static void ChangeButtonColor(Button btn, Color color)
    {
        var colors = btn.colors;
        colors.normalColor = color;
        colors.highlightedColor = color;
        colors.selectedColor = color;
        btn.colors = colors;
    }
}

public static class ObjectUtil
{
    public static void DestoryAllChild(Transform parent)
    {
        foreach (Transform child in parent)
            Object.Destroy(child.gameObject);
    }

    public static List<TView> SpawnItems<TData, TView>(Transform parent, TView prefab, IEnumerable<TData> dataList, Action<TView, TData> onInit) where TView : Component
    {
        DestoryAllChild(parent);

        var result = new List<TView>();
        foreach (var data in dataList)
        {
            var item = Object.Instantiate(prefab, parent);
            onInit(item, data);
            result.Add(item);
        }
        return result;
    }
}
