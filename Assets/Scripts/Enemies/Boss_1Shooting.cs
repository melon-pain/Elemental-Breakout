using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1Shooting : MonoBehaviour
{
    [SerializeField] private float m_Damage = 10.0f;
    private void OnParticleCollision(GameObject other)
    {
        IDamage damage = other.GetComponent<IDamage>();
        damage.TakeDamage((Element)Random.Range(0, 4), m_Damage);
    }
}
