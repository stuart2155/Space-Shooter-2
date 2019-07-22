using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Start is called before the first frame update
    // speed variable
    [SerializeField]
    private float _speed = 8f;


    void Update()
    {
        //translate laser up

        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        // if laser y > 12: destroy object
        if (transform.position.y > 12f )
        {
            //check if object has a parent
            //destroy parent too
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            
            Destroy(this.gameObject);
        }




    }
}
