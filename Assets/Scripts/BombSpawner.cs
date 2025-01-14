using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : Spawner
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    protected override void OnEnable()
    {
        _cubeSpawner.ObjectDisabled += SpawnObject;
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        _cubeSpawner.ObjectDisabled -= SpawnObject;
        base.OnDisable();
    }

    protected override void OnGetFromPool(BaseObject bomb)
    {
        bomb.gameObject.SetActive(true);
    }

    private void SpawnObject(Vector3 targetPosition)
    {
        _pool.Get(); //???
    }
}
