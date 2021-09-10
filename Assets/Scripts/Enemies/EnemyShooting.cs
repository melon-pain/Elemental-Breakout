using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Element m_Element;
    [Header("Projectile")]
    [SerializeField] private ParticleSystem m_Projectiles;
    [SerializeField] private float m_Damage = 5.0f;
    [SerializeField] private List<Color> m_ProjectileColors = new List<Color>();
    [Header("Animator")]
    [SerializeField] private Animator m_Animator;

    protected Enemy enemy;

    private void Start()
    {
        enemy = this.GetComponentInParent<Enemy>();
        m_Element = enemy.element;
        var main = m_Projectiles.main;
        main.startColor = m_ProjectileColors[(int)m_Element];
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (!enemy.isDead)
        {
            m_Projectiles.Stop();
            m_Animator.SetBool("IsShooting", false);
            yield return new WaitForSeconds(Random.Range(2.0f, 4.0f));

            if (enemy.isDead)
                break;
            m_Projectiles.Play();
            m_Animator.SetBool("IsShooting", true);
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
        }
        yield break;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            player.TakeDamage(m_Element, m_Damage);
        }
    }
}
