using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject MainPanel, ExplanationPanel, CreditsPanel, OptionsPanel;


    [SerializeField] Slider soundSlider, musicSlider;
    [SerializeField] AudioMixer soundMixer, musicMixer;

    public void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Art_scene 2", LoadSceneMode.Single);
    }

    public void Credits()
    {
        MainPanel.SetActive(false);
        CreditsPanel.SetActive(true);
    }

    public void Options()
    {
        MainPanel.SetActive(false);
        OptionsPanel.SetActive(true);

        float soundValue;
        soundMixer.GetFloat("Sound", out soundValue);
        soundValue = Mathf.Pow(10, (soundValue / 20));
        soundSlider.value = soundValue;

        float musicValue;
        musicMixer.GetFloat("Music", out musicValue);
        musicValue = Mathf.Pow(10, (musicValue / 20));
        musicSlider.value = musicValue;
    }

    public void BackToMain(GameObject panelToClose)
    {
        panelToClose.SetActive(false);
        MainPanel.SetActive(true);
    }

    public void SetSoundLevel(float sliderValue)
    {
        soundMixer.SetFloat("Sound", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusicLevel(float sliderValue)
    {
        musicMixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
