using UnityEngine;

public class DupliBird : Bird
{

    /// <summary>
    /// Number used to have a limit of the user clicks on the bird
    /// </summary>
    [SerializeField]
    private int numberOfTouchAllowed = 1;

    /// <summary>
    /// Number of clones the DupliBird will spawn
    /// </summary>
    [SerializeField]
    private int numberOfClones = 2;

    /// <summary>
    /// The distance that the clones will spawn from the original one multiplied by a value
    /// </summary>
    [SerializeField]
    private float distSpawnY = 0.5f;

    /// <summary>
    /// The angle that the clone will have to seperate them a little bit
    /// </summary>
    [SerializeField]
    private float angleSpawn = 5;

    /// <summary>
    /// Prefab of the bird itself to spawn it
    /// </summary>
    [SerializeField]
    public GameObject prefabDupliBird;

    //-----------------------------------------------------------

    /// <summary>
    /// Function used to spawn the clones when the user clicks on the bird
    /// </summary>
    protected override void OnTouchEffect()
    {
        if (touchCount <= numberOfTouchAllowed - 1)
        {
            if (prefabDupliBird == null) return;

            // Each clone is spawn from top to bottom depending on the 'i' with the value of angle & distance multiplied by the 'i'
            for (int i = 0; i < numberOfClones; i++)
            {
                Vector2 force = (Vector2)(Quaternion.Euler(0, 0, i * i * i % 2 == 0 ? angleSpawn : -angleSpawn) * rb.linearVelocity);

                GameObject newBird = Instantiate(prefabDupliBird, transform.position + new Vector3(0, i * i % 2 == 0 ? distSpawnY : -distSpawnY, 0), Quaternion.identity); ;
                Bird bird = newBird.GetComponent<Bird>();
                bird.Launch(force);
                bird.SetTouchCount(10);

            }

        }
        
    }

}
