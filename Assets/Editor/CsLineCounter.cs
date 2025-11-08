using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class CsLineCounter
{
    // 합산할 폴더 경로 2개 (원하는 경로로 수정 가능)
    static readonly string FolderPath1 = "Assets/1_Script";
    static readonly string FolderPath2 = "Assets/2_Tests";

    [MenuItem("Custom Tools/Count C# Lines (Custom Folders)")]
    public static void CountCsLines()
    {
        long totalLineCount = 0;
        int totalFileCount = 0;

        totalLineCount += CountFolder(FolderPath1, ref totalFileCount);
        totalLineCount += CountFolder(FolderPath2, ref totalFileCount);

        Debug.Log($"📄 총 C# 코드 줄 수: {totalLineCount:N0} lines, 파일 수: {totalFileCount:N0}");
    }

    static long CountFolder(string relativeFolderPath, ref int fileCount)
    {
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativeFolderPath);
        if (!Directory.Exists(fullPath))
        {
            Debug.LogWarning($"폴더를 찾을 수 없습니다: {relativeFolderPath}");
            return 0;
        }

        var csFiles = Directory.EnumerateFiles(fullPath, "*.cs", SearchOption.AllDirectories).ToList();
        fileCount += csFiles.Count;

        long lineCount = csFiles
            .Select(path => File.ReadLines(path).LongCount())
            .Sum();

        Debug.Log($"{relativeFolderPath} → {lineCount:N0} lines, {csFiles.Count:N0} files");
        return lineCount;
    }
}
