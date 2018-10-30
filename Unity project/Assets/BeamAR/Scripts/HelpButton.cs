using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpButton : MonoBehaviour
{

    [SerializeField] private GameObject HelpPage;
    [SerializeField] private GameObject[] elementsToBeHidden;
   /* [SerializeField] private Sprite BtnUp;
    [SerializeField] private Sprite BtnDown;
    [SerializeField] private Image ImageComponent;*/

    private bool activeOrNot = false;
    private int ImageInt = 0;

    public void OnClick()
    {
        activeOrNot = !activeOrNot;
        HelpPage.SetActive(activeOrNot);
        for (int i = 0; i < elementsToBeHidden.Length; i++)
        {
            elementsToBeHidden[i].SetActive(!activeOrNot);
        }

        //Not working
        /*if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)        // If touching
            {
                if (ImageComponent.sprite == BtnUp)
                {
                    ImageComponent.sprite = BtnDown;
                }
            }
            else if (touch.phase == TouchPhase.Ended)   // If not touching
            {
                StartCoroutine(PressedButtonUp());
                if (ImageComponent.sprite == BtnDown)
                {
                    ImageComponent.sprite = BtnUp;
                }
            }
        }
    }

    IEnumerator PressedButtonUp()
    {
        yield return new WaitForSeconds(0.3f);
        ImageComponent.sprite = BtnUp;
    }*/
    }
}


