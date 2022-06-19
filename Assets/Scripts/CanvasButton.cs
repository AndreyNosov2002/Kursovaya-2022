using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class CanvasButton : MonoBehaviour
{
    public Sprite musicOn, musicOff;

    public void Start()
    {
        if (PlayerPrefs.GetString("music") == "No"&& gameObject.name=="Music")
        {
            GetComponent<Image>().sprite = musicOff;
        }
    }
    public void RestartGame()
 {

     if (PlayerPrefs.GetString("music") != "No")
         GetComponent<AudioSource>().Play();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
 }

 public void LoadInstagram()
 {
     if (PlayerPrefs.GetString("music") != "No")
         GetComponent<AudioSource>().Play();
        Application.OpenURL("https://www.instagram.com/a.andrsvv/");
 }
    public void LoadShop()
    {

        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();

        SceneManager.LoadScene("Shop");
    }

    public void CloseShop()
    {

        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();

        SceneManager.LoadScene("Main");
    }
    public void MusicWork()
 {
     //������ ������ ��������� � � ����� ��������
     if (PlayerPrefs.GetString("music") == "No")
     {
         GetComponent<AudioSource>().Play();
            PlayerPrefs.SetString("music","Yes");
         GetComponent<Image>().sprite = musicOn;
     }
     else
     {
         PlayerPrefs.SetString("music", "No");
         GetComponent<Image>().sprite = musicOff;
        }
 }
}
