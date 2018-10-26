using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpButton : MonoBehaviour {

    [SerializeField] private GameObject HelpPage;

    private bool activeOrNot = false;

    public void OnClick() {
        activeOrNot = !activeOrNot;
        HelpPage.SetActive(activeOrNot);
    }


}
