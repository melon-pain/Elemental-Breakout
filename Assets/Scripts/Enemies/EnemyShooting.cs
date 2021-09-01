using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Element m_Element;
    [Header("Projectile")]
    [SerializeField] private ParticleSystem m_Projectiles;
    [SerializeField] private float m_Damage = 10.0f;

    [Header("Animator")]
    [SerializeField] private Animator m_Animator;

    protected Enemy self;

    private void Start()
    {
        self = this.GetComponentInParent<Enemy>();
        m_Element = self.element;
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (!self.isDead)
        {
            m_Projectiles.Stop();
            m_Animator.SetBool("IsShooting", false);
            yield return new WaitForSeconds(Random.Range(2.0f, 4.0f));

            if (self.isDead)
                break;
            m_Projectiles.Play();
            m_Animator.SetBool("IsShooting", true);
            yield return new WaitForSeconds(Random.Range(2.0f, 4.0f));
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
