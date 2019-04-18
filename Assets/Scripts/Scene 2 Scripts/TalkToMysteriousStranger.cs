using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(VRInteractiveItem))]
public class TalkToMysteriousStranger : MonoBehaviour
{
        public Image playerReticle;

    // player collectable ctrl
    PlayerCollectableCtrl playerCollect;

    // vr interactive item
    VRInteractiveItem vrItem;

    Color32 originalColor;

    public AudioClip extraAudio;

    public GameObject wand;

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
        playerReticle.GetComponent<Image>().color = flag ? new Color32(124, 252, 0, 100) : originalColor;
        GetComponent<Renderer>().material.SetFloat("_Outline", flag ? 0.002f : 0f);
    }

    void HandleClick()
    {
        if (Vector3.Distance(transform.position, playerCollect.gameObject.transform.position) / 3 <= playerCollect.maxDistance)
        {
            StartCoroutine(HandleAudio());
        }
    }

    IEnumerator HandleAudio() {
        AudioSource aSource = this.GetComponent<AudioSource>();
        aSource.Play(0);
        Debug.Log("Waiting for... " + aSource.clip.length);
        yield return new WaitForSeconds(aSource.clip.length);
        aSource.PlayOneShot(extraAudio, 0.7F);
        wand.SetActive(!wand.activeSelf);
    }
}
