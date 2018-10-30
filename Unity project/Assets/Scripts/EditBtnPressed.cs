using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**
 * Handles what happens when the edit button is pressed. If edit mode is not on (first if statement), change sprite and activate corresponding gameobjects.
 * If edit mode is on, revert back to previous settings with correct sprite and gameobjects active.
 * */
public class EditBtnPressed : MonoBehaviour {

    [SerializeField] private GameObject PlayerMirror;
    [SerializeField] private GameObject Crosshair;
    //[SerializeField] private MeshRenderer PlayerMirrorRend;
    //[SerializeField] private MeshRenderer CrosshairRend;

    [SerializeField] private Sprite EditBtnSprite;
    [SerializeField] private Sprite ExitEditBtnSprite;
    [SerializeField] private Image ImageComponent;



    public void EditBtn()
    {
        if (PlayerMirror.activeSelf)        //If edit mode is not on previously
        {
            ImageComponent.sprite = ExitEditBtnSprite;
            PlayerMirror.SetActive(false);
            Crosshair.SetActive(true);
        }
        else if (!PlayerMirror.activeSelf)      //If edit mode is on
        {
            ImageComponent.sprite = EditBtnSprite;
            PlayerMirror.SetActive(true);
            Crosshair.SetActive(false);
        }
    }
}
