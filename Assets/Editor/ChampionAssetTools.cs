// Assets/Editor/ChampionAssetTools.cs
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public static class ChampionAssetTools
{
    private const int START_ID = 1;   // ID ���۰�(�ʿ��ϸ� ����)

    [MenuItem("BP Master/Tools/Rename & Re-index ChampionSO Assets")]
    private static void RenameAndReindexChampionAssets()
    {
        // ����������������������������������������������������������������������������
        // 1) Project â���� ���õ� ���� Ȯ��
        // ����������������������������������������������������������������������������
        var folders = Selection
            .GetFiltered<Object>(SelectionMode.Assets)
            .Select(AssetDatabase.GetAssetPath)
            .Where(AssetDatabase.IsValidFolder)
            .ToArray();

        if (folders.Length == 0)
        {
            EditorUtility.DisplayDialog(
                "Champion Asset Tools",
                "���� Project â���� ��� ������ ������ �ּ���.",
                "Ȯ��");
            return;
        }

        // ����������������������������������������������������������������������������
        // 2) ���� �� ��� ChampionSO ����
        // ����������������������������������������������������������������������������
        var champions = new List<(ChampionSO so, string path)>();

        foreach (string folder in folders)
        {
            foreach (string guid in AssetDatabase.FindAssets("t:ChampionSO", new[] { folder }))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var so = AssetDatabase.LoadAssetAtPath<ChampionSO>(path);
                if (so != null) champions.Add((so, path));
            }
        }

        if (champions.Count == 0)
        {
            EditorUtility.DisplayDialog("Champion Asset Tools", "ChampionSO�� �߰ߵ��� �ʾҽ��ϴ�.", "Ȯ��");
            return;
        }

        // ����������������������������������������������������������������������������
        // 3) championName ���� ���� �̸� ����
        // ����������������������������������������������������������������������������
        int renameCount = 0;
        foreach (var (champion, oldPath) in champions)
        {
            string safeName = MakeSafeFileName(champion.ChampionName);
            string newPath = Path.Combine(Path.GetDirectoryName(oldPath)!, $"{safeName}.asset");

            if (oldPath.Equals(newPath)) continue;                       // �̹� ���� �̸��̸� ��ŵ
            if (AssetDatabase.LoadAssetAtPath<ChampionSO>(newPath) != null)
            {
                Debug.LogWarning($"[SKIP] {champion.ChampionName} : ���� �̸��� ������ �̹� �����մϴ�.");
                continue;
            }

            string err = AssetDatabase.MoveAsset(oldPath, newPath);
            if (string.IsNullOrEmpty(err)) renameCount++;
            else Debug.LogError($"[FAIL] {champion.ChampionName} : {err}");
        }

        // ����������������������������������������������������������������������������
        // 4) ID ���� �ο� (���� ����, ����Ʈ ���� �״��)
        // ����������������������������������������������������������������������������
        int nextId = START_ID;
        foreach (var (champion, _) in champions)
        {
            // Undo ����
            Undo.RecordObject(champion, "Re-index ChampionSO");

            // id �ʵ�� private �̹Ƿ� reflection ���
            typeof(ChampionSO)
                .GetField("id", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(champion, nextId);

            EditorUtility.SetDirty(champion);
            nextId++;
        }

        // ����������������������������������������������������������������������������
        // 5) ���� & ��� �˸�
        // ����������������������������������������������������������������������������
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog(
            "�Ϸ�",
            $"{renameCount}�� ���� �̸� ����\n{champions.Count}�� ID �ο� �Ϸ� (���۰� {START_ID})",
            "Ȯ��");
    }

    /** ���ϸ� ����� �� ���� ���� ���� */
    private static string MakeSafeFileName(string raw)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var sanitized = new string(raw.Where(c => !invalid.Contains(c)).ToArray());
        return string.IsNullOrWhiteSpace(sanitized) ? "UnnamedChampion" : sanitized;
    }
}
