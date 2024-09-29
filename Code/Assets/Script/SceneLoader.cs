using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HindiMenu()
    {
        SceneManager.LoadScene("Hindi Translation System");
    }

    public void EnglishMenu()
    {
        SceneManager.LoadScene("English Translation System");
    }

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
