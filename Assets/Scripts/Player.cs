using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _fireRate = 0.02f;
    private float _canFire = -1f;


    // Start is called before the first frame update
    void Start()
    {
        // assign the current player position = a new position(0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        SpawnLaser();
    }



    void PlayerMovement()
    {
        //Set the Player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime;
        transform.Translate(direction);

        //Set the Player Bound
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.1f, 9.1f), Mathf.Clamp(transform.position.y, -3.9f, 5.8f), 0);

    }

    void SpawnLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.85f, 0), Quaternion.identity);
        }
    }


}
