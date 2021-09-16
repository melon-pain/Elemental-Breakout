using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner: MonoBehaviour
{
    [System.Serializable]
    public class EnemyWave
    {
        [SerializeField] private List<GameObject> m_EnemiesToSpawn = new List<GameObject>();
        [ReadOnlyInInspector, SerializeField] private int m_Amount;
        private List<GameObject> m_Enemies = new List<GameObject>();
        public UnityEvent OnWaveFinished = new UnityEvent();

        public void Instantiate(Transform parent)
        {
            for (int i = 0; i < m_EnemiesToSpawn.Count; i++)
            {
                m_EnemiesToSpawn[i].SetActive(false);
                GameObject enemy = GameObject.Instantiate(m_EnemiesToSpawn[i], parent);
                enemy.transform.localPosition = new Vector3(Mathf.Sin(i * 15.0f * (Random.Range(-1, 1) >= 0 ? 1 : -1)) * 20.0f, Mathf.Sin(i * 15.0f) * 5.0f, Random.Range(-1.0f, 1.0f));
                m_Enemies.Add(enemy);
                m_EnemiesToSpawn[i].SetActive(true);
            }

            m_Amount = m_EnemiesToSpawn.Count;
        }

        public void SpawnWave()
        {
            foreach (GameObject enemy in m_Enemies)
            {
                enemy.SetActive(true);
                enemy.GetComponent<Enemy>().OnDeath.AddListener(delegate { m_Amount--; CheckWave(); });
            }
        }

        private void CheckWave()
        {
            if (m_Amount <= 0)
            {
                OnWaveFinished.Invoke();
            }
        }
    }

    [SerializeField] private List<EnemyWave> m_Waves = new List<EnemyWave>();
    private int m_CurrentWave = 0;

    private void Start()
    {
        for (int i = 0; i < m_Waves.Count; i++)
        {
            m_Waves[i].Instantiate(this.transform);
            if (i < m_Waves.Count - 1)
                m_Waves[i].OnWaveFinished.AddListener(delegate { NextWave(); });
        }
        m_Waves[m_CurrentWave].SpawnWave();
    }

    private void NextWave()
    {
        m_CurrentWave++;
        m_CurrentWave = Mathf.Min(m_CurrentWave, m_Waves.Count - 1);
        m_Waves[m_CurrentWave].SpawnWave();
    }
}
