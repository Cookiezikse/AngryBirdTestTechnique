using UnityEngine;

public class GravityCube : Cube
{

    /// <summary>
    /// Force that will attract the cube only when it enters the cube trigger zone
    /// </summary>
    [SerializeField]
    float forceAttractGravity = 1.0f;

    /// <summary>
    /// To invert the direction of the force (false = to the cube, true = opposite direction of the bird)
    /// </summary>
    [SerializeField]
    bool invert = true;

    //-------------------------------------------------------------------

    /// <summary>
    /// Used when a bird enters to change its direction
    /// </summary>
    /// <param name="bird"></param>
    protected override void OnBirdEnter(Bird bird)
    {
        //Debug.Log("BIRD IN");
        float multi = invert ? 1 : -1;

        bird.Launch((bird.transform.position - transform.position).normalized * forceAttractGravity * multi);
    }

    /// <summary>
    /// Used to deactivate the cube and change its color
    /// </summary>
    protected override void DeactivateCube()
    {
        gameObject.SetActive(false);
    }

}
