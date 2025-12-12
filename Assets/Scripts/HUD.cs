using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    public PlayerController player;
    public Image[] lifeImages;
    public Image[] reflectCharges;
    public TMP_Text scoreText;
    public GameObject gameOverPanel;

    private int totalScore = 0;
    bool isGameOver = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverPanel.SetActive(false);
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        SetReflectCharges(player.currentEnergy);
        SetLifeCharges(player.currentHealth);
    }

    void SetReflectCharges(int shipReflects) 
    {
        if (shipReflects >= player.maxEnergy)
        {
            return;
        }

        var currentCharges = Mathf.Clamp(shipReflects, 0, 4);

     
        reflectCharges[currentCharges].gameObject.SetActive(false);

        for (int i = 0; i< currentCharges; i++) 
        {
            reflectCharges[i].gameObject.SetActive(true);
        }

    }

    void SetLifeCharges(int shipLifess)
    {
        if (shipLifess >= player.maxHealth)
        {
            return;
        }

        var currentCharges = Mathf.Clamp(shipLifess, 0, 2);
      

        lifeImages[currentCharges].gameObject.SetActive(false);
        for (int i = 0; i < currentCharges; i++)
        {
            lifeImages[i].gameObject.SetActive(true);
        }

    }

    public void AddScore(int score) 
    {
        totalScore += score;
        scoreText.text = totalScore.ToString();
    }

    public void ResetLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("SampleScene");
    }

    public void OnGameOver()
    {
        gameOverPanel.SetActive(true);
        isGameOver = true;
        
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

}
