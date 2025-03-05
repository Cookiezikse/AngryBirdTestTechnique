using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Trajectory : MonoBehaviour
{
    
    /// <summary>
    /// The line renderer used to show the trajectory of an object
    /// </summary>
    [SerializeField]
    public LineRenderer lineRenderer;

    /// <summary>
    /// Prefab used to have the dots to show the trajectory of an object
    /// </summary>
    [SerializeField] 
    public GameObject dotPrefab;

    /// <summary>
    /// The time between each points to show the trajectory
    /// </summary>
    [SerializeField]
    public float timeBetweenPoints = 0.1f;

    /// <summary>
    /// The number of points that the trajectory will have
    /// </summary>
    [SerializeField]
    private int numberOfPoints = 10;

    /// <summary>
    /// If the trajectory is activated or not to show the trajectory
    /// </summary>
    [SerializeField]
    public bool activated = false;

    /// <summary>
    /// The start point where the trajectory will begin
    /// </summary>
    private Vector2 startPoint = Vector2.zero;
    
    /// <summary>
    /// The force of which the object is thrown
    /// </summary>
    private Vector2 force = Vector2.zero;

    /// <summary>
    /// The type of trajectory to display it : 0 -> Line ; 1 -> Dots
    /// </summary>
    [SerializeField]
    private int typeTrajectory = 0;

    /// <summary>
    /// The parent of the dots that are instantiated
    /// </summary>
    Transform dotsParent;

    /// <summary>
    /// The list of the dots that are used to show the trajectory
    /// </summary>
    List<Transform> dots = new List<Transform>();

    //---------------------------------------------------------

    /// <summary>
    /// Instantiate & Changes the values needed with the current options selected (activated & typeTrajectory)
    /// </summary>
    private void Awake()
    {
        dotsParent = Instantiate(new GameObject("Dots"), transform).transform;
        lineRenderer.enabled = false;
        dotsParent.gameObject.SetActive(false);

        for (int i = 0; i < numberOfPoints; i++)
        {
            dots.Add(Instantiate(dotPrefab, dotsParent).transform);
        }

        lineRenderer.positionCount = numberOfPoints;

        if (activated)
        {
            Show();
        } else
        {
            Hide();
        }
    }
    
    /// <summary>
    /// Update the trajectory of the object
    /// </summary>
    /// <param name="_startPoint"></param>
    /// <param name="_force"></param>
    public void UpdateTrajectory(Vector2 _startPoint, Vector2 _force)
    {
        startPoint = _startPoint;
        force = _force;
        DotsOrLine();
    }

    /// <summary>
    /// To set the type of trajectory that will be used (Line or Dots)
    /// </summary>
    /// <param name="newType"></param>
    public void SetTypeTrajectory(int newType)
    {
        if (newType == typeTrajectory) return;

        typeTrajectory = newType;

        switch (typeTrajectory)
        {
            case 0:
                {
                    dotsParent.gameObject.SetActive(false);
                    break;
                }
            case 1:
                { 
                    lineRenderer.enabled = false; 
                    break;
                }

        }
    }

    /// <summary>
    /// Function used to set the values of each different state (Line or Dots)
    /// </summary>
    /// <param name="type"></param>
    /// <param name="active"></param>
    private void SetValuesWithTypeTrajectory(int type, bool active)
    {
        switch (type)
        {
            case 0:
                {
                    if (active)
                    {
                        lineRenderer.enabled = true;
                    } else
                    {
                        lineRenderer.enabled = false;
                    }
                    break;
                }
            case 1:
                {
                    if (active)
                    {
                        dotsParent.gameObject.SetActive(true);
                    } else
                    {
                        dotsParent.gameObject.SetActive(false);
                    }
                    break;
                }

        }
    }

    /// <summary>
    /// Function used to update either the dots or the line
    /// </summary>
    private void DotsOrLine()
    {
        if (!activated) return;

        switch (typeTrajectory)
        {
            // Line
            case 0:
                {
                    Line();
                    break;
                }

            // Points
            case 1:
                {
                    Dots();
                    break;
                }


            default:
                {
                    break;
                }
        }
    } 

    /// <summary>
    /// Function used to change the points of the line to correspond to the trajectory of the object
    /// </summary>
    private void Line()
    {
        if (lineRenderer == null) return;

        for (int i = 0; i < numberOfPoints; i++)
        {
            lineRenderer.SetPosition(i, CalculatePositionWithTime(timeBetweenPoints * i));
        }
    }

    /// <summary>
    /// Function used to change the position of each dots corresponding to the trajectory of the object
    /// </summary>
    private void Dots()
    {
        for (int i = 0; i < dotsParent.childCount; i++)
        {
            dots[i].position = CalculatePositionWithTime(timeBetweenPoints * i);
        }
    }

    /// <summary>
    /// Function to show the trajectory
    /// </summary>
    public void Show()
    {
        activated = true;

        SetValuesWithTypeTrajectory(typeTrajectory, activated);
    }

    /// <summary>
    /// Function to hide the trajectory
    /// </summary>
    public void Hide()
    {
        activated = false;

        SetValuesWithTypeTrajectory(typeTrajectory, activated);
    }

    /// <summary>
    /// Function used to calculate the position with time of an object with a force applied
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public Vector2 CalculatePositionWithTime(float time)
    {
        // x(t) = x0 + V0x * t
        // y(t) = y0 + V0y * t - (1/2 * gt^2)

        return new Vector2(
            startPoint.x + force.x * time,
            (startPoint.y + force.y * time) - (Physics2D.gravity.magnitude * Mathf.Pow(time,2)) / 2f);
    }


}
