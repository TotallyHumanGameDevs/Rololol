using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public float speed;

    public IEnumerator Triggered(){
        transform.localPosition = new Vector3(0f, transform.position.y + speed * Time.deltaTime, 0f);
        if(transform.localPosition.y < 0.04f)
            yield return null;
        transform.localPosition = new Vector3(0f, 0.04f, 0f);
    }
}
