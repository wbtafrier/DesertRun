using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour, IStateController
{
    [SerializeField] GameObject settingsTextProp = default;
    [SerializeField] GameObject settingsTextBgProp = default;
    [SerializeField] GameObject saveButtonProp = default;
    [SerializeField] GameObject saveButtonBgProp = default;
    [SerializeField] GameObject closeButtonProp = default;
    [SerializeField] GameObject closeButtonBgProp = default;
    [SerializeField] GameObject volumeTextProp = default;
    [SerializeField] GameObject volumeTextBgProp = default;
    [SerializeField] GameObject volumeSliderProp = default;
    [SerializeField] GameObject musicVolumeTextProp = default;
    [SerializeField] GameObject musicVolumeTextBgProp = default;
    [SerializeField] GameObject musicVolumeSliderProp = default;
    [SerializeField] GameObject soundsVolumeTextProp = default;
    [SerializeField] GameObject soundsVolumeTextBgProp = default;
    [SerializeField] GameObject soundsVolumeSliderProp = default;

    GameObject settingsTextObj;
    GameObject settingsTextBgObj;
    GameObject saveButtonObj;
    GameObject saveButtonBgObj;
    GameObject closeButtonObj;
    GameObject closeButtonBgObj;
    GameObject volumeTextObj;
    GameObject volumeTextBgObj;
    GameObject volumeSliderObj;
    GameObject musicVolumeTextObj;
    GameObject musicVolumeTextBgObj;
    GameObject musicVolumeSliderObj;
    GameObject soundsVolumeTextObj;
    GameObject soundsVolumeTextBgObj;
    GameObject soundsVolumeSliderObj;

    static Slider volumeSlider;
    Slider musicVolumeSlider;
    Slider soundsVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        settingsTextObj = settingsTextProp;
        settingsTextBgObj = settingsTextBgProp;
        saveButtonObj = saveButtonProp;
        saveButtonBgObj = saveButtonBgProp;
        closeButtonObj = closeButtonProp;
        closeButtonBgObj = closeButtonBgProp;
        volumeTextObj = volumeTextProp;
        volumeTextBgObj = volumeTextBgProp;
        volumeSliderObj = volumeSliderProp;
        musicVolumeTextObj = musicVolumeTextProp;
        musicVolumeTextBgObj = musicVolumeTextBgProp;
        musicVolumeSliderObj = musicVolumeSliderProp;
        soundsVolumeTextObj = soundsVolumeTextProp;
        soundsVolumeTextBgObj = soundsVolumeTextBgProp;
        soundsVolumeSliderObj = soundsVolumeSliderProp;

        volumeSlider = volumeSliderObj.GetComponent<Slider>();
        volumeSlider.value = AudioListener.volume;
        musicVolumeSlider = musicVolumeSliderObj.GetComponent<Slider>();
        musicVolumeSlider.value = GameStateMachine.GetMusicVolume();
        soundsVolumeSlider = soundsVolumeSliderObj.GetComponent<Slider>();
        soundsVolumeSlider.value = GameStateMachine.GetSoundVolume();
    }

    public void OnStateEnable()
    {
        Start();
        settingsTextObj.SetActive(true);
        settingsTextBgObj.SetActive(true);
        saveButtonObj.SetActive(true);
        saveButtonBgObj.SetActive(true);
        closeButtonObj.SetActive(true);
        closeButtonBgObj.SetActive(true);
        volumeTextObj.SetActive(true);
        volumeTextBgObj.SetActive(true);
        volumeSliderObj.SetActive(true);
        musicVolumeTextObj.SetActive(true);
        musicVolumeTextBgObj.SetActive(true);
        musicVolumeSliderObj.SetActive(true);
        soundsVolumeTextObj.SetActive(true);
        soundsVolumeTextBgObj.SetActive(true);
        soundsVolumeSliderObj.SetActive(true);
    }

    public void OnStateDisable()
    {
        settingsTextObj.SetActive(false);
        settingsTextBgObj.SetActive(false);
        saveButtonObj.SetActive(false);
        saveButtonBgObj.SetActive(false);
        closeButtonObj.SetActive(false);
        closeButtonBgObj.SetActive(false);
        volumeTextObj.SetActive(false);
        volumeTextBgObj.SetActive(false);
        volumeSliderObj.SetActive(false);
        musicVolumeTextObj.SetActive(false);
        musicVolumeTextBgObj.SetActive(false);
        musicVolumeSliderObj.SetActive(false);
        soundsVolumeTextObj.SetActive(false);
        soundsVolumeTextBgObj.SetActive(false);
        soundsVolumeSliderObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void UpdateVolume()
    {
        volumeSlider.value = AudioListener.volume;
    }

    public void VolumeSliderChange()
    {
        AudioListener.volume = volumeSlider.value;
        GameStateMachine.UpdateMuteButton();
    }

    public void MusicVolumeSliderChange()
    {
        GameStateMachine.SetMusicVolume(musicVolumeSlider.value);
    }

    public void SoundVolumeSliderChange()
    {
        GameStateMachine.SetSoundVolume(soundsVolumeSlider.value);
    }

    public void SaveAndClose()
    {
        if (GameStateMachine.GetCurrentStateId() == GameStateMachine.SETTINGS.GetId())
        {
            GameStateMachine.ReturnToMainMenu();
        }
    }
}
