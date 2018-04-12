// reference - http://wergia.tistory.com/59


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public Loading m_LoadingAni;

    private void Awake()
    {
        if ( m_LoadingAni == null ) return;
        
        StartCoroutine( LoadScene() );
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync( "Play" );
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while ( op.isDone == false )
        {
            yield return null;

            timer += Time.deltaTime;

            if ( op.progress >= 0.9f )
            {
                float progress = Mathf.Lerp( op.progress, 1f, timer );
                float progressDegree = Mathf.Lerp( 0f, 360f, progress );
                m_LoadingAni.RotateZ( progressDegree );

                if ( progress == 1.0f )
                {
                    op.allowSceneActivation = true;
                }

            }
            else
            {
                float progress = Mathf.Lerp( op.progress, op.progress, timer );
                float progressDegree = Mathf.Lerp( 0f, 360f, progress );
                m_LoadingAni.RotateZ( progressDegree );
            }


            

            
        }
    }
}
