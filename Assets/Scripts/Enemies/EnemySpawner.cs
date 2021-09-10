using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner: MonoBehaviour
{
    [System.Serializable]
    public class EnemyWave
    {
        [SerializeField] private GameObject m_EnemyToSpawn;
        [Range(1, 10), SerializeField] private int m_Amount;
        private List<GameObject> m_Enemies = new List<GameObject>();
        public UnityEvent OnWaveFinished = new UnityEvent();

        public void Instantiate(Transform parent)
        {
            m_EnemyToSpawn.SetActive(false);
            for (int i = 0; i < m_Amount; i++)
            {
                GameObject enemy = GameObject.Instantiate(m_EnemyToSpawn, parent);
                enemy.transform.localPosition = new Vector3(Mathf.Sin(i * 15.0f * (Random.Range(-1, 1) >= 0 ? 1 : -1)) * 15.0f, Mathf.Sin(i * 15.0f) * 5.0f, Random.Range(-1.0f, 1.0f));
                m_Enemies.Add(enemy);
            }
            m_EnemyToSpawn.SetActive(true);
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
    private EnemyWave m_CurrentWave;

    private void Start()
    {
        for (int i = 0; i < m_Waves.Count; i++)
        {
            m_Waves[i].Instantiate(this.transform);
            m_Waves[i].OnWaveFinished.AddListener(delegate { m_CurrentWave = m_Waves[Mathf.Min(i + 1, m_Waves.Count - 1)]; m_CurrentWave.SpawnWave(); });
        }
        m_CurrentWave = m_Waves[0];
        m_CurrentWave.SpawnWave();
    }
}
