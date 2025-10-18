using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer audioMixer;

    [Header("UI Sliders")]
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    
    const string MASTER_PARAM = "MasterVolume";
    const string BGM_PARAM = "BGMVolume";
    const string SFX_PARAM = "SFXVolume";

    void Start()
    {
        // inicializa sliders a partir dos valores atuais do AudioMixer (em dB)
        InitializeSliderFromMixer(masterSlider, MASTER_PARAM, 0.75f);
        InitializeSliderFromMixer(bgmSlider, BGM_PARAM, 0.75f);
        InitializeSliderFromMixer(sfxSlider, SFX_PARAM, 0.75f);

        // adiciona listeners — se preferir pode ligar pelo Inspector
        if (masterSlider != null) masterSlider.onValueChanged.AddListener(SetMasterVolume);
        if (bgmSlider != null) bgmSlider.onValueChanged.AddListener(SetMusicVolume);
        if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void OnDestroy()
    {
        // remove listeners para evitar memory leaks
        if (masterSlider != null) masterSlider.onValueChanged.RemoveListener(SetMasterVolume);
        if (bgmSlider != null) bgmSlider.onValueChanged.RemoveListener(SetMusicVolume);
        if (sfxSlider != null) sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
    }

    void InitializeSliderFromMixer(Slider slider, string paramName, float defaultLinear)
    {
        if (slider == null) return;

        if (audioMixer != null && audioMixer.GetFloat(paramName, out float dB))
        {
            // converte dB -> linear (0..1)
            slider.value = DecibelToLinear(dB);
        }
        else
        {
            // mixer não tem o param exposto ou audioMixer null: usa default
            slider.value = defaultLinear;
            // opcional: aplicar o default no mixer agora
            SetMixerParameterFromLinear(paramName, defaultLinear);
        }
    }

    // conversão linear (0..1) -> decibéis (dB)
    float LinearToDecibel(float linear)
    {
        linear = Mathf.Clamp01(linear);
        if (linear <= 0.0001f) return -80f;
        return Mathf.Log10(linear) * 20f;
    }

    // conversão decibel -> linear (0..1)
    float DecibelToLinear(float dB)
    {
        return Mathf.Pow(10f, dB / 20f);
    }

    // aplica no AudioMixer (usando linear 0..1)
    void SetMixerParameterFromLinear(string paramName, float linear)
    {
        if (audioMixer == null) return;
        float dB = LinearToDecibel(linear);
        audioMixer.SetFloat(paramName, dB);
    }

    // --- funções chamadas pelo UI ---
    public void SetMasterVolume(float linearValue) => SetMixerParameterFromLinear(MASTER_PARAM, linearValue);
    public void SetMusicVolume(float linearValue) => SetMixerParameterFromLinear(BGM_PARAM, linearValue);
    public void SetSFXVolume(float linearValue) => SetMixerParameterFromLinear(SFX_PARAM, linearValue);
}