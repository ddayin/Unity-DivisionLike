/*
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/40756
 */

using UnityEngine;

namespace DivisionLike
{
    public class EnemyManager : MonoBehaviour
    {
        public GameObject _enemy;                // The enemy prefab to be spawned.
        public float _spawnTime = 3f;            // How long between each spawn.
        public Transform[] _spawnPoints;         // An array of the spawn points this enemy can spawn from.
        public bool _isStartToSpawn = true;

        void Start()
        {
            if ( _isStartToSpawn == true )
            {
                StartToSpawn();
            }
        }

        private void StartToSpawn()
        {
            // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
            InvokeRepeating( "Spawn", _spawnTime, _spawnTime );
        }

        void Spawn()
        {
            // If the player has no health left...
            if ( Player.instance._health._currentHealth <= 0f )
            {
                // ... exit the function.
                return;
            }

            // Find a random index between zero and one less than the number of spawn points.
            int spawnPointIndex = Random.Range( 0, _spawnPoints.Length );

            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
            //Instantiate( enemy, spawnPoints[ spawnPointIndex ].position, spawnPoints[ spawnPointIndex ].rotation );
            GameObject newEnemy = Lean.LeanPool.Spawn( _enemy, _spawnPoints[ spawnPointIndex ].position, _spawnPoints[ spawnPointIndex ].rotation );
            newEnemy.transform.SetParent( this.transform );
        }
    }
}