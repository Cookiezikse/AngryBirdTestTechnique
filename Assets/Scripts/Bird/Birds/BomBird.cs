using UnityEngine;

public class BomBird : Bird
{

    /// <summary>
    /// Force of the explosion of the bird when clicked by the player
    /// </summary>
    [SerializeField]
    float forceExplosion = 500;

    /// <summary>
    /// Number used to have a limit of the user clicks on the bird
    /// </summary>
    [SerializeField]
    int numberOfTouchAllowed = 1;

    //-------------------------------------------------

    /// <summary>
    /// Function to explode all the cubes around the BomBird
    /// </summary>
    protected override void OnTouchEffect()
    {
        if (touchCount >= numberOfTouchAllowed) return;

        foreach (Cube a in cubesInTrigger)
        {
            Score.AddScore(scoreToAdd);

            GameObject effect = Instantiate(scoreEffect, transform.position, Quaternion.identity, GameObject.Find("Effects").transform);
            effect.GetComponent<ScoreEffect>().SetScore(scoreToAdd);

        }

        ExplodeCubesAround(forceExplosion);
    }
}
