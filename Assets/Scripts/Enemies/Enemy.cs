using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamage
{
    [Header("Element")]
    [SerializeField] private Element m_Element;
    public Element element { get => m_Element; }
    [SerializeField] private bool m_RandomElement = true;

    [Header("HP")]
    [SerializeField] private float m_MaxHP = 50.0f;
    public float maxHP { get => m_MaxHP; }
    [ReadOnlyInInspector, SerializeField] private float m_CurrentHP = 50.0f;
    public float currentHP { get => m_CurrentHP; }
    [SerializeField] private Bar m_HPBar;

    [Header("Score")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private int m_Score = 100;

    [Header("Body")]
    [SerializeField] private SkinnedMeshRenderer m_Body;
    [SerializeField] List<Color> m_BodyColors = new List<Color>();

    [Header("Core")]
    [SerializeField] private SkinnedMeshRenderer m_Core;
    [ColorUsage(true, true)] [SerializeField] List<Color> m_CoreColors = new List<Color>();

    [Header("UI")]
    [SerializeField] private AssetBundleManager assetBundleManager;
    [SerializeField] private Image m_ElementIcon;
    [SerializeField] private Image m_HPBarIcon;
    [SerializeField] private Image m_Indicator;

    public bool isDead { get => m_IsDead; }
    private bool m_IsDead = false;
    public UnityEvent OnDeath = new UnityEvent();

    private Player player;

    private void Awake()
    {
        if (m_RandomElement)
            m_Element = (Element)UnityEngine.Random.Range(0, 4);

        m_Body.material.SetColor("_BaseColor", m_BodyColors[(int)m_Element]);
        m_Core.material.SetColor("_BaseColor", m_CoreColors[(int)m_Element]);
        
        player = FindObjectOfType<Player>();
        transform.LookAt(player.transform);
        OnDeath.AddListener(player.GetComponentInChildren<PlayerShooting>().RemoveLockOn);

        m_CurrentHP = m_MaxHP;

        AssetBundle bundle = assetBundleManager.LoadBundle("uibundle");

        Sprite[] icons = bundle.LoadAssetWithSubAssets<Sprite>("T_Enemy_Element");
        m_ElementIcon.sprite = Array.Find<Sprite>(icons, item => item.name == "T_Enemy_Element_" + m_Element.ToString());

        m_HPBarIcon.sprite = bundle.LoadAsset<Sprite>("T_Bar");
        m_Indicator.sprite = bundle.LoadAsset<Sprite>("T_Indicator");

        assetBundleManager.UnloadBundle("uibundle");

        levelManager = FindObjectOfType<LevelManager>();
        OnDeath.AddListener(delegate { levelManager.AddScore(m_Score);  } );
    }

    private void Update()
    {
        if (!m_IsDead)
            transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), 15.0f);
    }
    
    public void TakeDamage(Element attacking, float amount)
    {
        if (m_IsDead)
        {
            return;
        }

        m_CurrentHP -= amount * Damage.GetModifier(attacking, m_Element);

        m_HPBar.UpdateBar(m_CurrentHP / m_MaxHP);

        if (m_CurrentHP <= 0.0f)
        {
            m_IsDead = true;
            OnDeath.Invoke();
            Destroy(this.gameObject);
        }
    }

    public void ChangeElement(Element newElement)
    {
        m_Element = newElement;
        m_Body.material.SetColor("_BaseColor", m_BodyColors[(int)m_Element]);
        m_Core.material.SetColor("_BaseColor", m_CoreColors[(int)m_Element]);
    }
}
