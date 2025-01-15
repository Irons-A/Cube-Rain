using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class CubeSpawner : Spawner
{
    [SerializeField] protected float _spawnHeight = 10f;
    [SerializeField] protected float _minXSpawnPoint = -5f;
    [SerializeField] protected float _maxXSpawnPoint = 5f;
    [SerializeField] protected float _minZSpawnPoint = -5f;
    [SerializeField] protected float _maxZSpawnPoint = 5f;
    [SerializeField] protected float _spawnRate = 1f;

    public event Action<Vector3> ObjectDisabled;

    protected override void Start()
    {
        StartCoroutine(SpawnRoutine());
        base.Start();
    }

    protected override void OnGetFromPool(BaseObject cube)
    {
        float xSpawnPoint = Random.Range(_minXSpawnPoint, _maxXSpawnPoint);
        float zSpawnPoint = Random.Range(_minZSpawnPoint, _maxZSpawnPoint);

        cube.gameObject.SetActive(true);
        cube.transform.position = new Vector3(xSpawnPoint, _spawnHeight, zSpawnPoint);
        base.OnGetFromPool(cube);
    }

    protected override void OnReleaseToPool(BaseObject objectUnit)
    {
        ObjectDisabled?.Invoke(objectUnit.transform.position);
        base.OnReleaseToPool(objectUnit);
    }

    protected IEnumerator SpawnRoutine()
    {
        _spawnFrequency = new WaitForSeconds(_spawnRate);

        while (_isSpawnerActive)
        {
            _pool.Get();

            yield return _spawnFrequency;
        }
    }
}
