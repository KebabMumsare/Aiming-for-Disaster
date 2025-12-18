using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroller : MonoBehaviour
{
    [Tooltip("The RawImage to scroll. If not assigned, will try to get component on this object.")]
    public RawImage rawImage;

    [Tooltip("Speed of scrolling in X and Y direction.")]
    public Vector2 scrollSpeed = new Vector2(0.1f, -0.1f);

    private Rect uvRect;

    void Start()
    {
        if (rawImage == null)
        {
            rawImage = GetComponent<RawImage>();
        }

        if (rawImage == null)
        {
            Debug.LogError("BackgroundScroller needs a RawImage component to work!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        // Get current UV rect
        uvRect = rawImage.uvRect;

        // Move the rect
        uvRect.x += scrollSpeed.x * Time.deltaTime;
        uvRect.y += scrollSpeed.y * Time.deltaTime;

        // Apply changes
        rawImage.uvRect = uvRect;
    }
}
