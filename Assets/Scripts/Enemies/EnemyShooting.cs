using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyShooting : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private ParticleSystem m_Projectiles;
    [SerializeField] private float m_Damage = 5.0f;
    [SerializeField] private List<Color> m_ProjectileColors = new List<Color>();
    [Header("Animator")]
    [SerializeField] private Animator m_Animator;

    [SerializeField] private Enemy enemy;

    [Header("Audio")]
    [SerializeField] private UnityEvent OnShoot;
    [SerializeField] private UnityEvent OnDamageTaken;
    private void Start()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (!enemy.isDead)
        {
            float chargeTime = Random.Range(2.0f, 4.0f);
            m_Projectiles.Stop();
            if (m_Animator)
                m_Animator.SetBool("IsShooting", false);
            yield return new WaitForSeconds(chargeTime);

            if (enemy.isDead)
                break;
            var main = m_Projectiles.main;
            main.startColor = m_ProjectileColors[(int)enemy.element];
            m_Projectiles.Play();
            if (m_Animator)
            {
                m_Animator.SetBool("IsShooting", true);
                OnShoot.Invoke();
            }
            yield return new WaitForSeconds(chargeTime);
        }
        yield break;
    }

    private void OnParticleCollision(GameObject other)
    {
        IDamage damage = other.GetComponent<IDamage>();
        if (damage != null)
            damage.TakeDamage(enemy.element, m_Damage);
        else
        {
            Debug.Log("No damage?");
        }
    }
}
