using NUnit.Framework;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Events;

public abstract class Bird : MonoBehaviour
{

    // --------------------------- BIRD EVENTS ----------------------------------

    /// <summary>
    /// Called when the user clicked on the mouse or finger the bird
    /// </summary>
    protected UnityEvent OnTouch;
    
    /// <summary>
    /// Called when the bird is launched by the Catapult
    /// </summary>
    protected UnityEvent OnLauch;

    /// <summary>
    /// Called when the bird collides with a gameobject that a cube or an enemy
    /// </summary>
    protected UnityEvent OnImpact;
    
    /// <summary>
    /// Called when the bird collides with a gameobject that a cube or an enemy
    /// </summary>
    /// <returns>The Gameobject that it collided with</returns>
    protected UnityEvent<GameObject> OnImpactGameObject;

    /// <summary>
    /// Called when an enemy or cube enters the zone of the bird 
    /// </summary>
    /// <returns>The Gameobject that entered the zone</returns>
    protected UnityEvent<GameObject> OnTriggerEnter;

    // --------------------------- GENERAL VALUES ----------------------------------

    /// <summary>
    /// All the enemies that have been in the trigger zone of the bird
    /// </summary>
    protected List<Enemy> allEnemiesInTrigger = new List<Enemy>();
    
    /// <summary>
    /// All the enemies that are actually in the trigger zone of the bird
    /// </summary>
    protected List<Enemy> enemiesInTrigger = new List<Enemy>();

    /// <summary>
    /// All the cubes that have been in the trigger zone of the bird
    /// </summary>
    protected List<Cube> allCubesInTrigger = new List<Cube>();

    /// <summary>
    /// All the cubes that are actually in the trigger zone of the bird
    /// </summary>
    protected List<Cube> cubesInTrigger = new List<Cube>();

    // --------------------------- BIRD VALUES ----------------------------------

    /// <summary>
    /// Value to know if the bird has been launched
    /// </summary>
    protected bool launched = false;

    /// <summary>
    /// Value to know if the bird collided with an enemy or cube
    /// </summary>
    protected bool impacted = false;

    /// <summary>
    /// Rigidbody of the bird
    /// </summary>
    protected Rigidbody2D rb;

    /// <summary>
    /// Time since the bird has been launched
    /// </summary>
    protected float timer = 0f;

    /// <summary>
    /// The number of times the user has clicked on the bird
    /// </summary>
    protected int touchCount = 0;

    /// <summary>
    /// The collider used to detect the user click on the bird
    /// </summary>
    protected CircleCollider2D circleColliderTrigger;

    // ------------------------------------------------


    /// <summary>
    /// To assing the values before calling Bird.Launch in the DupliBird
    /// </summary>
    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Sets all the values needed for the bird to start (colliders)
    /// </summary>
    protected void Start()
    {
        if (tag == "Untagged") tag = "Bird";

        foreach(CircleCollider2D e in GetComponentsInChildren<CircleCollider2D>())
        {
            if (e.isTrigger)
            {
                Debug.Log("Found");
                circleColliderTrigger = e; 
                
                break;
            }
        }


        StartBird();
    }

    /// <summary>
    /// Function to add code in the child class after the start of the Bird class
    /// </summary>
    protected virtual void StartBird() { }

    /// <summary>
    /// Update to update general values needed for the Bird code 
    /// </summary>
    protected void Update()
    {
        if (launched) timer += Time.deltaTime;

        UpdateBird();
    }

    /// <summary>
    /// Function to add code if needed in a child class, called after the update of the Bird class
    /// </summary>
    protected virtual void UpdateBird() { }

    /// <summary>
    /// Used to change the value of the collider so cannot trigger a skill & calling events
    /// </summary>
    /// <param name="collision"></param>
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (launched)
        {

            if (collision.collider.CompareTag("Cube") || collision.collider.CompareTag("Enemy"))
            {
                impacted = true;

                Debug.Log("CUBE OR ENEMY");
                OnImpact?.Invoke();
                OnImpactGameObject?.Invoke(collision.gameObject);

            }

            if (collision.collider.CompareTag("Floor"))
            {
                circleColliderTrigger.enabled = false;
            }
        }
    }

    /// <summary>
    /// Used to attribute all the general values needed for general purposes (cubes in the trigger, enemies in the trigger) & calling events
    /// </summary>
    /// <param name="collision"></param>
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (launched)
        {
            if (collision.CompareTag("Cube"))
            {
                Cube cube = collision.gameObject.GetComponent<Cube>();

                allCubesInTrigger.Add(cube);
                cubesInTrigger.Add(cube);

                OnTriggerEnter?.Invoke(collision.gameObject);
                return;
            }

            if (collision.CompareTag("Enemy"))
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();

                allEnemiesInTrigger.Add(enemy);
                enemiesInTrigger.Add(enemy);

                OnTriggerEnter?.Invoke(collision.gameObject);
                return;
            }

        }
    }

    /// <summary>
    /// Used to attribute all the general values needed for general purposes, remove cubes & enemies not in the trigger anymore
    /// </summary>
    /// <param name="collision"></param>
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (launched)
        {
            if (collision.CompareTag("Cube"))
            {
                Cube cube = collision.gameObject.GetComponent<Cube>();

                cubesInTrigger.Remove(cube);
                return;
            }

            if (collision.CompareTag("Enemy"))
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();

                enemiesInTrigger.Remove(enemy);
                return;
            }

        }
    }


    //protected virtual void OnTriggerEnterBird(GameObject objectIn)
    //{

    //}

    /// <summary>
    /// Helper function to eject close objects that have been in the trigger
    /// </summary>
    /// <param name="force"></param>
    protected void ExplodeAllCubesAround(float force)
    {
        foreach (Cube cube in allCubesInTrigger)
        {
            cube.GetComponent<Rigidbody2D>().AddForce((cube.transform.position - transform.position).normalized * force);
        }
    }

    /// <summary>
    /// Helper function to eject close objects that are in the trigger
    /// </summary>
    /// <param name="force"></param>
    protected void ExplodeCubesAround(float force)
    {
        foreach (Cube cube in cubesInTrigger)
        {
            cube.GetComponent<Rigidbody2D>().AddForce((cube.transform.position - transform.position).normalized * force);
        }
    }

    /// <summary>
    /// Used to detect any input (mouse or touch) to use a skill & call events
    /// </summary>
    protected void OnMouseDown()
    {
        if (launched)
        {
            //Debug.Log("CAPA");
            OnTouch?.Invoke();
            OnTouchEffect();
            touchCount++;
        }
    }

    public void SetTouchCount(int newTouchCount)
    {
        touchCount = newTouchCount;
    }

    /// <summary>
    /// Function to add code when the player clicks on the bird to activate a skill 
    /// </summary>
    protected virtual void OnTouchEffect() { }

    /// <summary>
    /// General Function to launch the bird with a direction and force
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="force"></param>
    /// <returns></returns>
    public UnityEvent Launch(Vector2 dir, float force)
    {
        rb.AddForce(dir * force, ForceMode2D.Impulse);

        return LaunchStart();
    }

    /// <summary>
    /// General Function to launch the bird with a force directly
    /// </summary>
    /// <param name="force"></param>
    /// <returns></returns>
    public UnityEvent Launch(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);

        return LaunchStart();
    }

    /// <summary>
    /// Used to set values & call events
    /// </summary>
    /// <returns>Event when impacted</returns>
    UnityEvent LaunchStart()
    {
        launched = true;
        OnLauch?.Invoke();

        return OnImpact;
    }
}
