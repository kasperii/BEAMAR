using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Scripts.Multiplayer;

public class MirrorController : MonoBehaviour {

    [SerializeField] private string MirrorTag;
    [SerializeField] private int maxMirrors = 3;
    [SerializeField] private Text countText;
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

  	public void ResetBtn () {
        var mirrors = GameObject.FindGameObjectsWithTag(MirrorTag);
        foreach (GameObject o in mirrors)
        {
            PhotonNetwork.Destroy(o);
        }
        playerManager.setTouchLock(false);
        resetMirrors();
    }
}
