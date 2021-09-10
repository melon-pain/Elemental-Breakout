using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamage
{
    public Element element { get => m_Element; }
    [SerializeField] protected Element m_Element;

    [SerializeField] protected float m_MaxHP = 100.0f;
    [ReadOnlyInInspector, SerializeField] protected float m_CurrentHP = 100.0f;
    [SerializeField] private Bar m_HPBar;

    [Header("UI")]
    [SerializeField] private AssetBundleManager assetBundleManager;
    [SerializeField] private Image m_ElementIcon;
    [SerializeField] private Image m_HPBarIcon;

    public bool isDead { get => m_IsDead; }
    protected bool m_IsDead = false;
    public UnityEvent OnDeath = new UnityEvent();

    private Player player;

    private void Start()
    {
        m_Element = (Element)UnityEngine.Random.Range(0, 4);

        Sprite[] icons = assetBundleManager.LoadBundle("uibundle").LoadAssetWithSubAssets<Sprite>("T_Enemy_Element");
        m_ElementIcon.sprite = Array.Find<Sprite>(icons, item => item.name == "T_Enemy_Element_" + m_Element.ToString() );

        m_HPBarIcon.sprite = assetBundleManager.GetAsset<Sprite>("uibundle", "T_Bar");

        player = FindObjectOfType<Player>();
        transform.LookAt(player.transform);
        OnDeath.AddListener(player.GetComponentInChildren<PlayerShooting>().RemoveLockOn);

        m_CurrentHP = m_MaxHP;
    }

    private void Update()
    {
        if (!m_IsDead)
            transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), 15.0f);
    }
    
    public void TakeDamage(Element attacking, float amount)
    {
        if (m_IsDead)
            return;

        m_CurrentHP -= amount * Damage.GetModifier(attacking, m_Element);

        m_HPBar.UpdateBar(m_CurrentHP / m_MaxHP);

        if (m_CurrentHP <= 0.0f)
        {
            m_IsDead = true;
            OnDeath.Invoke();
            Destroy(this.gameObject);
        }
    }
}
