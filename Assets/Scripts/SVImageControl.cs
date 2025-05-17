using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SVImageControlFull : MonoBehaviour, IPointerClickHandler, IDragHandler
{
   
    [SerializeField] private Image pickerImage;
    [SerializeField] private Slider hueSlider;

    
    [SerializeField] private Material mainMaterial;
    [SerializeField] private Material outlineMaterial;

    private RawImage svImage;
    private RectTransform rectTransform, pickerTransform;
    private Texture2D svTexture;

    private float currentHue = 0f;
    private float currentS = 0f;
    private float currentV = 0f;

    private void Awake()
    {
        svImage = GetComponent<RawImage>();
        rectTransform = GetComponent<RectTransform>();
        pickerTransform = pickerImage.GetComponent<RectTransform>();

        hueSlider.onValueChanged.AddListener(OnHueChanged);

        GenerateSVTexture();
        UpdateColorWithCurrentHSV();
    }

    private void GenerateSVTexture()
    {
        svTexture = new Texture2D(100, 100);
        svTexture.wrapMode = TextureWrapMode.Clamp;
        svTexture.name = "SVTexture";
        UpdateSVTexture();
        svImage.texture = svTexture;
    }

    private void UpdateSVTexture()
    {
        for (int y = 0; y < svTexture.height; y++)
        {
            for (int x = 0; x < svTexture.width; x++)
            {
                float s = (float)x / svTexture.width;
                float v = (float)y / svTexture.height;
                svTexture.SetPixel(x, y, Color.HSVToRGB(currentHue, s, v));
            }
        }
        svTexture.Apply();
    }

    private void OnHueChanged(float newHue)
    {
        currentHue = newHue;
        UpdateSVTexture();
        UpdateColorWithCurrentHSV();
    }

    private void UpdateColor(Vector2 screenPos)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPos, null, out localPos);

        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        float s = Mathf.Clamp01((localPos.x + width / 2f) / width);
        float v = Mathf.Clamp01((localPos.y + height / 2f) / height);

        currentS = s;
        currentV = v;

        pickerTransform.localPosition = new Vector2(
            s * width - width / 2f,
            v * height - height / 2f
        );

        UpdateColorWithCurrentHSV();
    }

    private void UpdateColorWithCurrentHSV()
    {
        Color mainColor = Color.HSVToRGB(currentHue, currentS, currentV);
        if (mainMaterial != null) mainMaterial.color = mainColor;

        if (outlineMaterial != null)
        {
            float outlineV = Mathf.Clamp01(currentV - 0.3f);
            Color outlineColor = Color.HSVToRGB(currentHue, currentS, outlineV);
            outlineMaterial.color = outlineColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData) => UpdateColor(eventData.position);

    public void OnDrag(PointerEventData eventData) => UpdateColor(eventData.position);
}
