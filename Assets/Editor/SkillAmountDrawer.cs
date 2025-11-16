using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SkillAmount))]
public class SkillAmountDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var typeProp = property.FindPropertyRelative("Type");
        var fixProp = property.FindPropertyRelative("FixValue");
        var percentProp = property.FindPropertyRelative("PercentValue");

        // Type enum 표시
        var typeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(typeRect, typeProp);

        // 값 표시
        var valueRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2,
                                 position.width, EditorGUIUtility.singleLineHeight);

        if (typeProp.enumValueIndex == 0) // Fix
            EditorGUI.PropertyField(valueRect, fixProp, new GUIContent("Fix Value"));
        else                               // Percent
            EditorGUI.PropertyField(valueRect, percentProp, new GUIContent("Percent Value"));

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2 + 4;
    }
}
