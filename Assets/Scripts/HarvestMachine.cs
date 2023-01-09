using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HarvestMachine : MonoBehaviour
{
    public float speedupTimer;
    public float speedupInterval = 15.0f;
    public float speedupIntervalOff = 15.0f;

    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        speedupTimer = Random.Range(speedupInterval, speedupInterval + speedupIntervalOff);
    }

    // Update is called once per frame
    void Update()
    {
        speedupTimer -= Time.deltaTime;
        if (speedupTimer < 0.0f)
        {
            speedupTimer = Random.Range(speedupInterval, speedupInterval + speedupIntervalOff);
            Sprint();
        }
    }

    public void ShowUp()
    {
        animator.SetTrigger("ShowUp");
    }

    public void Sprint()
    {
        animator.SetTrigger("Sprint");
    }
}
