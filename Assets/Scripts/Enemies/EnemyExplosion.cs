using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class EnemyExplosion : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [Header("Projectile")]
    [SerializeField] private ParticleSystem m_Projectiles;
    [SerializeField] private float m_Damage = 25.0f;
    [SerializeField] private List<Color> m_ProjectileColors = new List<Color>();
    [SerializeField] private Animator m_Animator;

    public UnityEvent OnExplode = new UnityEvent();
    private AudioSource audioSource;

    private void Start()
    {
        var main = m_Projectiles.main;
        main.startColor = m_ProjectileColors[(int)enemy.element];
        audioSource = this.GetComponent<AudioSource>();
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(Random.Range(2.0f, 3.0f));
        m_Animator.SetTrigger("Explode");
        yield return new WaitForSeconds(1.0f);
        m_Projectiles.Play();
        audioSource.Play();
        OnExplode.Invoke();
        yield return new WaitForSeconds(2.0f);
        enemy.ResetScore();
        enemy.OnDeath.Invoke();
        enemy.gameObject.SetActive(false);
        Debug.Log("Explode");
        yield break;
    }
    private void OnParticleCollision(GameObject other)
    {
        IDamage damage = other.GetComponent<IDamage>();
        damage.TakeDamage(enemy.element, m_Damage);
    }
}
