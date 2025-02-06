using UnityEngine;

namespace DivisionLike
{
    /// <summary>
    /// 적 관리자
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        public GameObject m_Enemy; // The enemy prefab to be spawned.
        public float m_SpawnTime = 3f; // How long between each spawn.
        public Transform[] m_SpawnPoints; // An array of the spawn points this enemy can spawn from.
        public bool m_IsStartToSpawn = true;

        void Start()
        {
            if (m_IsStartToSpawn == true)
            {
                StartToSpawn();
            }
        }

        /// <summary>
        /// 일정 시간 반복해서 적을 생성한다.
        /// </summary>
        private void StartToSpawn()
        {
            // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
            InvokeRepeating("Spawn", m_SpawnTime, m_SpawnTime);
        }

        /// <summary>
        /// 적을 생성한다.
        /// </summary>
        void Spawn()
        {
            // If the player has no health left...
            if (Player.instance.m_Stats.m_CurrentHealth <= 0f)
            {
                // ... exit the function.
                return;
            }

            // Find a random index between zero and one less than the number of spawn points.
            int spawnPointIndex = Random.Range(0, m_SpawnPoints.Length);

            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
            //Instantiate( enemy, spawnPoints[ spawnPointIndex ].position, spawnPoints[ spawnPointIndex ].rotation );
            GameObject newEnemy = Lean.LeanPool.Spawn(m_Enemy, m_SpawnPoints[spawnPointIndex].position,
                m_SpawnPoints[spawnPointIndex].rotation);
            newEnemy.transform.SetParent(this.transform);
        }
    }
}