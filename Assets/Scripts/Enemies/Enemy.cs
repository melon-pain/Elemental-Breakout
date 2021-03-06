using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour, IDamage
{
    [SerializeField] private bool m_IsBoss = false;
    [Header("Element")]
    [SerializeField] private Element m_Element;
    public Element element { get => m_Element; }
    [SerializeField] private bool m_RandomElement = true;
    [SerializeField] private bool m_ChangeElementOnHP = false;
    [SerializeField] private bool m_ChangeElementOnTime = false;

    [Header("HP")]
    [SerializeField] private float m_MaxHP = 50.0f;
    public float maxHP { get => m_MaxHP; }
    [ReadOnlyInInspector, SerializeField] private float m_CurrentHP = 50.0f;
    public float currentHP { get => m_CurrentHP; }
    [SerializeField] private Bar m_HPBar;

    [SerializeField] private float m_TurnSpeed = 15.0f;

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
    [SerializeField] private Image m_HPBarRed;
    [SerializeField] private Image m_HPBarOrange;
    [SerializeField] private Image m_Indicator;

    [Header("Audio")]
    [SerializeField] private AudioClip m_DamageTaken;

    public bool isDead { get => m_IsDead; }
    private bool m_IsDead = false;
    public UnityEvent OnDeath = new UnityEvent();

    private Player player;
    private AudioSource audioSource;

    private void Awake()
    {
        m_Body.enabled = false;
        m_Core.enabled = false;

        if (m_RandomElement)
            m_Element = (Element)UnityEngine.Random.Range(0, 4);

        m_Body.material.SetColor("_BaseColor", m_BodyColors[(int)m_Element]);
        m_Core.material.SetColor("_BaseColor", m_CoreColors[(int)m_Element]);
        
        player = FindObjectOfType<Player>();
        transform.LookAt(player.transform);
        OnDeath.AddListener(player.GetComponentInChildren<PlayerShooting>().RemoveLockOn);

        m_CurrentHP = m_MaxHP;
        audioSource = this.GetComponent<AudioSource>();

        AssetBundle bundle = assetBundleManager.LoadBundle("uibundle");

        Sprite[] icons = bundle.LoadAssetWithSubAssets<Sprite>("T_Enemy_Element");
        m_ElementIcon.sprite = Array.Find<Sprite>(icons, item => item.name == "T_Enemy_Element_" + m_Element.ToString());

        m_HPBarIcon.sprite = bundle.LoadAsset<Sprite>("T_Bar");
        m_HPBarRed.sprite = bundle.LoadAsset<Sprite>("T_Bar");
        m_HPBarOrange.sprite = bundle.LoadAsset<Sprite>("T_Bar");
        m_Indicator.sprite = bundle.LoadAsset<Sprite>("T_Indicator");

        levelManager = FindObjectOfType<LevelManager>();
        OnDeath.AddListener(delegate { levelManager.AddScore(m_Score);  } );

        if (m_ChangeElementOnTime)
            StartCoroutine(ChangeElementOnTime());

        if (m_IsBoss)
            OnDeath.AddListener(delegate { levelManager.GameOver(true); });
        Invoke("Visible", 0.1f);
    }

    private void Update()
    {
        if (!m_IsDead)
            transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), m_TurnSpeed);
    }
    
    public void TakeDamage(Element attacking, float amount)
    {
        if (m_IsDead)
        {
            return;
        }


        m_CurrentHP -= amount * Damage.GetModifier(attacking, m_Element);
        float ratio = m_CurrentHP / m_MaxHP;
        m_HPBar.UpdateBar(ratio);
        audioSource.PlayOneShot(m_DamageTaken, 0.15f);

        if (m_ChangeElementOnHP)
        {
            if (ratio > 0.75f)
            {
                ChangeElement(Element.Fire);
            }
            else if (ratio > 0.5f)
            {
                ChangeElement(Element.Wind);
            }
            else if (ratio > 0.25f)
            {
                ChangeElement(Element.Lightning);
            }
            else if (ratio > 0.0f)
            {
                ChangeElement(Element.Ice);
            }
        }

        if (m_CurrentHP <= 0.0f)
        {
            m_IsDead = true;
            OnDeath.Invoke();
            this.gameObject.SetActive(false);
        }
    }

    public void ChangeElement(Element newElement)
    {
        m_Element = newElement;
        m_Body.material.SetColor("_BaseColor", m_BodyColors[(int)m_Element]);
        m_Core.material.SetColor("_BaseColor", m_CoreColors[(int)m_Element]);

        AssetBundle bundle = assetBundleManager.LoadBundle("uibundle");

        Sprite[] icons = bundle.LoadAssetWithSubAssets<Sprite>("T_Enemy_Element");
        m_ElementIcon.sprite = Array.Find<Sprite>(icons, item => item.name == "T_Enemy_Element_" + m_Element.ToString());

    }

    public void ResetScore()
    {
        m_Score = 0;
    }

    private IEnumerator ChangeElementOnTime()
    {
        while (!m_IsDead)
        {
            ChangeElement((Element)UnityEngine.Random.Range(0, 4));
            yield return new WaitForSeconds(5.0f);
        }
        yield break;
    }

    private void Visible()
    {
        m_Body.enabled = true;
        m_Core.enabled = true;
    }
}
