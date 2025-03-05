using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Catapult : MonoBehaviour
{

    //--------------------------------------- CATAPULT EVENTS -------------------

    /// <summary>
    /// Event called when there is no more birds to be thrown
    /// </summary>
    public UnityEvent OnEnd;

    // -------------------------------------- GENERAL VALUES ---------------------

    /// <summary>
    /// The point where the user started to click
    /// </summary>
    protected Vector2 startPoint = Vector2.zero;
    
    /// <summary>
    /// The point where the user is currently clicking
    /// </summary>
    protected Vector2 endPoint = Vector2.zero;

    /// <summary>
    /// The point where the bird is placed when throwing
    /// </summary>
    public Transform startPointBird;
    
    /// <summary>
    /// The line renderer used to see the trajectory of the object when throwing it
    /// </summary>
    //public LineRenderer dirRenderer;

    /// <summary>
    /// The line renderer used to draw the lines of the catapults 
    /// </summary>    
    public LineRenderer lr;

    /// <summary>
    /// Value when the catapult cannot throw another bird because there are no other birds to throw
    /// </summary>
    protected bool end = false;

    /// <summary>
    /// Value used for the radius of the bird to throw 
    /// </summary>
    [SerializeField]
    protected float maxThrowValue = 5;

    /// <summary>
    /// Force of the throw when throwing a bird
    /// </summary>
    [SerializeField]
    protected float forceThrow = 6;

    /// <summary>
    /// Value if the catapult is activated or not
    /// </summary>
    protected bool activated = true;

    /// <summary>
    /// List of all the birds that will be thrown 
    /// </summary>
    protected List<Bird> birds = new List<Bird>();
    
    /// <summary>
    /// The current bird that will be thrown
    /// </summary>
    protected Bird currentBird = null;
    
    /// <summary>
    /// The current bird index used to calculate if it's the end
    /// </summary>
    protected int currentBirdIndex = 0;

    /// <summary>
    /// The trajectory used to draw the points of the bird position before throwing
    /// </summary>
    protected Trajectory trajectory;

    //-----------------------------------------------------

    /// <summary>
    /// Start function to set all values needed
    /// </summary>
    protected void Start()
    {
        if (GetComponent<LineRenderer>() == null)
        {
            lr = transform.AddComponent<LineRenderer>();
        } else
        {
            lr = GetComponent<LineRenderer>();
        }
        lr.positionCount = 2;
        

        GameObject birdsGameObject = GameObject.Find("Birds");

        if (birdsGameObject == null) return;


        foreach (Bird bird in birdsGameObject.GetComponentsInChildren<Bird>())
        {
            birds.Add(bird);
            bird.GetComponent<Rigidbody2D>().simulated = false;
        }

        currentBirdIndex = 0;
        currentBird = birds[currentBirdIndex];

        trajectory = GetComponentInChildren<Trajectory>();

        StartCatapult();
    }

    /// <summary>
    /// Function to add code called after the original Start
    /// </summary>
    protected void StartCatapult() { }

    /// <summary>
    /// Update of the catapult to set values
    /// </summary>
    protected void Update()
    {
        // Calculate the throw distance etc...
        if (currentBird != null && startPoint != Vector2.zero)
        {

            if (!trajectory.activated) trajectory.Show();

            trajectory.UpdateTrajectory(currentBird.transform.position, GetForceThrow());

        } else
        {
            if(trajectory.activated) trajectory.Hide();
        }

        UpdateCatapult();
    }

    /// <summary>
    /// Function to add code for the Catapult called after the original update
    /// </summary>
    protected void UpdateCatapult() { }

    /// <summary>
    /// When the player clicks on the catapult to start to throw
    /// </summary>
    protected void OnMouseDown()
    {
        startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    /// <summary>
    /// When the player drags the click on the catapult to throw, used to calculate the position of the bird and force
    /// </summary>
    protected void OnMouseDrag()
    {
        if (currentBird == null) return;

        //Debug.Log("Mouse On");
        endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Debug.Log(GetDistanceClamped());

        currentBird.transform.position = 
            new Vector2(startPointBird.position.x, startPointBird.position.y) + 
            ((startPoint - endPoint).normalized * GetDistanceClamped() * -1);


        lr.SetPosition(0, startPointBird.position);
        lr.SetPosition(1, currentBird.transform.position);

    }

    /// <summary>
    /// Get the position of the current bird with the value of time
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public Vector2 GetPositionBirdWithTime(float time)
    {
        Vector2 force = GetForceThrow();
        // y(t) = y0 + V0y * t - (1/2 * gt^2)
        // x(t) = x0 + V0x * t
        return new Vector2(
            currentBird.transform.position.x + force.x * time ,
            (currentBird.transform.position.y + force.y * time) - (Physics2D.gravity.magnitude * time * time) / 2f);
    }

    /// <summary>
    /// Get the distance between the start click point and the end click point
    /// </summary>
    /// <returns></returns>
    public float GetDistance()
    {
        return Vector2.Distance(startPoint, endPoint);
    }

    /// <summary>
    /// Get the distance between the start click point and the end click point clamped with the 'maxThrowValue'
    /// </summary>
    /// <returns></returns>
    public float GetDistanceClamped()
    {
        return Mathf.Clamp(Vector2.Distance(startPoint, endPoint),0, maxThrowValue);
    }

    /// <summary>
    /// Get the Force of the throw for the bird
    /// </summary>
    /// <returns></returns>
    public Vector2 GetForceThrow()
    {
        return (startPoint - endPoint).normalized * (GetDistanceClamped() * forceThrow);
    }
    
    /// <summary>
    /// When the player throws the bird, used to change the current bird and launch the current bird
    /// </summary>
    protected void OnMouseUp()
    {
        lr.SetPosition(0, startPointBird.position);
        lr.SetPosition(1, startPointBird.position);

        // Throw
        if (currentBird == null) return;

        Rigidbody2D rbBird = currentBird.GetComponent<Rigidbody2D>();

        rbBird.simulated = true;

        currentBird.Launch(GetForceThrow());
        //    .AddListener( () =>
        //{
        //
        //});


        if (currentBirdIndex + 1 < birds.Count)
        {
            currentBird = birds[currentBirdIndex + 1];
            //currentBird.GetComponent<Rigidbody2D>().simulated = false;
            currentBird.transform.position = startPointBird.position;

            currentBirdIndex++;
        }
        else
        {
            currentBird = null;
            end = true;
            OnEnd!.Invoke();
        }

        startPoint = Vector2.zero; 
        endPoint = Vector2.zero;
    }
}
