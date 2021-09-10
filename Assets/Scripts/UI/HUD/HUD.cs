using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private AssetBundleManager assetBundleManager;

    [Header("Joystick")]
    [SerializeField] private Image m_JoystickBase;
    [SerializeField] private Image m_JoystickThumb;
    [Header("Crosshair")]
    [SerializeField] private Image m_Crosshair;
    [Header("HP Bar")]
    [SerializeField] private Image m_HPBar;
    [SerializeField] private Image m_HPBarRed;
    [SerializeField] private Image m_HPBarGreen;
    [Header("SP Bar")]
    [SerializeField] private Image m_SPBar;
    [SerializeField] private Image m_SPBarRed;
    [SerializeField] private Image m_SPBarWhite;
    [Header("MP Bar")]
    [SerializeField] private Image m_MPBar;
    [SerializeField] private Image m_MPBarRed;
    [SerializeField] private Image m_MPBarBlue;

    [Header("Shooting")]
    [SerializeField] private Image m_Shoot;
    [SerializeField] private Button m_ShootButton;

    [Header("Toggles")]
    [SerializeField] private Image m_FireToggle;
    [SerializeField] private Image m_FireToggleActive;
    [SerializeField] private Image m_IceToggle;
    [SerializeField] private Image m_IceToggleActive;
    [SerializeField] private Image m_LightningToggle;
    [SerializeField] private Image m_LightningToggleActive;
    [SerializeField] private Image m_WindToggle;
    [SerializeField] private Image m_WindToggleActive;

    private void Start()
    {
        Sprite[] joystickSprites = assetBundleManager.LoadBundle("uibundle").LoadAssetWithSubAssets<Sprite>("T_Joystick");
        m_JoystickBase.sprite = Array.Find(joystickSprites, item => item.name == "T_Joystick_Base");
        m_JoystickThumb.sprite = Array.Find(joystickSprites, item => item.name == "T_Joystick_Thumb");

        m_Crosshair.sprite = assetBundleManager.GetAsset<Sprite>("uibundle", "T_Crosshair");

        m_HPBar.sprite = assetBundleManager.GetAsset<Sprite>("uibundle", "T_Bar");
        m_HPBarRed.sprite = assetBundleManager.GetAsset<Sprite>("uibundle", "T_Bar");
        m_HPBarGreen.sprite = assetBundleManager.GetAsset<Sprite>("uibundle", "T_Bar");

        m_SPBar.sprite = assetBundleManager.GetAsset<Sprite>("uibundle", "T_Bar_Mask");
        m_SPBarRed.sprite = assetBundleManager.GetAsset<Sprite>("uibundle", "T_Bar");
        m_SPBarWhite.sprite = assetBundleManager.GetAsset<Sprite>("uibundle", "T_Bar");

        m_MPBar.sprite = assetBundleManager.GetAsset<Sprite>("uibundle", "T_MP_Wheel");
        m_MPBarRed.sprite = assetBundleManager.GetAsset<Sprite>("uibundle", "T_Bar");
        m_MPBarBlue.sprite = assetBundleManager.GetAsset<Sprite>("uibundle", "T_Bar");

        Sprite[] shootingSprites = assetBundleManager.LoadBundle("uibundle").LoadAssetWithSubAssets<Sprite>("T_Shooting");
        m_Shoot.sprite = Array.Find(shootingSprites, item => item.name == "T_Shoot_Button_Normal");
        var spriteState = m_ShootButton.spriteState;
        spriteState.pressedSprite = Array.Find(shootingSprites, item => item.name == "T_Shoot_Button_Pressed");
        m_ShootButton.spriteState = spriteState;

        m_FireToggle.sprite = Array.Find(shootingSprites, item => item.name == "T_Fire_Toggle_Normal");
        m_FireToggleActive.sprite = Array.Find(shootingSprites, item => item.name == "T_Fire_Toggle_Active");
        m_IceToggle.sprite = Array.Find(shootingSprites, item => item.name == "T_Ice_Toggle_Normal");
        m_IceToggleActive.sprite = Array.Find(shootingSprites, item => item.name == "T_Ice_Toggle_Active");
        m_LightningToggle.sprite = Array.Find(shootingSprites, item => item.name == "T_Lightning_Toggle_Normal");
        m_LightningToggleActive.sprite = Array.Find(shootingSprites, item => item.name == "T_Lightning_Toggle_Active");
        m_WindToggle.sprite = Array.Find(shootingSprites, item => item.name == "T_Wind_Toggle_Normal");
        m_WindToggleActive.sprite = Array.Find(shootingSprites, item => item.name == "T_Wind_Toggle_Active");
    }
}
