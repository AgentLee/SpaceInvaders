using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour 
{
    private Transform bases;
    public bool hordeStart;
    public GameObject g;

    public AudioClip barrierActivated;
    public bool playedBarrierActivatedClip;

    bool started;

    // Use this for initialization
    void Start () {
        g = GameObject.Find("GlobalObject");

        bases = GetComponent<Transform>();

        hordeStart = false;

        playedBarrierActivatedClip = true;

        started = true;
    }

    void FixedUpdate()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        hordeStart = g.GetComponent<Global>().hordeStart;

        // Hide bases during horde
        if (hordeStart)
        {
            playedBarrierActivatedClip = false;
            foreach (Transform b in bases)
            {
                MeshRenderer render = b.gameObject.GetComponentInChildren<MeshRenderer>();
                render.enabled = false;
            }
        }
        else
        {
            foreach (Transform b in bases)
            {
                MeshRenderer render = b.gameObject.GetComponentInChildren<MeshRenderer>();
                render.enabled = true;
            }

            if(!playedBarrierActivatedClip)
            {
                AudioSource.PlayClipAtPoint(barrierActivated, this.transform.position);
                playedBarrierActivatedClip = true;
            }
        }
    }
}
