using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMove : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float endPosX;
    [SerializeField] Vector3 initPos;

    private void Update()
    {
        transform.Translate(Vector3.right * -speed * Time.deltaTime);

        if(this.transform.position.x <= endPosX)
        {
            this.transform.position = initPos;
        }
    }
}
