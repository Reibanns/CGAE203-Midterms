using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame()
    {
        //SceneManager.LoadScene("MainLevel");
        Invoke("CallScene", 0.5f);
    }

    void CallScene()
    {
        SceneManager.LoadScene("MainLevel");
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();
    }
}
