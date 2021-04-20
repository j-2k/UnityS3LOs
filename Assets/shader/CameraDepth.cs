using UnityEngine;

public class CameraDepth : MonoBehaviour
{
    [SerializeField]
    DepthTextureMode depthTextureMode;

    void OnValidate()
    {
        SetCameraDepthTextureMode();
    }

    void Awake()
    {
        SetCameraDepthTextureMode();
    }

    void SetCameraDepthTextureMode()
    {
        GetComponent<Camera>().depthTextureMode = depthTextureMode;
    }
}