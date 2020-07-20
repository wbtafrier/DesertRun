using System.Collections.Generic;
using UnityEngine;

public class MenuAlien : MenuElement
{
    Vector2 centerPos;
    Vector2 popPos = new Vector2(0, 0);
    Vector2 prePopPos;
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
    bool popStarted = false;
    bool popping = false;
    bool finishingPop = false;
    List<List<string>> directionList = new List<List<string>>() {
        new List<string> { "up" }, new List<string> { "up", "right" }, new List<string> { "right" }, new List<string> { "right", "down" },
        new List<string> { "down" }, new List<string> { "down", "left" }, new List<string> { "left" }, new List<string> { "left", "up" }
    };
    List<string> currDir = new List<string>();

    public override void Start()
    {
        base.Start();

        centerPos = rectTransform.anchoredPosition;
        currRotation = rectTransform.rotation.z;
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
                rectTransform.anchoredPosition = new Vector2(
                    currDir.Contains("left") ? centerPos.x - currMovement : (currDir.Contains("right") ? centerPos.x + currMovement : centerPos.x),
                    currDir.Contains("up") ? centerPos.y + currMovement : (currDir.Contains("down") ? centerPos.y - currMovement : centerPos.y));
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

            rectTransform.Rotate(0, 0, tiltLeft ? deltaRotation : -deltaRotation, Space.Self);
        }
        else
        {
            float speed = popSpeed * Time.deltaTime;
            if (!popStarted)
            {
                maxPopTravelDist = Random.Range(200f, 250f);
                popDirection = directionList[Random.Range(0, directionList.Count - 1)];
                popPos = new Vector2(
                    currDir.Contains("left") ? centerPos.x - maxPopTravelDist : (currDir.Contains("right") ? centerPos.x + maxPopTravelDist : centerPos.x),
                    currDir.Contains("up") ? centerPos.y + maxPopTravelDist : (currDir.Contains("down") ? centerPos.y - maxPopTravelDist : centerPos.y));
                popStarted = true;
                prePopPos = rectTransform.anchoredPosition;
            }

            if (!finishingPop && popTravelDist <= maxPopTravelDist)
            {
                popTravelDist += speed;
                rectTransform.Translate(
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
                rectTransform.Translate(
                        currDir.Contains("left") ? speed : (currDir.Contains("right") ? -speed : 0),
                        currDir.Contains("up") ? -speed : (currDir.Contains("down") ? speed : 0),
                        0);
            }
            else if (finishingPop && popTravelDist > maxPopTravelDist)
            {
                if (!rectTransform.anchoredPosition.Equals(prePopPos))
                {
                    rectTransform.anchoredPosition = prePopPos;
                }
                moveTimer = 0f;
                popTravelDist = 0f;
                finishingPop = false;
                popping = false;
                popStarted = false;
                popPos = new Vector2(0, 0);
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
