using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DivisionLike
{
    /// <summary>
    /// 플레이어의 HP
    /// </summary>
    public class PlayerHealth : MonoBehaviour
    {
        public Image m_DamageImage; // Reference to an image to flash on the screen on being hurt.
        public Image m_BloodImage;
        public AudioClip m_DeathClip; // The audio clip to play when the player dies.
        public float m_FlashSpeed = 5f; // The speed the damageImage will fade at.
        public Color m_FlashColour = new Color(1f, 0f, 0f, 0.1f); // The colour the damageImage is set to, to flash.

        private Animator m_Animator; // Reference to the Animator component.
        private AudioSource m_PlayerAudio; // Reference to the AudioSource component.
        private bool m_IsDead = false; // Whether the player is dead.
        private bool m_Damaged = false; // True when the player gets damaged

        #region MonoBeviour

        void Awake()
        {
            // Setting up the references.
            m_Animator = transform.GetComponent<Animator>();
            m_PlayerAudio = transform.GetComponent<AudioSource>();

            if (m_BloodImage != null)
            {
                m_BloodImage.gameObject.SetActive(false);
            }
        }

        void Update()
        {
            UpdateDamageImage();

            // Reset the damaged flag.
            m_Damaged = false;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void UpdateDamageImage()
        {
            if (m_DamageImage == null) return;
            if (m_BloodImage == null) return;

            // HP가 한 칸도 남지 않았을 때에는 계속 빨간색 이미지로 전체 화면을 덮는다
            if (CalculateCurrentDivide() == 0)
            {
                m_DamageImage.color = m_FlashColour;
                m_BloodImage.gameObject.SetActive(true);
            }
            else
            {
                // If the player has just been damaged...
                if (m_Damaged == true)
                {
                    // ... set the colour of the damageImage to the flash colour.
                    m_DamageImage.color = m_FlashColour;
                }
                // Otherwise...
                else
                {
                    // ... transition the colour back to clear.
                    m_DamageImage.color = Color.Lerp(m_DamageImage.color, Color.clear, m_FlashSpeed * Time.deltaTime);
                }

                m_BloodImage.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 플레이어가 지정된 양만큼의 피해를 입는다.
        /// </summary>
        /// <param name="attackDirection"></param>
        /// <param name="amount"></param>
        public void TakeDamage(Vector3 attackDirection, int amount)
        {
            // Set the damaged flag so the screen will flash.
            m_Damaged = true;

            EZCameraShake.CameraShaker.Instance.ShakeOnce(2f, 2f, 0.1f, 0.1f);

            // Reduce the current health by the damage amount.
            Player.instance.m_Stats.m_CurrentHealth -= amount;

            // Set the health bar's value to the current health.           
            PlayerHUD.instance.SetHealthSlider(Player.instance.m_Stats.m_CurrentHealth);

            ScreenHUD.instance.RotateCircularHit(attackDirection);
            ScreenHUD.instance.RotateMinimapHit(attackDirection);

            // PlayPaid the hurt sound effect.
            m_PlayerAudio.Play();

            // If the player has lost all it's health and the death flag hasn't been set yet...
            if (Player.instance.m_Stats.m_CurrentHealth <= 0 && m_IsDead == false)
            {
                // ... it should die.
                Death();
            }
        }

        /// <summary>
        /// 최대치로 HP를 회복한다.
        /// </summary>
        public void RecoverMax()
        {
            Player.instance.m_Stats.m_CurrentHealth = Player.instance.m_Stats.m_MaxHealth;

            PlayerHUD.instance.SetMaxHealthSlider();

            PlayerHUD.instance.SetMedikitText();

            Player.instance.m_OutlineEffect.Enable(4f);
        }

        /// <summary>
        /// 하나의 셀 만큼의 HP를 회복한다.
        /// </summary>
        public void RecoverOneCell()
        {
            float toDivide = (float)Player.instance.m_Stats.m_MaxHealth / 3f;
            float fDivided = (float)Player.instance.m_Stats.m_CurrentHealth / toDivide;
            int iDivided = (int)fDivided;

            Player.instance.m_Stats.m_CurrentHealth = (int)toDivide * (iDivided + 1);


            PlayerHUD.instance.SetHealthSlider(Player.instance.m_Stats.m_CurrentHealth);

            PlayerHUD.instance.SetMedikitText();

            Player.instance.m_OutlineEffect.Enable(4f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int CalculateCurrentDivide()
        {
            float toDivide = (float)Player.instance.m_Stats.m_MaxHealth / 3f;
            float fDivided = (float)Player.instance.m_Stats.m_CurrentHealth / toDivide;
            return (int)fDivided;
        }

        /// <summary>
        /// 죽음
        /// </summary>
        void Death()
        {
            // Set the death flag so this function won't be called again.
            m_IsDead = true;

            // Turn off any remaining shooting effects.
            //playerShooting.DisableEffects ();

            // Tell the animator that the player is dead.
            //anim.SetTrigger ("Die");

            // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
            m_PlayerAudio.clip = m_DeathClip;
            m_PlayerAudio.Play();

            // Turn off the movement and shooting scripts.
            //playerMovement.enabled = false;
            //playerShooting.enabled = false;
        }

        /// <summary>
        /// 게임을 재시작한다.
        /// </summary>
        public void RestartLevel()
        {
            // Reload the level that is currently loaded.
        }
    }
}