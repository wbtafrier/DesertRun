﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : DesertObject
{
    [SerializeField] GameObject sparkleProp;

    private static readonly float BALLOON_Y_SPEED = 0.4f;
    private bool down = false;
    private float upPosY, downPosY, targetPosY;
    private GameObject sparkleObj;

    private ParticleSystem sparkles;

    public override void Restart()
    {
        down = false;
        targetPosY = upPosY;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Vector3 pos = transform.position;
        float x = pos.x;
        float y = pos.y;
        float maxY = y + 0.2f;
        float minY = y - 0.2f;

        upPosY = maxY;
        downPosY = minY;
        targetPosY = upPosY;

        sparkleObj = sparkleProp;
        sparkles = sparkleObj.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        Vector3 pos = transform.position;
        float yPos = pos.y;
        if (down && yPos == downPosY)
        {
            down = false;
            targetPosY = upPosY;
        }
        else if (!down && yPos == upPosY)
        {
            down = true;
            targetPosY = downPosY;
        }

        Vector3 targetPos = new Vector3(pos.x, targetPosY, pos.z);

        transform.position = Vector3.MoveTowards(pos, targetPos, BALLOON_Y_SPEED * Time.deltaTime);

        if (activated && !sparkleObj.activeSelf)
        {
            sparkleObj.SetActive(true);
        }

        if (transform.position.x < STOP_X_POS)
        {
            sparkles.Stop();
        }

        if (sparkleObj.activeSelf)
        {
            Vector3 balloonPos = transform.position;
            float x = balloonPos.x;
            float y = balloonPos.y + 0.6f;
            float z = sparkleObj.transform.position.z;
            sparkleObj.transform.position = new Vector3(x, y, z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            sparkles.Stop();
            transform.position = GetInitialPosition();
            Deactivate();
        }
    }
}