using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelevisionWhackAMole : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMPro.TextMeshProUGUI textObject;

    private void Start()
    {
        UpdateScore();
    }
    public void UpdateScore()
    {
        int score = WhackAMoleManager.Score;
        if (score == WhackAMoleManager.ScoreLimit)
        {
            textObject.text = "Bien joue tu as gagne !";
        }
        else
        {
            textObject.text = "" + score;
        }
    }
}
