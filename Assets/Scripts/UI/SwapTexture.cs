using UnityEngine;

/// <summary>
/// Swap texture
/// </summary>
public class SwapTexture : MonoBehaviour {

    public GameObject target;
    private MapGenerator generator;

    public void Start() {
        generator = target.GetComponent<MapGenerator>();
    }

    public void DoSwap() {
        SatelliteImageService.ToggleUseSatelliteImage();
        generator.UpdateTextures();
    }

}
