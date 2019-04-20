using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    public GameObject mysteriousStranger;

    public AudioClip daintyMuggle;
    public AudioClip takeItAndGo;

    public GameObject textCanvas;

    bool strangerPresent = false;

    private void Awake()
    {
        AudioSource aS = GetComponent<AudioSource>();
        aS.PlayOneShot(daintyMuggle, 1F);
        aS.PlayDelayed(daintyMuggle.length);
        HandleAudio(daintyMuggle.length);
        
    }

    IEnumerator HandleAudio(float length)
    {
        yield return new WaitForSeconds(length);
        textCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Position: " + transform.position + " in range: " + (transform.position.x > -40 && transform.position.z < 155));
        if (transform.position.x > -40 && transform.position.z < 155)
        {
            //start barking
            AudioSource hound = GameObject.Find("Casparian Hound").GetComponent<AudioSource>();

            if (!hound.isPlaying)
            {
                hound.Play();
            }
        }

        if (transform.position.x > -21 && transform.position.z < 139)
        {
            if (!strangerPresent) {
                //stop barking
                GameObject.Find("Casparian Hound").GetComponent<AudioSource>().Stop();

                //move mysterious stranger, and play audio
                GameObject newPosition = GameObject.Find("Mysterious Stranger Cave Position");
                mysteriousStranger.transform.position = newPosition.transform.position;
                mysteriousStranger.transform.rotation *= Quaternion.Euler(0, 180f, 0);

                //play electrical noises
                newPosition.GetComponent<AudioSource>().Play();

                //play audio for MS
                AudioSource audioSource = mysteriousStranger.GetComponent<AudioSource>();
                audioSource.PlayOneShot(takeItAndGo, 1F);
                strangerPresent = true;
            }
        }
    }
}
