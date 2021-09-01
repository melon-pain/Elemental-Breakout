using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Stat))]
public class StatEditor : PropertyDrawer
{
    private bool showValue = false;
    private SerializedProperty baseValue;
    private SerializedProperty additionalValue;

    private bool showUpgrade = false;
    private SerializedProperty upgradeLevel;
    private SerializedProperty maxUpgradeLevel;

    private bool showCost = false;
    private SerializedProperty baseUpgradeCost;
    private SerializedProperty additionalUpgradeCost;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        baseValue = property.FindPropertyRelative("m_baseValue");
        additionalValue = property.FindPropertyRelative("m_additionalValue");
        upgradeLevel = property.FindPropertyRelative("m_upgradeLevel");
        maxUpgradeLevel = property.FindPropertyRelative("m_maxUpgradeLevel");
        baseUpgradeCost = property.FindPropertyRelative("m_baseUpgradeCost");
        additionalUpgradeCost = property.FindPropertyRelative("m_additionalUpgradeCost");

        EditorGUI.BeginProperty(position, label, property);

        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true, EditorStyles.foldoutHeader);
        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;
            showValue = EditorGUILayout.Foldout(showValue, new GUIContent("Value"), true, EditorStyles.foldoutHeader);
            if (showValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(baseValue, new GUIContent("Base"));
                EditorGUILayout.PropertyField(additionalValue, new GUIContent("Per Level"));
                EditorGUI.indentLevel--;
            }

            showUpgrade = EditorGUILayout.Foldout(showUpgrade, new GUIContent("Upgrade"), true, EditorStyles.foldoutHeader);
            if (showUpgrade)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.IntSlider(upgradeLevel, 1, maxUpgradeLevel.intValue, new GUIContent("Level"));
                EditorGUILayout.PropertyField(maxUpgradeLevel, new GUIContent("Max Level"));
                EditorGUI.indentLevel--;
            }

            showCost = EditorGUILayout.Foldout(showCost, new GUIContent("Cost"), true, EditorStyles.foldoutHeader);
            if (showCost)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(baseUpgradeCost, new GUIContent("Base Cost"));
                EditorGUILayout.PropertyField(additionalUpgradeCost, new GUIContent("Additional Cost"));
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }
        EditorGUI.EndProperty();
        property.serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(Upgrades))]
public class UpgradesEditor : Editor
{
    private SerializedProperty currency;
    private static bool showPlayer
    {
        get { return SessionState.GetBool("SHOW_PLAYER", false); }
        set { SessionState.SetBool("SHOW_PLAYER", value); }
    }
    private SerializedProperty maxHP;
    private SerializedProperty maxSP;
    private SerializedProperty maxMP;

    private static bool showProjectile
    {
        get { return SessionState.GetBool("SHOW_PROJECTILE", false); }
        set { SessionState.SetBool("SHOW_PROJECTILE", value); }
    }
    private SerializedProperty projectileDMG;
    private SerializedProperty projectileCost;
    private SerializedProperty projectileRate;

    private static bool showBeam
    {
        get { return SessionState.GetBool("SHOW_BEAM", false); }
        set { SessionState.SetBool("SHOW_BEAM", value); }
    }

    private SerializedProperty beamDMG;
    private SerializedProperty beamCost;
    private SerializedProperty beamRate;

    private void OnEnable()
    {
        currency = serializedObject.FindProperty("m_currency");

        maxHP = serializedObject.FindProperty("m_maxHP");
        maxSP = serializedObject.FindProperty("m_maxSP");
        maxMP = serializedObject.FindProperty("m_maxMP");

        projectileDMG = serializedObject.FindProperty("m_projectileDMG");
        projectileRate = serializedObject.FindProperty("m_projectileRate");
        projectileCost = serializedObject.FindProperty("m_projectileCost");

        beamDMG = serializedObject.FindProperty("m_beamDMG");
        beamRate = serializedObject.FindProperty("m_beamRate");
        beamCost = serializedObject.FindProperty("m_beamCost");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(currency, new GUIContent("Currency"));
  
        showPlayer = EditorGUILayout.BeginFoldoutHeaderGroup(showPlayer, "Player");

        if (showPlayer)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(maxHP, new GUIContent("Max HP"));
            EditorUtility.SetDirty(target);
            EditorGUILayout.PropertyField(maxSP, new GUIContent("Max SP"));
            EditorUtility.SetDirty(target);
            EditorGUILayout.PropertyField(maxMP, new GUIContent("Max MP"));
            EditorUtility.SetDirty(target);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        showProjectile = EditorGUILayout.BeginFoldoutHeaderGroup(showProjectile, "Projectile");

        if (showProjectile)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(projectileDMG, new GUIContent("Damage"));
            EditorGUILayout.PropertyField(projectileRate, new GUIContent("Fire Rate"));
            EditorGUILayout.PropertyField(projectileCost, new GUIContent("MP Cost"));
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        showBeam = EditorGUILayout.BeginFoldoutHeaderGroup(showBeam, "Beam");

        if (showBeam)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(beamDMG, new GUIContent("Damage"));
            EditorGUILayout.PropertyField(beamRate, new GUIContent("Tick Rate"));
            EditorGUILayout.PropertyField(beamCost, new GUIContent("MP Cost"));
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.UpdateIfRequiredOrScript();
        serializedObject.ApplyModifiedProperties();
    }
}
#endif