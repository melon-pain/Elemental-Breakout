using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlate : MonoBehaviour, IDamage
{
    [SerializeField] private Boss_1 boss;
    [SerializeField] private Element m_Element;

    public void TakeDamage(Element attacking, float amount)
    {
        boss.TakeDamage(amount * Damage.GetModifier(attacking, m_Element));
    }
}
