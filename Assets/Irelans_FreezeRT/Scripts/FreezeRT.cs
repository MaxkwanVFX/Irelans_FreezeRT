using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FreezeRT : MonoBehaviour
{
    public Camera camera;
    public RenderTexture renderTexture;
    public GameObject CaptureSphere;

    public Material TargetSkyboxMaterial;
    void Start()
    {
        if (camera == null)
        {
            camera = Camera.main;
        }
        if (renderTexture == null)
        {
            Debug.LogError("RenderTexture not assigned");
            return;
        }
        if (CaptureSphere == null)
        {
            Debug.LogError("CaptureSphere not assigned");
            return;
        }
        CaptureSphere.SetActive(false);

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnFreezeRT();
        }

    }
    void OnDestroy()
    {
        renderTexture.Release();

        Debug.Log("RenderTexture released");
    }

    void SaveRenderTextureToTexture2D(RenderTexture rt)
    {
        RenderTexture.active = rt;
        Texture2D texture2D = new Texture2D(rt.width, rt.height);
        texture2D.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        texture2D.Apply();
        RenderTexture.active = null;


        //byte[] bytes = texture2D.EncodeToPNG();
        //System.IO.File.WriteAllBytes(Application.dataPath + "/CapturedImage.png", bytes);
        //Debug.Log("Image saved to " + Application.dataPath + "/CapturedImage.png");
    }

    void OnFreezeRT()
    {
        camera.targetTexture = renderTexture;
        var cameraData = camera.GetUniversalAdditionalCameraData();
        cameraData.renderPostProcessing = false;

        camera.Render();
        SaveRenderTextureToTexture2D(renderTexture);
        cameraData.renderPostProcessing = true;
        camera.targetTexture = null;
        CaptureSphere.SetActive(true);

        //关闭已经截取的物体，切换天空盒
        //这里可以根据你的需求更改逻辑

        var targetObject = GameObject.Find("Target");
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }

        if (TargetSkyboxMaterial != null)
        {
            RenderSettings.skybox = TargetSkyboxMaterial;
        }
    }
}