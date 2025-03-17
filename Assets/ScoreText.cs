using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreText : MonoBehaviour
{

    /// <summary>
    /// The text used to display the score
    /// </summary>
    TextMeshProUGUI text;

    void Awake()
    {

        text = GetComponent<TextMeshProUGUI>();

        //Score.OnScoreAdd.AddListener((int score) =>
        //{
        //Debug.Log(score);
        //});   
    }

    private void Update()
    {
        text.text = 
            "<swing>" +
            "<palette>" +
            Score.score.ToString();
    }
}
