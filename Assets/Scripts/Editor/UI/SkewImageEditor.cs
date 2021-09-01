using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(SkewImage))]
public class SkewImageEditor : Editor
{
    private GUIStyle contentStyle = new GUIStyle();
    public override void OnInspectorGUI()
    {
        SkewImage skewImage = target as SkewImage;

        SerializedProperty sourceImage = serializedObject.FindProperty("m_Sprite");
        EditorGUILayout.PropertyField(sourceImage, new GUIContent("Source Image"));

        SerializedProperty color = serializedObject.FindProperty("m_Color");
        EditorGUILayout.PropertyField(color, new GUIContent("Color"));

        SerializedProperty material = serializedObject.FindProperty("m_Material");
        EditorGUILayout.PropertyField(material, new GUIContent("Material"));

        SerializedProperty raycastTarget = serializedObject.FindProperty("m_RaycastTarget");
        EditorGUILayout.PropertyField(raycastTarget, new GUIContent("Raycast Target"));

        SerializedProperty raycastPadding = serializedObject.FindProperty("m_RaycastPadding");
        EditorGUILayout.PropertyField(raycastPadding, new GUIContent("Raycast Padding"));

        SerializedProperty maskable = serializedObject.FindProperty("m_Maskable");
        EditorGUILayout.PropertyField(maskable, new GUIContent("Maskable"));

        SerializedProperty skew = serializedObject.FindProperty("m_Skew");
        EditorGUILayout.PropertyField(skew, new GUIContent("Skew"));
        EditorGUI.EndChangeCheck();

        SerializedProperty type = serializedObject.FindProperty("m_Type");
        EditorGUILayout.PropertyField(type, new GUIContent("Image Type"));

        EditorGUI.indentLevel++;
        SerializedProperty preserveAspect = serializedObject.FindProperty("m_PreserveAspect");
        switch (skewImage.type)
        {
            case Image.Type.Simple:
                    SerializedProperty useSpriteMesh = serializedObject.FindProperty("m_UseSpriteMesh");
                    EditorGUILayout.PropertyField(useSpriteMesh, new GUIContent("Use Sprite Mesh"));
                    EditorGUILayout.PropertyField(preserveAspect, new GUIContent("Preserve Aspect"));
                break;
            case Image.Type.Sliced:
          
            case Image.Type.Tiled:
                    SerializedProperty fillCenter = serializedObject.FindProperty("m_FillCenter");
                    EditorGUILayout.PropertyField(fillCenter, new GUIContent("Fill Center"));
                    SerializedProperty pixelsPerUnitMultiplier = serializedObject.FindProperty("m_PixelsPerUnitMultiplier");
                    EditorGUILayout.PropertyField(pixelsPerUnitMultiplier, new GUIContent("Pixels Per Unit Multiplier"));
                break;
            case Image.Type.Filled:
                SerializedProperty fillMethod = serializedObject.FindProperty("m_FillMethod");
                EditorGUILayout.PropertyField(fillMethod, new GUIContent("Fill Method"));
                SerializedProperty fillOrigin = serializedObject.FindProperty("m_FillOrigin");
                EditorGUILayout.PropertyField(fillOrigin, new GUIContent("Fill Origin"));
                SerializedProperty fillAmount = serializedObject.FindProperty("m_FillAmount");
                EditorGUILayout.PropertyField(fillAmount, new GUIContent("Fill Amount"));
                SerializedProperty fillClockwise = serializedObject.FindProperty("m_FillClockwise");
                EditorGUILayout.PropertyField(fillClockwise, new GUIContent("Clockwise"));

                EditorGUILayout.PropertyField(preserveAspect, new GUIContent("Preserve Aspect"));
                break;
        }

        if (skewImage.type == Image.Type.Simple || skewImage.type == Image.Type.Filled)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent());
            if (GUILayout.Button(new GUIContent("Set Native Size"), EditorStyles.miniButton))
                skewImage.SetNativeSize();
            GUILayout.EndHorizontal();
        }

        EditorGUI.indentLevel--;
        serializedObject.ApplyModifiedProperties();
    }
}
#endif