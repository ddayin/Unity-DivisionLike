using UnityEngine;
using System.Collections;


namespace DivisionLike
{
    [System.Serializable]
    public class TextureType
    {
        public string name;
        public Texture[] textures;
        public AudioClip[] footstepSounds;
    }

    /// <summary>
    /// 플레이어의 발자국 소리
    /// </summary>
    public class PlayerFootstepSound : MonoBehaviour
    {
        public TextureType[] m_TextureTypes;
        public AudioSource m_AudioS;

        void Awake()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        void PlayFootstepSound()
        {
            RaycastHit hit;
            Vector3 start = transform.position + transform.up;
            Vector3 dir = Vector3.down;

            if (Physics.Raycast(start, dir, out hit, 1.3f))
            {
                if (hit.collider.GetComponent<MeshRenderer>())
                {
                    PlayMeshSound(hit.collider.GetComponent<MeshRenderer>());
                }
                else if (hit.collider.GetComponent<Terrain>())
                {
                    //PlayTerrainSound( hit.collider.GetComponent<Terrain>(), hit.point );
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="renderer"></param>
        void PlayMeshSound(MeshRenderer renderer)
        {
            if (m_AudioS == null)
            {
                Debug.LogError("PlayMeshSound -- We have no audio source to play the sound from.");
                return;
            }

            if (m_TextureTypes.Length > 0)
            {
                foreach (TextureType type in m_TextureTypes)
                {
                    if (type.footstepSounds.Length == 0)
                    {
                        return;
                    }

                    foreach (Texture tex in type.textures)
                    {
                        if (renderer.material.mainTexture == tex)
                        {
                            SoundController.instance.PlaySound(m_AudioS,
                                type.footstepSounds[Random.Range(0, type.footstepSounds.Length)], true, 1, 1.2f);
                        }
                    }
                }
            }
        }
    }
}