using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Renderer))]

public class Cube : BaseObject
{
    private Renderer _renderer;
    private bool _isCollisionDetected = false;

    private BaseObject _parent;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _parent = GetComponentInParent<BaseObject>();
    }

    private void OnEnable()
    {
        _renderer.material.color = Color.white;
        _isCollisionDetected = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isCollisionDetected == false && collision.gameObject.TryGetComponent(out Platform platform))
        {
            float delay = Random.Range(_minLifetime, _maxLifetime);

            _isCollisionDetected = true;
            ChangeColor();
            StartCoroutine(DeactivateRoutine(delay));
        }
    }

    private void ChangeColor()
    {
        _renderer.material.color = Random.ColorHSV();
    }

    private IEnumerator DeactivateRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        CommandRelease(this);
    }
}
