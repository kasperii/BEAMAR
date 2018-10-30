using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * Change material from list index 0 to list index 1. Not used anymore
 * */
public class ChangeColorOnGoal : MonoBehaviour
{
    public Material[] material;

    [HideInInspector] public Renderer rend;

    private int index = 0;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.material = material[index];
    }

    // Update is called once per frame
    public void materialChange(Renderer render)
    {
        render.material = material[index + 1];
    }
}

