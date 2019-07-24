using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // create variable
    // public or private reference
    // data type (int, float, bool, string)
    // every variable has a name
    // optional value assigned

    [SerializeField]
    private float _speed = 8f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _laserTriplePrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score = 0;
    private UIManager _UIManager;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _tripleShotActive = false;
    [SerializeField]
    private bool _speedPowerupActive = false;
    private float _speedMultiplier = 2;
    [SerializeField]
    private bool _shieldPowerupActive = false;

    // variable ref for shield visualiser
    [SerializeField]
    private GameObject _shieldVisualiser;
    [SerializeField]
    private GameObject _rightEngine, _leftEngine;
    [SerializeField]
    private AudioClip _laserSoundClip;
    
    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        //take current position = new position (0,0,0)
        transform.position = new Vector3(0, -5, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();


        if (_spawnManager == null)
        {
            Debug.LogError("Spawn_Manager is NULL");
        }

        if (_UIManager == null)
        {
            Debug.LogError("UIManager is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }




    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            ShootLaser();
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (_speedPowerupActive == false)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);
        }
        
            // set player boundries
            // vertical -2 >< 5

            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -5f, 5f), 0);

        // horizontal -7 >< 7 with wrap (no Clamp)
        if (transform.position.x < -8f)
        {
            transform.position = new Vector3(8f, transform.position.y, 0);
        }

        if (transform.position.x > 8f)
        {
            transform.position = new Vector3(-8f, transform.position.y, 0);
        }

    }

    void ShootLaser()
    {
        _canFire = Time.time + _fireRate;

        // if tripleshotactive = true
        // fire 3 lasers
        //instantiate 3 lasers using Triple_shot prefab
        // else fire 1 laser

        if (_tripleShotActive == true)
        {
            Instantiate(_laserTriplePrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.0f, 0), Quaternion.identity);
        }

        // play laser audio clip
        _audioSource.Play();


    }

    public void Damage()
    {
        // if shields active
        // deactivate shield
        if (_shieldPowerupActive == true)
        {
            _shieldPowerupActive = false;
            // disable shield visualiser
            _shieldVisualiser.SetActive(false);
        }
        // else reduce life
        else
        {
            _lives -= 1;

            // if lives = 2 enable right engine
            // if lives = 1 enable left engine
            if (_lives ==2)
            {
                _leftEngine.SetActive(true);
            }

            if (_lives == 1)
            {
                _rightEngine.SetActive(true);
            }




            _UIManager.UpdateLives(_lives);

            if (_lives < 1)
            {
                //communicate with spawn manager
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject);
            }
        }
        



    }

    public void TripleShotActive()
    {
        //TripleShotActive = true
        // start powerdown coroutine
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
        
    }

    public void SpeedPowerupActive()
    {
        // powerup = true
        // start powerdown coroutine
        _speedPowerupActive = true;
        StartCoroutine(SpeedPowerDownRoutine());

    }

    public void ShieldPowerupActive()
    {
        // powerup = true
        // start powerdown coroutine
        _shieldPowerupActive = true;
        // enable visualiser
        _shieldVisualiser.SetActive(true);

    }


    //ienumnertor TripleShotPowerDownRoutine
    //wait 5 sec
    //set triple shot to false

    IEnumerator TripleShotPowerDownRoutine()
    {
        while (_tripleShotActive == true)
        {
            yield return new WaitForSeconds(5.0f);
            _tripleShotActive = false;
        }
        
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        while (_speedPowerupActive == true)
        {
            yield return new WaitForSeconds(5.0f);
            _speedPowerupActive = false;
        }

    }

    //method to add 10 to score
    //communicate with UI to update score
    public void AddScore(int points)
    {
        _score += points;
        _UIManager.UpdateScore(_score);
        
    }

}