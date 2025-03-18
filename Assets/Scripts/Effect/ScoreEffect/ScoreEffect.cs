using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class ScoreEffect : MonoBehaviour
{
    /// <summary>
    /// Score that will be displayed
    /// </summary>
    int score = 0;

    /// <summary>
    /// Text used to display the score for the effect
    /// </summary>
    TextMeshPro text;

    /// <summary>
    /// The duration of the animation
    /// </summary>
    float duration = 2f;

    /// <summary>
    /// Set the value for the score to display
    /// </summary>
    /// <param name="newScore"></param>
    public void SetScore(int newScore) { 
        score = newScore;

        text = GetComponent<TextMeshPro>();

        text.color = Score.GetColorForScore(score);

        text.text = score.ToString();

        transform.DOMoveY(transform.position.y + 3, duration);
        text.DOFade(0, duration);

    }
}
