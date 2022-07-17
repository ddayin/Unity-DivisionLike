/*
 * reference - https://github.com/Area730/MiniMap
 */

using UnityEngine;
using UnityEngine.UI;

using System.Collections;

/// <summary>
/// 맵 상에 표시되는 마커
/// </summary>
[AddComponentMenu("MiniMap/Map marker")]
public class MapMarker : MonoBehaviour
{

    #region Public

    /* Sprite that will be shown on the map
     */
    public Sprite markerSprite;

    /* Size of the marker on the map (width & height)
     */
    public float markerSize = 6.5f;

    /* Enables or disables this marker on the map
     */
    public bool isActive = true;

    public Image MarkerImage
    {
        get
        {
            return markerImage;
        }
    }

    #endregion

    #region Private

    private Image markerImage;

    #endregion

    #region Unity methods

    void Start () {
        if (!markerSprite)
        {
            Debug.LogError(" Please, specify the marker sprite.");
        }
        GameObject markerImageObject = new GameObject("Marker");
        markerImageObject.AddComponent<Image>();
        MapCanvasController controller = MapCanvasController.Instance;
        if (!controller)
        {
            Destroy(gameObject);
            return;
        }
        markerImageObject.transform.SetParent(controller.MarkerGroup.MarkerGroupRect);
        markerImage = markerImageObject.GetComponent<Image>();
        markerImage.sprite = markerSprite;
        markerImage.rectTransform.localPosition = Vector3.zero;
        markerImage.rectTransform.localScale = Vector3.one;
        markerImage.rectTransform.sizeDelta = new Vector2(markerSize, markerSize);
        markerImage.gameObject.SetActive(false);
	}


	void Update () {
        MapCanvasController controller = MapCanvasController.Instance;
        if (!controller)
        {
            return;
        }
        MapCanvasController.Instance.checkIn(this);
        markerImage.rectTransform.rotation = Quaternion.identity;
	}

    void OnDestroy()
    {
        if (markerImage)
        {
            Destroy(markerImage.gameObject);
        }
    }

    #endregion

    #region Custom methods

    public void show()
    {
        markerImage.gameObject.SetActive(true);
    }

    public void hide()
    {
        markerImage.gameObject.SetActive(false);
    }

    public bool isVisible()
    {
        return markerImage.gameObject.activeSelf;
    }

    public Vector3 getPosition()
    {
        return gameObject.transform.position;
    }

    public void setLocalPos(Vector3 pos)
    {
        markerImage.rectTransform.localPosition = pos;

    }

    public void setOpacity(float opacity)
    {
        markerImage.color = new Color(1.0f, 1.0f, 1.0f, opacity);
    }

    #endregion
}
