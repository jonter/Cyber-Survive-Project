using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ErrorPanel : MonoBehaviour
{
    [SerializeField] Button restartButton;
    [SerializeField] Button exitButton;

    [SerializeField] TMP_Text errorText;

    public void SetupPanel(string errorString)
    {
        errorText.text = "Error: "+ errorString;
        restartButton.onClick.AddListener(RestartGame);
    }


    // выход из игры сделаем потом
    void RestartGame()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }

    
}
