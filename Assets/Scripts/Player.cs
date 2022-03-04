using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6f;
    private float _speedMultiplier = 2f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleshotPrefab;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private GameObject _rightengine, _leftengine;

    [SerializeField]
    private float _fireRate = 0.02f;
    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isShieldActive = false;

    [SerializeField]
    private int _score;


    // Start is called before the first frame update
    void Start()
    {
        // assign the current player position = a new position(0, 0, 0)
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        if(_uiManager == null)
        {
            Debug.LogError(" The UIManager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
        
    }



    void PlayerMovement()
    {
        //Set the Player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

            transform.Translate(direction * _speed * Time.deltaTime);

        //Set the Player Bound
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.1f, 9.1f), Mathf.Clamp(transform.position.y, -3.9f, 5.8f), 0);

    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleshotPrefab, transform.position, Quaternion.identity);

        }

        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        
    }

    public void Damage()
    {
        if(_isShieldActive == true)
        {
            StartCoroutine(ShieldPowerDownRoutine());
            return;
        }
        _lives--;

        if (_lives == 2)
        {
            _rightengine.SetActive(true);
        }

        else if (_lives == 1)
        {
            _leftengine.SetActive(true);
        }
        _uiManager.UpdateLives(_lives);

        if (_lives == 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);

        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotActivePowerDownRoutine());
    }

    IEnumerator TripleShotActivePowerDownRoutine() {

        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed = _speed * _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }
    IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isShieldActive = false;
        _shieldVisualizer.SetActive(false);

    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);

    }

    public void AddScore(int points)
    {
        _score = _score + points;
        _uiManager.UpdateScore(_score);
    }

}
