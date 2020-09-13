using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBalloon : DesertObject
{
    //[SerializeField] GameObject sparkleProp = default;

    private static readonly float BALLOON_Y_SPEED = 0.4f;
    private bool down = false;
    private float upPosY, downPosY, targetPosY;
    //private GameObject sparkleObj;

    //private ParticleSystem sparkles;
    //private AudioSource dingSfx;
    private SpriteRenderer spriteRenderer;

    public override void Restart()
    {
        base.Restart();
        //SetDingVolume(GameStateMachine.GetSoundVolume());
        down = false;
        targetPosY = upPosY;
        spriteRenderer.enabled = true;
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

        spriteRenderer = GetComponent<SpriteRenderer>();
        //sparkleObj = sparkleProp;
        //sparkles = sparkleObj.GetComponent<ParticleSystem>();

        //dingSfx = GetComponent<AudioSource>();
        //SetDingVolume(GameStateMachine.GetSoundVolume());
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        //float pitch = GameController.GetBalloonPitch();
        //if (pitch != dingSfx.pitch)
        //{
        //    dingSfx.pitch = pitch;
        //}

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

        //if (activated && !sparkleObj.activeSelf)
        //{
        //    sparkleObj.SetActive(true);
        //}

        //if (transform.position.x < STOP_X_POS)
        //{
        //    sparkles.Stop();
        //}

        //if (sparkleObj.activeSelf)
        //{
        //    Vector3 balloonPos = transform.position;
        //    float x = balloonPos.x;
        //    float y = balloonPos.y + 0.6f;
        //    float z = sparkleObj.transform.position.z;
        //    sparkleObj.transform.position = new Vector3(x, y, z);
        //}
    }
    
    //private void SetDingVolume(float vol)
    //{
    //    dingSfx.volume = vol;
    //    dingSfx.pitch = 1f;
    //}

    //public void ResetSparkles()
    //{
    //    sparkleObj.transform.position = transform.position;
    //    sparkles.Stop();
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform t = collision.transform;
        if (t.CompareTag("Player"))
        {
            //if (!GameController.IsReloInvincible())
            //{
            //    dingSfx.Play();
            //    GameController.AddCoin();
            //}
            //sparkles.Stop();
            spriteRenderer.enabled = false;
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            if (!GameController.IsReloInvincible())
            {
                collision.attachedRigidbody.constraints = RigidbodyConstraints2D.None;
                GameController.SetGameOver();
            }
        }
    }
}
