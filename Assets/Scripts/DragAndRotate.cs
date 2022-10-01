using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndRotate : MonoBehaviour
{
    Vector3 mPreviousPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;
    public float rotateSpeed;
    bool rotating = false;


    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetMouseButton(0)){
            mPosDelta = Input.mousePosition - mPreviousPos;
            transform.parent.Rotate(transform.position - transform.GetChild(0).position, -Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
            transform.parent.Rotate(transform.position - transform.GetChild(1).position, Vector3.Dot(mPosDelta, Camera.main.transform.up), Space.World);
        }*/

        if(Input.GetKey(KeyCode.D) && !rotating){
            StartCoroutine(Rotate(transform.parent.up, -1f));
        }
        else if(Input.GetKey(KeyCode.A) && !rotating){
            StartCoroutine(Rotate(transform.parent.up, 1f));
        }
        else if(Input.GetKey(KeyCode.Q) && !rotating){
            StartCoroutine(Rotate(transform.parent.forward, 1f));
        }
        else if(Input.GetKey(KeyCode.E) && !rotating){
            StartCoroutine(Rotate(transform.parent.forward, -1f));
        }
        else if(Input.GetKey(KeyCode.W) && !rotating){
            StartCoroutine(Rotate(transform.parent.right, 1f));
        }
        else if(Input.GetKey(KeyCode.S) && !rotating){
            StartCoroutine(Rotate(transform.parent.right, -1f));
        }


        mPreviousPos = Input.mousePosition;
    }

    IEnumerator Rotate(Vector3 axis, float angle){
        rotating = true;
        float remainingAngle = 90f;

        while(remainingAngle > 0f){
            float rotationalAngle = Mathf.Min(Time.deltaTime * rotateSpeed, remainingAngle);
            transform.RotateAround(transform.position, axis, rotationalAngle * angle);
            remainingAngle -= rotationalAngle;
            yield return null;
        }

        rotating = false;
    }
}
