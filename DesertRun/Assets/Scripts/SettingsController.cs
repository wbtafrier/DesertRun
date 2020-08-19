using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour, IStateController
{
    [SerializeField] GameObject settingsTextProp;
    [SerializeField] GameObject settingsTextBgProp;
    [SerializeField] GameObject saveButtonProp;
    [SerializeField] GameObject saveButtonBgProp;
    [SerializeField] GameObject closeButtonProp;
    [SerializeField] GameObject closeButtonBgProp;
    [SerializeField] GameObject volumeTextProp;
    [SerializeField] GameObject volumeTextBgProp;
    [SerializeField] GameObject volumeSliderProp;
    [SerializeField] GameObject musicVolumeTextProp;
    [SerializeField] GameObject musicVolumeTextBgProp;
    [SerializeField] GameObject musicVolumeSliderProp;
    [SerializeField] GameObject soundsVolumeTextProp;
    [SerializeField] GameObject soundsVolumeTextBgProp;
    [SerializeField] GameObject soundsVolumeSliderProp;

    static GameObject settingsTextObj;
    static GameObject settingsTextBgObj;
    static GameObject saveButtonObj;
    static GameObject saveButtonBgObj;
    static GameObject closeButtonObj;
    static GameObject closeButtonBgObj;
    static GameObject volumeTextObj;
    static GameObject volumeTextBgObj;
    static GameObject volumeSliderObj;
    static GameObject musicVolumeTextObj;
    static GameObject musicVolumeTextBgObj;
    static GameObject musicVolumeSliderObj;
    static GameObject soundsVolumeTextObj;
    static GameObject soundsVolumeTextBgObj;
    static GameObject soundsVolumeSliderObj;

    static Slider volumeSlider;
    static Slider musicVolumeSlider;
    static Slider soundsVolumeSlider;

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
