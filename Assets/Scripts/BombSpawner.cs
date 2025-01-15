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
        base.OnGetFromPool(bomb);
    }

    private void SpawnObject(Vector3 targetPosition)
    {
        BaseObject bomb = _pool.Get();
        bomb.transform.position = targetPosition;
    }
}
