using UnityEngine;

public class SwapTexture : MonoBehaviour {

    public GameObject target;
    private MapGenerator generator;

    public void Start() {
        generator = target.GetComponent<MapGenerator>();
    }

    public void DoSwap() {
        SatelliteImageService.getSatelliteImage().drawSatelliteImage = ! SatelliteImageService.getSatelliteImage().drawSatelliteImage;
        generator.UpdateTextures();
    }

}
