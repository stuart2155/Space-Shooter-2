using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;

    private Player _player;
    //handle to animator componenent
    private Animator _anim;
    private AudioSource _audioSource;




        
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        //null check player
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL");
        }




        //assign the component to Anim
    }

    // Update is called once per frame
    void Update()
    {
        // move down at 4ms-1
        // if bottom, respawn at top with new random x

        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        // if laser y > 12: destroy object
        if (transform.position.y < -6.0f)
        {
            float RandomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(RandomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if other is player, damage the player, destroy enemy
        if (other.tag == "Player")
        {
            //damage player
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            //trigger animation
            _anim.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.5f);
            
        }
        

        //if other is laser, destroy laser, destroy enemy
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            // add 10 to score
            if (_player != null)
            {
                _player.AddScore(10);
            }

            //trigger animation
            _anim.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.5f);
        }


    }

}
