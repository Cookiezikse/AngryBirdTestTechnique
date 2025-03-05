using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Cube : Interactable
{

    //--------------------------- CUBE EVENTS ----------------------

    /// <summary>
    /// Event when a bird enters in the trigger zone of the cube
    /// </summary>
    public UnityEvent<Bird> birdEnterEvent;
    
    /// <summary>
    /// Event when a bird exits in the trigger zone of the cube
    /// </summary>
    public UnityEvent<Bird> birdExitEvent;

    //---------------------------- GENERAL VALUES ----------------------

    /// <summary>
    /// Birds that are currently in the trigger zone of the cube
    /// </summary>
    protected List<Bird> birdsInZone = new List<Bird>();

    // ---------------------------- CUBE VALUES ---------------------------

    /// <summary>
    /// Value used to do something when a bird enters
    /// </summary>
    [SerializeField]
    protected bool activated = false;

    /// <summary>
    /// Time since the cube has appeared
    /// </summary>
    protected float timer = 0;

    //--------------------------------------------

    /// <summary>
    /// Start function to set the properties of the cube in general
    /// </summary>
    protected void Start()
    {
        tag = "Cube";

        StartCube();
    }

    /// <summary>
    /// Function to add code to do something after the start of the general cube
    /// </summary>
    protected virtual void StartCube() { }

    /// <summary>
    /// Update of the cube to set general values
    /// </summary>
    protected void Update()
    {
        timer += Time.deltaTime;

        UpdateCube();
    }

    /// <summary>
    /// Function to add code to do something after the update of the general cube
    /// </summary>
    protected virtual void UpdateCube() { }

    /// <summary>
    /// Detect if a bird entered the zone and add it to the list & call events
    /// </summary>
    /// <param name="collision"></param>
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated)
        {
            if (collision.CompareTag("Bird")) {
                Bird bird = collision.GetComponent<Bird>();

                birdsInZone.Add(bird);

                OnBirdEnter(bird);
                birdEnterEvent?.Invoke(bird);
            }
        }
    }

    /// <summary>
    /// Function to set the deactivation of the cube (no more trigger of the bird enter)
    /// </summary>
    public void Deactivate()
    {
        activated = false;

        DeactivateCube();
    }

    /// <summary>
    /// Function to set the deactivation of the cube & some properties (no more trigger of the bird enter)
    /// </summary>
    public void DeactivateAll()
    {
        activated = false;
        GetComponent<BoxCollider2D>().enabled = false;

        DeactivateCube();
    }

    /// <summary>
    /// Function to add code when the deactivation / deactivationAll of the cube is called 
    /// </summary>
    protected virtual void DeactivateCube() { }

    /// <summary>
    /// Function to change the color of the cube with its sprite renderer
    /// </summary>
    /// <param name="color"></param>
    public void ChangeColor(Color color)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr != null) 
        {
            sr.color = color;
        }
    }

    /// <summary>
    /// Function to set the activation of the cube 
    /// </summary>
    public void Activate()
    {
        activated = true;
    }

    /// <summary>
    /// Function used when a bird leaves the trigger zone of the cube
    /// </summary>
    /// <param name="collision"></param>
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (activated)
        {
            if (collision.CompareTag("Bird"))
            {
                Bird bird = collision.GetComponent<Bird>();

                birdsInZone.Remove(bird);

                birdExitEvent?.Invoke(bird);
                OnBirdExit(bird);
            }
        }
    }

    /// <summary>
    /// Function to add code when a bird enters the trigger zone of the cube
    /// </summary>
    /// <param name="bird"></param>
    protected virtual void OnBirdEnter(Bird bird) { }

    /// <summary>
    /// Function to add code when a bird exits the trigger zone of the cube
    /// </summary>
    /// <param name="bird"></param>
    protected virtual void OnBirdExit(Bird bird) { }
}
