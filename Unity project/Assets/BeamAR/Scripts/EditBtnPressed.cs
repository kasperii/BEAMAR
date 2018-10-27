using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditBtnPressed : MonoBehaviour {

    [SerializeField] private GameObject PlayerMirror;
    [SerializeField] private GameObject Crosshair;
    [SerializeField] private MeshRenderer PlayerMirrorRend;
    [SerializeField] private MeshRenderer CrosshairRend;

    public void Start()
    {
        //MeshRenderer PlayerMirrorRend = PlayerMirror.GetComponent<MeshRenderer>();
        //MeshRenderer CrossHairRend = Crosshair.GetComponent<MeshRenderer>();
    }

    public void EditBtn()
    {
        if (PlayerMirror.activeSelf)
        {
            PlayerMirror.SetActive(false);
            Crosshair.SetActive(true);
        }
        else if (!PlayerMirror.activeSelf)
        {
            PlayerMirror.SetActive(true);
            Crosshair.SetActive(false);
        }
    }
}
