using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinTextScript : MonoBehaviour {

    //public ParticleSystem PSys;
    public Text winText;
    //[SerializeField] private GameObject Win;
    // Use this for initialization

    private void Awake()
    {
        //Win.SetActive(false);
    }
    void Start () {
        //PSys.Stop();
        winText.text = "";
    }
	
	public void UpdateText () {
        //Win.SetActive(true);
        winText.text = "You Win!";
        //PSys.Play();
    }

    public void Update()
    {
        //UpdateText();
        //winText.text = "You Win!";
        //Win.SetActive(true);
    }
}
