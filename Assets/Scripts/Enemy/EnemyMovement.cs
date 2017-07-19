/*
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/40756
 */

using UnityEngine;
using System.Collections;

namespace DivisionLike
{
    public class EnemyMovement : MonoBehaviour
    {
        private Transform _player;               // Reference to the player's position.
        private PlayerHealth _playerHealth;      // Reference to the player's health.
        private EnemyHealth _enemyHealth;        // Reference to this enemy's health.
        private UnityEngine.AI.NavMeshAgent _nav;               // Reference to the nav mesh agent.


        void Awake()
        {
            // Set up the references.
            _player = GameObject.FindGameObjectWithTag( "Player" ).transform;
            _playerHealth = _player.GetComponent<PlayerHealth>();
            _enemyHealth = transform.GetComponent<EnemyHealth>();
            _nav = transform.GetComponent<UnityEngine.AI.NavMeshAgent>();
        }


        void Update()
        {
            // If the enemy and the player have health left...
            if ( _enemyHealth._currentHealth > 0 && Player.instance._stats._currentHealth > 0 )
            {
                // ... set the destination of the nav mesh agent to the player.
                _nav.SetDestination( _player.position );
            }
            // Otherwise...
            else
            {
                // ... disable the nav mesh agent.
                _nav.enabled = false;
            }
        }
    }
}