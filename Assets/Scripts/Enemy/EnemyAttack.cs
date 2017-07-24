/*
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/40756
 */

using UnityEngine;
using System.Collections;

namespace DivisionLike
{
    public class EnemyAttack : MonoBehaviour
    {
        public float _timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
        public int _attackDamage = 10;               // The amount of health taken away per attack.


        private Animator _anim;                              // Reference to the animator component.
        private GameObject _player;                          // Reference to the player GameObject.
        private PlayerHealth _playerHealth;                  // Reference to the player's health.
        private EnemyHealth _enemyHealth;                    // Reference to this enemy's health.
        private bool _playerInRange;                         // Whether player is within the trigger collider and can be attacked.
        private float _timer;                                // Timer for counting up to the next attack.


        void Awake()
        {
            // Setting up the references.
            _player = GameObject.FindGameObjectWithTag( "Player" );
            _playerHealth = _player.GetComponent<PlayerHealth>();
            _enemyHealth = transform.GetComponent<EnemyHealth>();
            _anim = transform.GetComponent<Animator>();
        }


        void OnTriggerEnter( Collider other )
        {
            // If the entering collider is the player...
            if ( other.gameObject == _player )
            {
                // ... the player is in range.
                _playerInRange = true;
            }
        }


        void OnTriggerExit( Collider other )
        {
            // If the exiting collider is the player...
            if ( other.gameObject == _player )
            {
                // ... the player is no longer in range.
                _playerInRange = false;
            }
        }


        void Update()
        {
            // Add the time since Update was last called to the timer.
            _timer += Time.deltaTime;

            // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
            if ( _timer >= _timeBetweenAttacks && _playerInRange == true && _enemyHealth._currentHealth > 0 )
            {
                // ... attack.
                Attack();
            }

            // If the player has zero or less health...
            if ( Player.instance._stats._currentHealth <= 0 )
            {
                // ... tell the animator the player is dead.
                _anim.SetTrigger( "PlayerDead" );
            }
        }


        void Attack()
        {
            // Reset the timer.
            _timer = 0f;

            // If the player has health to lose...
            if ( Player.instance._stats._currentHealth > 0 )
            {
                // ... damage the player.
                _playerHealth.TakeDamage( transform.forward, _attackDamage );
            }
        }
    }
}