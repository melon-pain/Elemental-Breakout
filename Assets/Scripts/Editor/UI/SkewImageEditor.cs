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

    private SerializedProperty sourceImage;
    private SerializedProperty color;
    private SerializedProperty material;
    private SerializedProperty raycastTarget;
    private SerializedProperty raycastPadding;
    private SerializedProperty maskable;
    private SerializedProperty skew;
    private SerializedProperty type;
    private SerializedProperty preserveAspect;
    private SerializedProperty useSpriteMesh;
    private SerializedProperty fillCenter;
    private SerializedProperty pixelsPerUnitMultiplier;
    private SerializedProperty fillMethod;
    private SerializedProperty fillOrigin;
    private SerializedProperty fillAmount;
    private SerializedProperty fillClockwise;

    void OnEnable()
    {
        sourceImage = serializedObject.FindProperty("m_Sprite");
        color = serializedObject.FindProperty("m_Color");
        material = serializedObject.FindProperty("m_Material");
        raycastTarget = serializedObject.FindProperty("m_RaycastTarget");
        raycastPadding = serializedObject.FindProperty("m_RaycastPadding");
        maskable = serializedObject.FindProperty("m_Maskable");
        skew = serializedObject.FindProperty("m_Skew");
        type = serializedObject.FindProperty("m_Type");
        preserveAspect = serializedObject.FindProperty("m_PreserveAspect");
        useSpriteMesh = serializedObject.FindProperty("m_UseSpriteMesh");
        fillCenter = serializedObject.FindProperty("m_FillCenter");
        pixelsPerUnitMultiplier = serializedObject.FindProperty("m_PixelsPerUnitMultiplier");
        fillMethod = serializedObject.FindProperty("m_FillMethod");
        fillOrigin = serializedObject.FindProperty("m_FillOrigin");
        fillAmount = serializedObject.FindProperty("m_FillAmount");
        fillClockwise = serializedObject.FindProperty("m_FillClockwise");
    }

    public override void OnInspectorGUI()
    {
        SkewImage skewImage = target as SkewImage;

        EditorGUILayout.PropertyField(sourceImage, new GUIContent("Source Image"));

        EditorGUILayout.PropertyField(color, new GUIContent("Color"));

        EditorGUILayout.PropertyField(material, new GUIContent("Material"));

        EditorGUILayout.PropertyField(raycastTarget, new GUIContent("Raycast Target"));

        EditorGUILayout.PropertyField(raycastPadding, new GUIContent("Raycast Padding"));

        EditorGUILayout.PropertyField(maskable, new GUIContent("Maskable"));

        EditorGUILayout.PropertyField(skew, new GUIContent("Skew"));

        EditorGUILayout.PropertyField(type, new GUIContent("Image Type"));

        EditorGUI.indentLevel++;
        switch (skewImage.type)
        {
            case Image.Type.Simple:
                    EditorGUILayout.PropertyField(useSpriteMesh, new GUIContent("Use Sprite Mesh"));
                    EditorGUILayout.PropertyField(preserveAspect, new GUIContent("Preserve Aspect"));
                break;
            case Image.Type.Sliced:
          
            case Image.Type.Tiled:
                    EditorGUILayout.PropertyField(fillCenter, new GUIContent("Fill Center"));
                    EditorGUILayout.PropertyField(pixelsPerUnitMultiplier, new GUIContent("Pixels Per Unit Multiplier"));
                break;
            case Image.Type.Filled:
                EditorGUILayout.PropertyField(fillMethod, new GUIContent("Fill Method"));
                EditorGUILayout.PropertyField(fillOrigin, new GUIContent("Fill Origin"));
                EditorGUILayout.PropertyField(fillAmount, new GUIContent("Fill Amount"));
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