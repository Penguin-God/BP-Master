using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "CardListSO", menuName = "Data/CardList")]
public class CardListSO : ScriptableObject
{
    [OnValueChanged(nameof(RemoveDuplicates), includeChildren: true)]
    [SerializeField] [Header("중복 허용 안하는 리스트")] ChampionSO[] _cardList;
    public IEnumerable<ChampionSO> CardList => _cardList;

    void RemoveDuplicates() => _cardList = _cardList.Where(x => x != null).Distinct().ToArray();

    [ContextMenu("Load Champions From Folder")]
    [Button("폴더에서 챔피언 SO 채우기", ButtonSizes.Medium)]
    void LoadChampionsFromFolder()
    {
        // 1. 탐색할 기본 폴더 경로 (프로젝트 구조에 맞춰 수정)
        string folderPath = "Assets/Data/Champions";

        // 폴더 선택창에서 선택
        folderPath = EditorUtility.OpenFolderPanel("Champion SO 폴더 선택", "Assets", "");
        if (string.IsNullOrEmpty(folderPath)) return;
        folderPath = "Assets" + folderPath.Substring(Application.dataPath.Length);

        // 2. 해당 폴더 내의 ChampionSO 타입 에셋 검색
        string[] guids = AssetDatabase.FindAssets($"t:{nameof(ChampionSO)}", new[] { folderPath });

        var loadedList = guids
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Select(path => AssetDatabase.LoadAssetAtPath<ChampionSO>(path))
            .Where(x => x != null)
            .Distinct()
            .ToArray();

        Undo.RecordObject(this, "Load Champions From Folder");
        _cardList = loadedList;
        EditorUtility.SetDirty(this);

        Debug.Log($"[{name}] {folderPath} 경로에서 {loadedList.Length}개의 챔피언 SO를 로드했습니다.");
    }
}
