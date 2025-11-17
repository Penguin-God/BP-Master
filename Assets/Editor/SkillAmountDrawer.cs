using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SkillAmount))]
public class SkillAmountDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var typeProp = property.FindPropertyRelative(nameof(SkillAmount.Type));
        var valueProp = property.FindPropertyRelative(nameof(SkillAmount.ValueAmount));
        var percentProp = property.FindPropertyRelative(nameof(SkillAmount.PercentValue));
        var fixProp = property.FindPropertyRelative(nameof(SkillAmount.FixValue));

        // Type enum 표시
        var typeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(typeRect, typeProp);

        // 값 표시
        var valueRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2,
                                 position.width, EditorGUIUtility.singleLineHeight);

        DrawAmountField(valueProp, AmountType.Value, "Value");
        DrawAmountField(percentProp, AmountType.Percent, "Percent");
        DrawAmountField(fixProp, AmountType.Fix, "Fix");

        EditorGUI.EndProperty();


        void DrawAmountField(SerializedProperty property, AmountType type, string content)
        {
            if (typeProp.enumValueIndex == (int)type)
                EditorGUI.PropertyField(valueRect, property, new GUIContent(content));
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2 + 4;
    }
}
