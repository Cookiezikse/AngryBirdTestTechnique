using UnityEngine;
using UnityEngine.Events;

public class Score : MonoBehaviour
{

    /// <summary>
    /// The instance of the gameObject
    /// </summary>
    static public GameObject instance;

    /// <summary>
    /// The instance of the score inside the gameObject
    /// </summary>
    static public Score instanceScore;

    /// <summary>
    /// The score of the game
    /// </summary>
    static public int score { get; private set; } = 0;

    /// <summary>
    /// Event when the score is added
    /// </summary>
    //static public UnityEvent<int> OnScoreAdd;

    /// <summary>
    /// To get the instance for the score
    /// </summary>
    /// <returns></returns>
    static public Score GetInstance()
    {
        if (instance == null)
        {
            GameObject gameObject = new GameObject("Score");
            gameObject.AddComponent<Score>(); 

            instance = Instantiate(gameObject);

            instanceScore = instance.GetComponent<Score>();
        }

        return instanceScore;
    }

    /// <summary>
    /// Add score to the instance
    /// </summary>
    /// <param name="addScore"></param>
    static public void AddScore(int addScore)
    {
        score += addScore;

        //OnScoreAdd?.Invoke(score);
    }

    static public Color GetColorForScore(int score)
    {
        switch (score)
        {
            case 100:
                {
                    return Color.magenta;
                }
            case 200:
                {
                    return Color.red;
                }
            case 500:
                {
                    return Color.blue;
                }
            case 50:
                {
                    return Color.green;
                }
            default:
                {
                    return Color.grey;
                }
        }
    }

}
