using UnityEngine;

public class TerrainTreeToggle : MonoBehaviour
{
    private void OnEnable()
    {
        var terrains = FindObjectsByType<Terrain>(FindObjectsSortMode.None);
        foreach (Terrain terrain in terrains)
        {
            terrain.drawTreesAndFoliage = false;
        }
    }

    private void OnDisable()
    {
        var terrains = FindObjectsByType<Terrain>(FindObjectsSortMode.None);
        foreach (Terrain terrain in terrains)
        {
            terrain.drawTreesAndFoliage = true;
        }
    }
}
