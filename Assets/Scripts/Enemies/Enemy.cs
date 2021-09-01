using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IDamage
{
    public Element element { get => m_Element; }
    [SerializeField] protected Element m_Element;
    [ReadOnlyInInspector] [SerializeField] protected float m_HP = 100.0f;
    public bool isDead { get => m_IsDead; }
    protected bool m_IsDead = false;
    public UnityEvent OnDeath = new UnityEvent();

    private Player player;
    private Vector3 currentRotation = Vector3.zero;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        transform.LookAt(player.transform);
        OnDeath.AddListener(player.GetComponentInChildren<PlayerShooting>().RemoveLockOn);
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

        if (Damage.GetWeakness(m_Element) == attacking)
            m_HP -= amount * 1.5f;
        else if (m_Element == attacking)
            m_HP -= amount * 0.5f;
        else
            m_HP -= amount;

        if (m_HP <= 0.0f)
        {
            m_IsDead = true;
            OnDeath.Invoke();
            Destroy(this.gameObject);
        }
    }
}
