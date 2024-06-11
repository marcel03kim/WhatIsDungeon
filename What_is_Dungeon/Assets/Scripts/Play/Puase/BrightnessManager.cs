using UnityEngine;
using UnityEngine.UI;

public class BrightnessManager : MonoBehaviour
{
    public static BrightnessManager Instance;
    public Image brightnessOverlay;
    public Slider brightnessSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Set initial brightness based on saved player preference or default value
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0.5f);
        brightnessSlider.value = savedBrightness;
        SetBrightness(savedBrightness);

        // Add listener to the slider
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
    }

    public void SetBrightness(float value)
    {
        // Set the alpha of the brightness overlay image
        if (brightnessOverlay != null)
        {
            Color overlayColor = brightnessOverlay.color;
            overlayColor.a = 1 - value; // Invert the value to make slider work correctly
            brightnessOverlay.color = overlayColor;

            // Save brightness setting
            PlayerPrefs.SetFloat("Brightness", value);
        }
    }
}