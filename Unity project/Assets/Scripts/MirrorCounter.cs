using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Counts how many mirrors are placed and updates the UI text.
 * */
public class MirrorCounter : MonoBehaviour {
    [SerializeField] private string MirrorTag;
    [SerializeField] private Text countText;
    private int amountOfMirrors;

    // Use this for initialization
    void Start () {
        amountOfMirrors = 0;
        SetCountText();
    }

    void SetCountText()
    {
        countText.text = "Mirrors placed: " + amountOfMirrors.ToString() + " / 5";
    }

    // Update is called once per frame
    void Update () {
        amountOfMirrors =  GameObject.FindGameObjectsWithTag(MirrorTag).Length;
        if (amountOfMirrors < 0)
            amountOfMirrors = 0;
        SetCountText();
    }
}
