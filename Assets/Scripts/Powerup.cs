using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    // id for powerups
    // 0 = triple shot
    // 1 = speed
    // 2 = shields
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private AudioClip _clip;

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //move down @ speed 3
        // when leave screen, destroy object

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.0f)
        {
            Destroy(this.gameObject);
        }

    }



    //on triggercollision
    // only to be collectable by player
    // on collect = destroy

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //handle to component
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (player != null)
            {

                
                // else if powerupID = 1
                // play speed powerup
                // else if powerup is 2
                // shieds powerup


                // switch case insetad of multiple "if else"
                switch(powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        Debug.Log("Tripleshot collected");
                        break;
                    case 1:
                        player.SpeedPowerupActive();
                        Debug.Log("Speed boost collected");
                        break;
                    case 2:
                        player.ShieldPowerupActive();
                        Debug.Log("Shield collected");
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                        
                }
                
               
            }
            Destroy(this.gameObject);

        }
    }




    





}
