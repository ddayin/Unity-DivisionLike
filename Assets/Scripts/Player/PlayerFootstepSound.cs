/*
 * reference - https://www.youtube.com/watch?v=pEvUDorfQXA
 */


using UnityEngine;
using System.Collections;


namespace DivisionLike
{
    public class FootstepSound : MonoBehaviour
    {
        public TextureType[] m_TextureTypes;

        public AudioSource m_AudioS;

        SoundController m_SC;

        // Use this for initialization
        void Start()
        {
            GameObject check = GameObject.FindGameObjectWithTag( "Sound Controller" );

            if ( check != null )
            {
                m_SC = check.GetComponent<SoundController>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void PlayFootstepSound()
        {
            RaycastHit hit;
            Vector3 start = transform.position + transform.up;
            Vector3 dir = Vector3.down;

            if ( Physics.Raycast( start, dir, out hit, 1.3f ) )
            {
                if ( hit.collider.GetComponent<MeshRenderer>() )
                {
                    PlayMeshSound( hit.collider.GetComponent<MeshRenderer>() );
                }
                else if ( hit.collider.GetComponent<Terrain>() )
                {
                    //PlayTerrainSound( hit.collider.GetComponent<Terrain>(), hit.point );
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="renderer"></param>
        void PlayMeshSound( MeshRenderer renderer )
        {

            if ( m_AudioS == null )
            {
                Debug.LogError( "PlayMeshSound -- We have no audio source to play the sound from." );
                return;
            }

            if ( m_SC == null )
            {
                Debug.LogError( "PlayMeshSound -- No sound manager!!!" );
                return;
            }

            if ( m_TextureTypes.Length > 0 )
            {
                foreach ( TextureType type in m_TextureTypes )
                {

                    if ( type.footstepSounds.Length == 0 )
                    {
                        return;
                    }

                    foreach ( Texture tex in type.textures )
                    {
                        if ( renderer.material.mainTexture == tex )
                        {
                            m_SC.PlaySound( m_AudioS, type.footstepSounds[ Random.Range( 0, type.footstepSounds.Length ) ], true, 1, 1.2f );
                        }
                    }
                }
            }
        }

        /*
        void PlayTerrainSound( Terrain t, Vector3 hitPoint )
        {
            if ( audioS == null )
            {
                Debug.LogError( "PlayTerrainSound -- We have no audio source to play the sound from." );
                return;
            }

            if ( sc == null )
            {
                Debug.LogError( "PlayTerrainSound -- No sound manager!!!" );
                return;
            }

            if ( textureTypes.Length > 0 )
            {

                int textureIndex = TerrainSurface.GetMainTexture( hitPoint );

                foreach ( TextureType type in textureTypes )
                {

                    if ( type.footstepSounds.Length == 0 )
                    {
                        return;
                    }

                    foreach ( Texture tex in type.textures )
                    {
                        if ( t.terrainData.splatPrototypes[ textureIndex ].texture == tex )
                        {
                            sc.PlaySound( audioS, type.footstepSounds[ Random.Range( 0, type.footstepSounds.Length ) ], true, 1, 1.2f );
                        }
                    }
                }
            }
        }
        */
        
    }

    [System.Serializable]
    public class TextureType
    {
        public string name;
        public Texture[] textures;
        public AudioClip[] footstepSounds;
    }
}

