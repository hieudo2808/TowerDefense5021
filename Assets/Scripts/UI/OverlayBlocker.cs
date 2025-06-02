using UnityEngine;
using UnityEngine.UI;

public class OverlayBlocker : MonoBehaviour
{
    private Image overlayImage;

    void Awake()
    {
        overlayImage = GetComponent<Image>();
        if (overlayImage == null)
        {
            overlayImage = gameObject.AddComponent<Image>();
        }

        Color transparentColor = Color.black;
        transparentColor.a = 0.3f;
        overlayImage.color = transparentColor;

        overlayImage.raycastTarget = true;
    }
}
