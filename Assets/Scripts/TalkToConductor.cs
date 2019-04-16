using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(VRInteractiveItem))]
public class TalkToConductor : MonoBehaviour
{
    public Image playerReticle;

    // player collectable ctrl
    PlayerCollectableCtrl playerCollect;

    // vr interactive item
    VRInteractiveItem vrItem;

    Color32 originalColor;

    void Awake()
    {
        originalColor = playerReticle.GetComponent<Image>().color;

        // get the vr interactive item component
        vrItem = GetComponent<VRInteractiveItem>();

        // get the PlayerCollectableCtr object
        playerCollect = FindObjectOfType<PlayerCollectableCtrl>();

        if (playerCollect == null)
        {
            Debug.LogError("There needs to be a PlayerCollectableCtrl item in your scene");
        }
    }


    void OnEnable()
    {
        // subscribe to events
        vrItem.OnOver += HandleOver;
        vrItem.OnOut += HandleOut;
        vrItem.OnClick += HandleClick;
    }

    void OnDisable()
    {
        // unsubscribe to events
        vrItem.OnOver -= HandleOver;
        vrItem.OnOut -= HandleOut;
        vrItem.OnClick -= HandleClick;
    }


    void HandleOut()
    {
        //the player collectable ctr knows we are looking away
        playerCollect.SelectionOut();

        // unselect the item
        Highlight(false);
    }

    void HandleOver()
    {
        Debug.Log("SEEING THIS");
        if (Vector3.Distance(transform.position, playerCollect.gameObject.transform.position) <= playerCollect.maxDistance)
        {
            //the player collectable ctr knows we are selecting
            playerCollect.SelectionOver();

            // highlight the item
            Highlight(true);
        }
    }

    void Highlight(bool flag)
    {
        Debug.Log("HIGHLIGHTING " + flag);
        playerReticle.GetComponent<Image>().color = flag ? new Color32(124, 252, 0, 100) : originalColor;
        GetComponent<Renderer>().material.SetFloat("_Outline", flag ? 0.002f : 0f);
    }

    void HandleClick()
    {
        Debug.Log("CLICKING = " + Vector3.Distance(transform.position, playerCollect.gameObject.transform.position));
        if (Vector3.Distance(transform.position, playerCollect.gameObject.transform.position) / 3 <= playerCollect.maxDistance)
        {
            Initiate.Fade("TrainStation", Color.black, 2.0f);
            Initiate.DoneFading();
            SceneManager.LoadScene("houndScene");
        }
    }
}
