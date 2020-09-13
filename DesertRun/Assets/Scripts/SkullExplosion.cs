using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullExplosion : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        SkullBalloon sb = transform.GetComponentInParent<SkullBalloon>();
        sb.ReturnToInitialPosition();
        sb.Deactivate();
        transform.GetComponentInParent<SpriteRenderer>().enabled = true;
    }
}
