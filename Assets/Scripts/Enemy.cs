using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;
    private Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

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
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -5f)
        {
            float randomX= Random.Range(-8f, 8f);
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
            Destroy(this.gameObject, 2f);

        }

        if(other.tag == "Laser")
        {
            Destroy(other);
            _player.AddScore(10);
            _anim.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2f);
        }
    }

}
