using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerControl : MonoBehaviour
{

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider MusicMasterSlider;
    [SerializeField] private Slider MusicBGMSlider;

    private void Awake()
    {
        MusicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        MusicBGMSlider.onValueChanged.AddListener(SetBGMVolume);

    }


    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
