using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAlien : MenuElement
{
    Vector3 centerPos;
    Vector3 popPos = new Vector3(0, 0, -1);
    Vector3 prePopPos;
    List<string> popDirection;
    readonly float maxRotation = 20f;
    float currRotation = 0f;
    float moveTimer = 0f;
    readonly float moveTimerMax = 0.2f;
    readonly float popSpeed = 600f;
    float maxPopTravelDist = 50f;
    float popTravelDist = 0f;
    float currMovement = 0f;
    int currDirIndex = 0;
    [SerializeField] float deltaRotation = 0.1f;
    bool tiltLeft = true;
    bool popping = false;
    bool finishingPop = false;
    List<List<string>> directionList = new List<List<string>>() {
        new List<string> { "up" }, new List<string> { "up", "right" }, new List<string> { "right" }, new List<string> { "right", "down" },
        new List<string> { "down" }, new List<string> { "down", "left" }, new List<string> { "left" }, new List<string> { "left", "up" }
    };
    List<string> currDir = new List<string>();

    void Start()
    {
        centerPos = transform.position;
        currRotation = transform.rotation.z;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (exiting || stopped)
        {
            return;
        }

        if (!popping)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer > moveTimerMax)
            {
                moveTimer = 0f;

                currMovement = Random.Range(1500f, 2000f) * Time.deltaTime;
                currDir = SwitchDirections(); 
                transform.position = new Vector3(
                    currDir.Contains("left") ? centerPos.x - currMovement : (currDir.Contains("right") ? centerPos.x + currMovement : centerPos.x),
                    currDir.Contains("up") ? centerPos.y + currMovement : (currDir.Contains("down") ? centerPos.y - currMovement : centerPos.y),
                    centerPos.z);
            }

            if (currRotation <= -maxRotation)
            {
                tiltLeft = true;
            }
            else if (currRotation >= maxRotation)
            {
                tiltLeft = false;
            }

            if (tiltLeft)
            {
                currRotation += deltaRotation;
            }
            else
            {
                currRotation -= deltaRotation;
            }

            transform.Rotate(0, 0, tiltLeft ? deltaRotation : -deltaRotation, Space.Self);
        }
        else
        {
            float speed = popSpeed * Time.deltaTime;
            if (popPos.z == -1)
            {
                maxPopTravelDist = Random.Range(200f, 250f);
                popDirection = directionList[Random.Range(0, directionList.Count - 1)];
                popPos = new Vector3(
                    currDir.Contains("left") ? centerPos.x - maxPopTravelDist : (currDir.Contains("right") ? centerPos.x + maxPopTravelDist : centerPos.x),
                    currDir.Contains("up") ? centerPos.y + maxPopTravelDist : (currDir.Contains("down") ? centerPos.y - maxPopTravelDist : centerPos.y),
                    centerPos.z);
                prePopPos = transform.position;
            }

            if (!finishingPop && popTravelDist <= maxPopTravelDist)
            {
                popTravelDist += speed;
                transform.Translate(
                        currDir.Contains("left") ? -speed : (currDir.Contains("right") ? speed : 0),
                        currDir.Contains("up") ? speed : (currDir.Contains("down") ? -speed : 0),
                        0);
            }
            else if (!finishingPop && popTravelDist > maxPopTravelDist)
            {
                finishingPop = true;
                popTravelDist = 0;
            }

            if (finishingPop && popTravelDist <= maxPopTravelDist)
            {
                popTravelDist += speed;
                transform.Translate(
                        currDir.Contains("left") ? speed : (currDir.Contains("right") ? -speed : 0),
                        currDir.Contains("up") ? -speed : (currDir.Contains("down") ? speed : 0),
                        0);
            }
            else if (finishingPop && popTravelDist > maxPopTravelDist)
            {
                if (!transform.position.Equals(prePopPos))
                {
                    transform.position = prePopPos;
                }
                moveTimer = 0f;
                popTravelDist = 0f;
                finishingPop = false;
                popping = false;
                popPos = new Vector3(0, 0, -1);
            }

        }
    }

    public void Pop()
    {
        if (!popping)
        {
            popping = true;
        }
    }

    private List<string> SwitchDirections()
    {
        currDirIndex++;
        if (currDirIndex >= directionList.Count - 1)
        {
            currDirIndex = 0;
        }

        return directionList[currDirIndex];
    }
}
