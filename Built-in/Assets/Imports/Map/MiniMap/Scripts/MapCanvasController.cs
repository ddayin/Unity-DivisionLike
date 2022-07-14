/*
 * reference - https://github.com/Area730/MiniMap
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DivisionLike;

[AddComponentMenu("MiniMap/Map canvas controller")]
[RequireComponent(typeof(RectTransform))]
public class MapCanvasController : MonoBehaviour
{
    #region Singleton
    public static MapCanvasController Instance
    {
        get
        {
            if (!_instance)
            {
                MapCanvasController[] controllers = GameObject.FindObjectsOfType<MapCanvasController>();

                if (controllers.Length != 0)
                {
                    if (controllers.Length == 1)
                    {
                        _instance = controllers[0];
                    }
                    else
                    {
                        Debug.LogError("You have more than one MapCanvasController in the scene.");
                    }
                }
                else
                {
                    Debug.LogError("You should add Map prefab to your canvas");
                }


            }
           

            return _instance;
        }
    }

    private static MapCanvasController _instance;
    #endregion

    #region Public

    /* Transform of the player that will be shown in the center of the map
     */ 
    public Transform playerTransform;

    /* Distance from which the objects will be shown on the map
     * If objects are farer - they will be always on the border of the map or now shown at all
     */
    public float radarDistance = 10;

    /* If true - objects out of range (radarDistance) will be hidden
     * If false - objects out of range (radarDistance) will stick to the border of the map
     * */
    public bool hideOutOfRadius = true;

    /* Applied to objects that are out of range (radarDistance)
     * If true - the farer the object is, the more transparent it is
     * Used only when hideOutOfRadius == false
     * */
    public bool useOpacity = true;

    /* Objects that are farer than radar distance but closer than maxRadarDistance
     * will be shown on the border of the map.
     * Used only when hideOutOfRadius == false
     */
    public float maxRadarDistance = 10;

    /* Enables or disables the rotation of the map
     * If true - the map is rotated and the arrow is not
     * and vice versa
     */
    public bool rotateMap = false;

    /* Sets the scale of the radaDistance and maxRadarDistance
     */
    public float scale = 1.0f;

    /*Minimal opacity for the markers that are farther than radar distance
     */
    public float minimalOpacity = 0.3f;


    public InnerMap InnerMapComponent
    {
        get
        {
            return innerMap;
        }
    }

    public MarkerGroup MarkerGroup
    {
        get
        {
            return markerGroup;
        }
    }
    #endregion


    #region Private
    private RectTransform mapRect;
    private InnerMap innerMap;
    private MapArrow mapArrow;
    private MarkerGroup markerGroup;
    private float innerMapRadius;
    #endregion

    #region Unity Methods

    void Awake()
    {

        if (!playerTransform)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            //Debug.LogError("You must specify the player transform");
        }

        mapRect = GetComponent<RectTransform>();

        innerMap = GetComponentInChildren<InnerMap>();
        if (!innerMap)
        {
            Debug.LogError("InnerMap component is missing from children");
        }

        mapArrow = GetComponentInChildren<MapArrow>();
        if (!mapArrow)
        {
            Debug.LogError("MapArrow component is missing from children");
        }

        markerGroup = GetComponentInChildren<MarkerGroup>();
        if (!markerGroup)
        {
            Debug.LogError("MerkerGroup component is missing. It must be a child of InnerMap");
        }

        innerMapRadius = innerMap.getMapRadius();

    }

	void Update () 
    {
        if (!playerTransform)
        {
            //error was already fired in Awake()
            return;
        }
        if (rotateMap)
        {
            mapRect.rotation = Quaternion.Euler(new Vector3(0, 0, playerTransform.eulerAngles.y));
            mapArrow.rotate(Quaternion.identity);
        }
        else
        {
            mapArrow.rotate(Quaternion.Euler(new Vector3(0, 0, -playerTransform.eulerAngles.y)));
        }
    }

    #endregion

    #region Custom methods

    public void checkIn(MapMarker marker)
    {
        if (!playerTransform)
        {
            //error was already fired in Awake()
            return;
        }

        float scaledRadarDistance = radarDistance * scale;
        float scaledMaxRadarDistance = maxRadarDistance * scale;

        if (marker.isActive)
        {
            float distance = distanceToPlayer(marker.getPosition());
            float opacity = 1.0f;

            if (distance > scaledRadarDistance)
            {
                if (hideOutOfRadius)
                {
                    if (marker.isVisible()) 
                    { 
                        marker.hide(); 
                    }
                    return;
                }
                else
                {
                    if (distance > scaledMaxRadarDistance)
                    {
                        if (marker.isVisible()) 
                        { 
                            marker.hide(); 
                        }
                        return;
                    }
                    else
                    {
                        if (useOpacity) 
                        {
                            float opacityRange = scaledMaxRadarDistance - scaledRadarDistance;
                            if (opacityRange <= 0)
                            {
                                Debug.LogError("Max radar distance should be bigger than radar distance");
                                return;
                            }
                            else
                            {
                                float distanceDiff = distance - scaledRadarDistance;
                                opacity = 1 - (distanceDiff / opacityRange);

                                if (opacity < minimalOpacity)
                                {
                                    opacity = minimalOpacity;
                                }
                            }
                        }
                        distance = scaledRadarDistance;
                    }
                }
            }

            if (!marker.isVisible())
            {
                marker.show();
            }

            Vector3 posDif = marker.getPosition() - playerTransform.position;
            Vector3 newPos = new Vector3(posDif.x, posDif.z, 0);
            newPos.Normalize();

            float markerRadius = (marker.markerSize / 2);
            float newLen = (distance / scaledRadarDistance) * (innerMapRadius - markerRadius);

            newPos *= newLen;
            marker.setLocalPos(newPos);
            marker.setOpacity(opacity);
        }
        else
        {
            if (marker.isVisible())
            {
                marker.hide();
            }
        }
    }

    private float distanceToPlayer(Vector3 other)
    {
        
        return Vector2.Distance(new Vector2(playerTransform.position.x, playerTransform.position.z), new Vector2(other.x, other.z));
    }

    #endregion
}
