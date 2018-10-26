using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditBtnPressed : MonoBehaviour {

    [SerializeField] private GameObject PlayerMirror;
    [SerializeField] private GameObject Crosshair;
    [SerializeField] private MeshRenderer PlayerMirrorRend;
    [SerializeField] private MeshRenderer CrosshairRend;

    [SerializeField] private Sprite EditBtnSprite;
    [SerializeField] private Sprite ExitEditBtnSprite;
    [SerializeField] private Image ImageComponent;



    public void EditBtn()
    {
        if (PlayerMirror.activeSelf)
        {
            ImageComponent.sprite = ExitEditBtnSprite;
            PlayerMirror.SetActive(false);
            Crosshair.SetActive(true);
        }
        else if (!PlayerMirror.activeSelf)
        {
            ImageComponent.sprite = EditBtnSprite;
            PlayerMirror.SetActive(true);
            Crosshair.SetActive(false);
        }
    }
}
