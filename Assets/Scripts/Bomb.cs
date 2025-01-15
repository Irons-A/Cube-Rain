using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Bomb : BaseObject
{
    public const float DefaultOpacity = 1f;
    public const float TargetOpacity = 0f;

    [SerializeField] private float _explosionRadius = 20;
    [SerializeField] private float _explosionForce = 600;

    private MeshRenderer _renderer;

    protected override void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        base.Awake();
    }

    private void OnEnable()
    {
        StopAllCoroutines();

        float delay = Random.Range(_minLifetime, _maxLifetime);
        StartCoroutine(DeactivateRoutine(delay));
        StartCoroutine(FadeOutRoutine(delay));
    }

    private void Explode()
    {
        foreach (Rigidbody affectedObject in GetAffectedObjects())
        {
            affectedObject.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
        }
    }

    private List<Rigidbody> GetAffectedObjects()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

        List<Rigidbody> hitObjects = new();

        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody != null)
            {
                hitObjects.Add(hit.attachedRigidbody);
            }
        }

        return hitObjects;
    }

    private IEnumerator DeactivateRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        Explode();
        CommandRelease(this);
    }

    private IEnumerator FadeOutRoutine(float delay)
    {
        Color color = _renderer.material.color;
        float opacity = DefaultOpacity;
        float fadeSpeed = 1f / delay;

        while (opacity > TargetOpacity)
        {
            opacity -= fadeSpeed * Time.deltaTime;
            color.a = opacity;
            _renderer.material.color = color;

            yield return null;
        }

        opacity = TargetOpacity;
        color.a = opacity;
        _renderer.material.color = color;
    }
}
