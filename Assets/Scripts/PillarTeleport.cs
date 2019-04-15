using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

[RequireComponent(typeof(VRInteractiveItem))]
public class PillarTeleport : MonoBehaviour
{
    // teleportation target game object
    public GameObject teleportDestination;

    public GameObject player;

    // player collectable ctrl
    PlayerCollectableCtrl playerCollect;

    // vr interactive item
    VRInteractiveItem vrItem;

    void Awake()
    {
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
        Debug.Log("SEEEING IT");
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
        GetComponent<Renderer>().material.SetFloat("_Outline", flag ? 0.002f : 0f);
    }

    void HandleClick()
    {
        if (Vector3.Distance(transform.position, playerCollect.gameObject.transform.position)/3 <= playerCollect.maxDistance)
        {
            //teleport player to secret platform
            player.transform.position = teleportDestination.transform.position;
        }
    }
}
