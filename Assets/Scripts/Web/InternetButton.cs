using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InternetButton : MonoBehaviour
{
    private void Start()
    {
        if(this.gameObject.GetComponentInChildren<Button>() == null)
        {
            this.enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            this.gameObject.GetComponentInChildren<Button>().interactable = true;
        }
        else
        {
            this.gameObject.GetComponentInChildren<Button>().interactable = false;
        }
    }
}
