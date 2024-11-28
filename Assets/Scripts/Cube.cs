using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]

public class Cube : MonoBehaviour
{
    [SerializeField] private float _minLifetime = 2f;
    [SerializeField] private float _maxLifetime = 5f;

    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private bool _isCollisionDetected = false;
    private ObjectPool<Cube> objectPool;

    public ObjectPool<Cube> ObjectPool { set => objectPool = value; }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _renderer.material.color = Color.white;
        _isCollisionDetected = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Platform platform) && !_isCollisionDetected)
        {
            float delay = Random.Range(_minLifetime, _maxLifetime);

            _isCollisionDetected = true;
            _renderer.material.color = Random.ColorHSV();
            StartCoroutine(DeactivateRoutine(delay));
        }
    }

    private IEnumerator DeactivateRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        _rigidbody.velocity = new Vector3(0f, 0f, 0f);
        _rigidbody.velocity = new Vector3(0f, 0f, 0f);
        objectPool.Release(this);
    }
}
