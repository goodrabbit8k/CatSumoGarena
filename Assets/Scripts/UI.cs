using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public int score = 0;

    [SerializeField] TextMeshProUGUI scoreText;

    private void Update()
    {
        scoreText.text = "Score\n" + score;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
