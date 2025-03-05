using UnityEngine;

public class Enemy : Interactable
{
    protected bool activated = true;

    void Start()
    {
        if (tag == null) tag = "Enemy";
    }

    protected void Disappear()
    {
        Destroy(gameObject);
    }

}
