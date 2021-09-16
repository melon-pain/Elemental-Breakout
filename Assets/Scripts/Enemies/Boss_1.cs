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
    [SerializeField] private Image m_HPBarRed;
    [SerializeField] private Image m_HPBarOrange;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        m_CurrentHP = m_MaxHP;
        StartCoroutine(Shoot());

        AssetBundle bundle = assetBundleManager.LoadBundle("uibundle");
        m_HPBarIcon.sprite = bundle.LoadAsset<Sprite>("T_Bar");
        assetBundleManager.UnloadBundle("uibundle");
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
            yield return new WaitForSeconds(chargeTime);
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
