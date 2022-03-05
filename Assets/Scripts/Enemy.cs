using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;
    private Animator _anim;

    
    private AudioSource _audiosource;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    [SerializeField]
    private GameObject _enemyLaserprefab;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audiosource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("Player is  NULL");
        }
        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("The animator  is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
           GameObject enemyLaser = Instantiate(_enemyLaserprefab, transform.position, Quaternion.identity);
           Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

           for (int i= 0; i<lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 5.8f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if(player != null)
            {
               player.Damage();               
            }
            _anim.SetTrigger("OnEnemyDeath");
            _audiosource.Play();
            Destroy(this.gameObject, 2f);

        }

        if(other.tag == "Laser")
        {
            Destroy(other);
            _player.AddScore(10);
            _anim.SetTrigger("OnEnemyDeath");
            _audiosource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2f);
        }
    }

}
