using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnManager : NetworkBehaviour
{
    public SpawnManager Instance { get; private set; }

    [SerializeField] private readonly GameObject[] _carPrefabs;

    [SerializeField] private Transform[] _spawnPositions;
    private int _spawnPositionIndex = 0;

    private void Start()
    {
        InitializeInstance();
        InitializeSpawnPositions();
    }

    private void InitializeInstance()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning($"Trying to create another one {nameof(SpawnManager)} when it's Singleton." +
                $"The duplicate of {nameof(SpawnManager)} will be destroyed");
#endif
            Destroy(gameObject);
        }
    }

    [ServerCallback]
    private void InitializeSpawnPositions()
    {
        GameObject[] positions = GameObject.FindGameObjectsWithTag("SpawnPosition");
        _spawnPositions = new Transform[positions.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            _spawnPositions[i] = positions[i].transform;
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdSpawn(GameObject carPrefab, GameObject ownerPlayer)
    {
        Transform spawnPositionTransform = _spawnPositions[_spawnPositionIndex];

        GameObject car = Instantiate(carPrefab, spawnPositionTransform.position, spawnPositionTransform.rotation);
        NetworkServer.Spawn(car, ownerPlayer);

        _spawnPositionIndex = (_spawnPositionIndex + 1) % _spawnPositions.Length;
    }
}
