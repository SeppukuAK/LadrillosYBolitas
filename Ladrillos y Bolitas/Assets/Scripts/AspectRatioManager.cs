using UnityEngine;

public class AspectRatioManager : MonoBehaviour {

    private const float MAGIC = 6.1875F;

    private void Awake()
    {
        AdjustCameraToAspectRatio();
    }

    private void AdjustCameraToAspectRatio()
    {
        float aspectRatio = (float)Screen.height / Screen.width;
        Camera.main.orthographicSize = aspectRatio * MAGIC;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
