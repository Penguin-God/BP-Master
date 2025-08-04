// Assets/Editor/ChampionAssetTools.cs
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public static class ChampionAssetTools
{
    private const int START_ID = 1;   // ID 시작값(필요하면 변경)

    [MenuItem("BP Master/Tools/Rename & Re-index ChampionSO Assets")]
    private static void RenameAndReindexChampionAssets()
    {
        // ──────────────────────────────────────
        // 1) Project 창에서 선택된 폴더 확인
        // ──────────────────────────────────────
        var folders = Selection
            .GetFiltered<Object>(SelectionMode.Assets)
            .Select(AssetDatabase.GetAssetPath)
            .Where(AssetDatabase.IsValidFolder)
            .ToArray();

        if (folders.Length == 0)
        {
            EditorUtility.DisplayDialog(
                "Champion Asset Tools",
                "먼저 Project 창에서 대상 폴더를 선택해 주세요.",
                "확인");
            return;
        }

        // ──────────────────────────────────────
        // 2) 폴더 내 모든 ChampionSO 수집
        // ──────────────────────────────────────
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
            EditorUtility.DisplayDialog("Champion Asset Tools", "ChampionSO가 발견되지 않았습니다.", "확인");
            return;
        }

        // ──────────────────────────────────────
        // 3) championName 으로 파일 이름 변경
        // ──────────────────────────────────────
        int renameCount = 0;
        foreach (var (champion, oldPath) in champions)
        {
            string safeName = MakeSafeFileName(champion.ChampionName);
            string newPath = Path.Combine(Path.GetDirectoryName(oldPath)!, $"{safeName}.asset");

            if (oldPath.Equals(newPath)) continue;                       // 이미 같은 이름이면 스킵
            if (AssetDatabase.LoadAssetAtPath<ChampionSO>(newPath) != null)
            {
                Debug.LogWarning($"[SKIP] {champion.ChampionName} : 같은 이름의 파일이 이미 존재합니다.");
                continue;
            }

            string err = AssetDatabase.MoveAsset(oldPath, newPath);
            if (string.IsNullOrEmpty(err)) renameCount++;
            else Debug.LogError($"[FAIL] {champion.ChampionName} : {err}");
        }

        // ──────────────────────────────────────
        // 4) ID 순차 부여 (정렬 없이, 리스트 순서 그대로)
        // ──────────────────────────────────────
        int nextId = START_ID;
        foreach (var (champion, _) in champions)
        {
            // Undo 지원
            Undo.RecordObject(champion, "Re-index ChampionSO");

            // id 필드는 private 이므로 reflection 사용
            typeof(ChampionSO)
                .GetField("id", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(champion, nextId);

            EditorUtility.SetDirty(champion);
            nextId++;
        }

        // ──────────────────────────────────────
        // 5) 저장 & 결과 알림
        // ──────────────────────────────────────
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog(
            "완료",
            $"{renameCount}개 파일 이름 변경\n{champions.Count}개 ID 부여 완료 (시작값 {START_ID})",
            "확인");
    }

    /** 파일명에 사용할 수 없는 문자 제거 */
    private static string MakeSafeFileName(string raw)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var sanitized = new string(raw.Where(c => !invalid.Contains(c)).ToArray());
        return string.IsNullOrWhiteSpace(sanitized) ? "UnnamedChampion" : sanitized;
    }
}
