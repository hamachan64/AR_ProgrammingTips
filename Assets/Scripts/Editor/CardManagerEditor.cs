using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardManager))]
public class CardManagerEditor : Editor
{
    // SerializedProperty insideCardMotion;
    // SerializedProperty pathFirstHalf;
    // SerializedProperty pathSecondHalf;
    // SerializedProperty snakePath;

    // private void OnEnable()
    // {
    //     insideCardMotion = serializedObject.FindProperty("insideCardMotion");
    //     pathFirstHalf = serializedObject.FindProperty("pathFirstHalf");
    //     pathSecondHalf = serializedObject.FindProperty("pathSecondHalf");
    //     snakePath = serializedObject.FindProperty("snakePath");
    // }

    // public override void OnInspectorGUI()
    // {
    //     serializedObject.Update();

    //     // MotionType enum (正しい)
    //     EditorGUILayout.PropertyField(insideCardMotion);

    //     EditorGUILayout.Space(10);

    //     MotionType motion = (MotionType)insideCardMotion.enumValueIndex;

    //     switch (motion)
    //     {
    //         case MotionType.LoopCircleMotion:
    //             EditorGUILayout.LabelField("Loop Circle Motion Settings", EditorStyles.boldLabel);
    //             EditorGUILayout.PropertyField(pathFirstHalf);
    //             EditorGUILayout.PropertyField(pathSecondHalf);
    //             break;

    //         case MotionType.SnakeBackwardMotion:
    //             EditorGUILayout.LabelField("Snake Backward Motion Settings", EditorStyles.boldLabel);
    //             EditorGUILayout.PropertyField(snakePath);
    //             break;

    //             // 他のモーションは UI なし
    //     }

    //     serializedObject.ApplyModifiedProperties();
    // }
}
