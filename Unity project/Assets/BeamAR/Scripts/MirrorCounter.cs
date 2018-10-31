using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Multiplayer;

public class MirrorCounter : MonoBehaviour {
    [SerializeField] private Text countText;
    [SerializeField] private int maxMirrors = 3;
    [SerializeField] private PlayerManager playerManager;

    private int amountOfMirrors = 0;

    // Use this for initialization
    void Start () {
        SetCountText();
    }

    void SetCountText()
    {
        countText.text = "Mirrors placed: " + amountOfMirrors + " / " + maxMirrors;
        /*if (count >= 12)
        {
            winText.text = "You Win!";
        }*/
    }

    public void addMirror()
    {
        ++amountOfMirrors;
        SetCountText();
    }

    public void resetMirrors()
    {
        amountOfMirrors = 0;
        SetCountText();
    }

    // Update is called once per frame
    void Update () {

    }
}
