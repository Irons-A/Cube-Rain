using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnerStatistics : MonoBehaviour
{
    public const string AlltimeObjectsTitle = "Alltime objects: ";
    public const string SpawnedObjectsTitle = "Spawned objects: ";
    public const string ActiveObjectsTitle = "Active objects: ";

    [SerializeField] private Spawner _spawner;
    [SerializeField] private TMP_Text _alltimeObjects;
    [SerializeField] private TMP_Text _spawnedObjects;
    [SerializeField] private TMP_Text _activeObjects;

    private void OnEnable()
    {
        _spawner.StatiscticUpdated += UpdateStats;
    }

    private void OnDisable()
    {
        _spawner.StatiscticUpdated -= UpdateStats;
    }

    private void UpdateStats()
    {
        _alltimeObjects.text = $"{AlltimeObjectsTitle} {_spawner._alltimeObjects}";
        _spawnedObjects.text = $"{SpawnedObjectsTitle} {_spawner._createdObjects}";
        _activeObjects.text = $"{ActiveObjectsTitle} {_spawner._activeObjects}";
    }
}
