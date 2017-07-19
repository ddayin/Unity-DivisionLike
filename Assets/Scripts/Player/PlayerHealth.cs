/*
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/40756
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DivisionLike
{
    public class PlayerHealth : MonoBehaviour
    {
        //public Slider _healthSlider;                                 // Reference to the UI's health bar.
        public Image _damageImage;                                   // Reference to an image to flash on the screen on being hurt.
        public AudioClip _deathClip;                                 // The audio clip to play when the player dies.
        public float _flashSpeed = 5f;                               // The speed the damageImage will fade at.
        public Color _flashColour = new Color( 1f, 0f, 0f, 0.1f );     // The colour the damageImage is set to, to flash.
        
        private Animator _anim;                                              // Reference to the Animator component.
        private AudioSource _playerAudio;                                    // Reference to the AudioSource component.
        //PlayerMovement playerMovement;                              // Reference to the player's movement.
        //PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.
        private bool _isDead = false;                                                // Whether the player is dead.
        private bool _damaged = false;                                               // True when the player gets damaged


        void Awake()
        {
            // Setting up the references.
            _anim = transform.GetComponent<Animator>();
            _playerAudio = transform.GetComponent<AudioSource>();
            //playerMovement = GetComponent <PlayerMovement> ();
            //playerShooting = GetComponentInChildren <PlayerShooting> ();

            
        }


        void Update()
        {
            // If the player has just been damaged...
            if ( _damaged == true )
            {
                // ... set the colour of the damageImage to the flash colour.
                _damageImage.color = _flashColour;
            }
            // Otherwise...
            else
            {
                // ... transition the colour back to clear.
                _damageImage.color = Color.Lerp( _damageImage.color, Color.clear, _flashSpeed * Time.deltaTime );
            }

            // Reset the damaged flag.
            _damaged = false;
        }


        public void TakeDamage( int amount )
        {
            // Set the damaged flag so the screen will flash.
            _damaged = true;

            // Reduce the current health by the damage amount.
            Player.instance._stats._currentHealth -= amount;

            // Set the health bar's value to the current health.           
            PlayerHUD.instance.SetHealthSlider( Player.instance._stats._currentHealth );

            // Play the hurt sound effect.
            _playerAudio.Play();

            // If the player has lost all it's health and the death flag hasn't been set yet...
            if ( Player.instance._stats._currentHealth <= 0 && _isDead == false )
            {
                // ... it should die.
                Death();
            }
        }

        public void RecoverMax()
        {
            Player.instance._stats._currentHealth = Player.instance._stats._maxHealth;
            
            PlayerHUD.instance.SetMaxHealthSlider();

            PlayerHUD.instance.SetMedikitText();

            Player.instance._outlineEffect.Enable( 4f );
        }

        public void RecoverOneCell()
        {
            float toDivide = (float) Player.instance._stats._maxHealth / 3f;
            float fDivided = (float) Player.instance._stats._currentHealth / toDivide;
            int iDivided = (int) fDivided;
            //if ( iDivided == fDivided && iDivided != 3 )
            //{
            //    Player.instance._stats._currentHealth = (int) toDivide * ( iDivided + 2 );
            //}
            //else
            {
                Player.instance._stats._currentHealth = (int) toDivide * ( iDivided + 1 );
            }
            

            PlayerHUD.instance.SetHealthSlider( Player.instance._stats._currentHealth );

            PlayerHUD.instance.SetMedikitText();

            Player.instance._outlineEffect.Enable( 4f );
        }


        void Death()
        {
            // Set the death flag so this function won't be called again.
            _isDead = true;

            // Turn off any remaining shooting effects.
            //playerShooting.DisableEffects ();

            // Tell the animator that the player is dead.
            //anim.SetTrigger ("Die");

            // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
            _playerAudio.clip = _deathClip;
            _playerAudio.Play();

            // Turn off the movement and shooting scripts.
            //playerMovement.enabled = false;
            //playerShooting.enabled = false;
        }


        public void RestartLevel()
        {
            // Reload the level that is currently loaded.
            SceneManager.LoadScene( 0 );
        }
    }
}