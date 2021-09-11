using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Boss_1 : MonoBehaviour
{
    [SerializeField] private float m_MaxHP = 1000.0f;
    [ReadOnlyInInspector, SerializeField] private float m_CurrentHP = 1000.0f;
    [SerializeField] private Bar m_HPBar;

    [SerializeField] private ParticleSystem m_Projectiles;

    [SerializeField] private Animator m_Animator;
    private bool m_IsDead = false;
    public UnityEvent OnDeath = new UnityEvent();

    private bool isShooting = false;
    private Player player;

    [Header("UI")]
    [SerializeField] private AssetBundleManager assetBundleManager;
    [SerializeField] private Image m_HPBarIcon;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        m_CurrentHP = m_MaxHP;
        StartCoroutine(Shoot());
    }

    private void Update()
    {
        if (!m_IsDead && !isShooting)
        {
            transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), 1.0f);
        }
    }

    private IEnumerator Shoot()
    {
        while (!m_IsDead)
        {
            isShooting = false;
            m_Projectiles.Stop();
            yield return new WaitForSeconds(Random.Range(4.0f, 8.0f));
            isShooting = true;
            m_Projectiles.Play();
            yield return new WaitForSeconds(Random.Range(2.0f, 4.0f));
        }
    }

    public void TakeDamage(float amount)
    {
        m_CurrentHP -= amount;
        m_HPBar.UpdateBar(m_CurrentHP / m_MaxHP);

        if (m_CurrentHP <= 0.0f)
        {
            m_IsDead = true;
            OnDeath.Invoke();
            Destroy(this.gameObject);
        }
    }
}
