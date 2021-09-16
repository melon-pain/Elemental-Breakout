using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemySpawnVFX : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [GradientUsage(true)] [SerializeField] private List<Gradient> m_Gradients = new List<Gradient>();
    [SerializeField] private VisualEffect m_VFX;

    private void Start()
    {
        m_VFX.SetGradient("Gradient", m_Gradients[(int)enemy.element]);
        Destroy(this);
    }
}
