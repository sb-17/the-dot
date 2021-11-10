using UnityEngine;

public class GameSceneLoad : MonoBehaviour
{
    public GameObject bg;

    public Camera mainCam;

    void Start()
    {
        scaleBackgroundImageFitScreenSize();
    }

    private void scaleBackgroundImageFitScreenSize()
    {
        Vector2 deviceScreenResolution = new Vector2(Screen.width, Screen.height);

        float srcHeight = Screen.height;
        float srcWidth = Screen.width;

        float DEVICE_SCREEN_ASPECT = srcWidth / srcHeight;

        mainCam.aspect = DEVICE_SCREEN_ASPECT;

        float camHeight = 100.0f * mainCam.orthographicSize * 2.0f;
        float camWidth = camHeight * DEVICE_SCREEN_ASPECT;

        SpriteRenderer bgSR = bg.GetComponent<SpriteRenderer>();
        float bgHeight = bgSR.sprite.rect.height;
        float bgWidth = bgSR.sprite.rect.width;

        float bgScaleRatioHeight = camHeight / bgHeight;
        float bgScaleRatioWidth = camWidth / bgWidth;

        float x = bgWidth / (bgHeight/33);

        bg.transform.localScale = new Vector3(x, x, 1);
    }
}
