using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MirrorCounter : MonoBehaviour {
    [SerializeField] private string MirrorTag;
    [SerializeField] private Text countText;
    //private int count;
    private int amountOfMirrors;

    // Use this for initialization
    void Start () {
        amountOfMirrors = 0;
        //count = 5;
        SetCountText();
    }

    void SetCountText()
    {
        //count = count - amountOfMirrors;
        countText.text = "Mirrors placed: " + amountOfMirrors.ToString() + " / 5";
        /*if (count >= 12)
        {
            winText.text = "You Win!";
        }*/ 
    }

    // Update is called once per frame
    void Update () {
        amountOfMirrors =  GameObject.FindGameObjectsWithTag(MirrorTag).Length - 1;
        SetCountText();
    }
}
