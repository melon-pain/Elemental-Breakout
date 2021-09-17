using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Boss_1 : MonoBehaviour
{
    [SerializeField] private float m_MaxHP = 1000.0f;
    [ReadOnlyInInspector, SerializeField] private float m_CurrentHP = 1000.0f;
    [SerializeField] private Bar m_HPBar;

    [SerializeField] private ParticleSystem m_Projectiles;

    [Header("Score")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private int m_Score = 1000;

    [SerializeField] private Animator m_Animator;
    private bool m_IsDead = false;
    public UnityEvent OnDeath = new UnityEvent();

    private bool isShooting = false;
    private Player player;

    [Header("UI")]
    [SerializeField] private AssetBundleManager assetBundleManager;
    [SerializeField] private Image m_HPBarIcon;
    [SerializeField] private Image m_HPBarRed;
    [SerializeField] private Image m_HPBarOrange;

    [Header("Audio")]
    [SerializeField] private AudioClip m_Shoot;
    [SerializeField] private AudioClip m_DamageTaken;

    private AudioSource audioSource;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        m_CurrentHP = m_MaxHP;
        audioSource = this.GetComponent<AudioSource>();

        StartCoroutine(Shoot());

        AssetBundle bundle = assetBundleManager.LoadBundle("uibundle");
        m_HPBarIcon.sprite = bundle.LoadAsset<Sprite>("T_Bar");
        m_HPBarRed.sprite = bundle.LoadAsset<Sprite>("T_Bar");
        m_HPBarOrange.sprite = bundle.LoadAsset<Sprite>("T_Bar");
        levelManager = FindObjectOfType<LevelManager>();

        OnDeath.AddListener(player.GetComponentInChildren<PlayerShooting>().RemoveLockOn);
        OnDeath.AddListener(delegate { levelManager.AddScore(m_Score); });
        OnDeath.AddListener(delegate { levelManager.GameOver(true); });

        transform.LookAt(player.transform);
    }

    private void Update()
    {
        if (!m_IsDead && isShooting)
        {
            transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), 1.0f);
        }
    }

    private IEnumerator Shoot()
    {
        while (!m_IsDead)
        {
            float chargeTime = Random.Range(2.0f, 5.0f);
            isShooting = false;
            m_Projectiles.Stop();
            m_Animator.SetBool("IsShooting", false);
            yield return new WaitForSeconds(chargeTime);
            isShooting = true;
            m_Projectiles.Play();
            m_Animator.SetBool("IsShooting", true);
            audioSource.PlayOneShot(m_Shoot, 0.25f);
            yield return new WaitForSeconds(chargeTime);
        }
    }

    public void TakeDamage(float amount)
    {
        m_CurrentHP -= amount;
        m_HPBar.UpdateBar(m_CurrentHP / m_MaxHP);
        audioSource.PlayOneShot(m_DamageTaken, 0.15f);
        if (m_CurrentHP <= 0.0f)
        {
            m_IsDead = true;
            OnDeath.Invoke();
            Destroy(this.gameObject);
        }
    }
}
