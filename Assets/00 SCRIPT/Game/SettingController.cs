using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SettingController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject soundOn;
    [SerializeField] GameObject soundOff;
    [SerializeField] GameObject vibrationOn;
    [SerializeField] GameObject vibrationOff;
    bool soundEnable = true;
    bool vibrationEnable = true;

    void Start()
    {
        if (soundOn.activeInHierarchy) { soundEnable = true;} else { soundEnable = false;}
        if (vibrationOn.activeInHierarchy) {  vibrationEnable = true;} else {  vibrationEnable = false;}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickedSoundButton()
    {
        if (soundEnable)
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        } else
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }
        soundEnable = !soundEnable;
    }

    public void ClickedVibrationButton()
    {
        if (vibrationEnable)
        {
            vibrationOn.SetActive(false);
            vibrationOff.SetActive(true);
        }
        else
        {
            vibrationOn.SetActive(true);
            vibrationOff.SetActive(false);
        }
        vibrationEnable = !vibrationEnable;
    }

    public void ClickedHomeButton()
    {
        // Lấy tên của scene hiện tại
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Tải lại scene hiện tại
        SceneManager.LoadScene(currentSceneName);

    }
}
