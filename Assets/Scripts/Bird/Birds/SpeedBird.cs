using UnityEngine;

public class SpeedBird : Bird
{

    /// <summary>
    /// The multiplier by which the current velocity of the bird will be multiplied
    /// </summary>
    [SerializeField]
    private float speedBoostMultiplier = 1.5f;

    /// <summary>
    /// Number used to have a limit of the user clicks on the bird
    /// </summary>
    [SerializeField]
    private int numberOfTouchAllowed = 1;

    //-----------------------------------------------------------------

    /// <summary>
    /// Used to multiply the speed of the bird when the user clicks on the bird
    /// </summary>
    protected override void OnTouchEffect()
    {
        if (touchCount <= numberOfTouchAllowed - 1)
        {
            rb.linearVelocity *= speedBoostMultiplier;
        }
    }

}
