using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[DisallowMultipleComponent]
public class SpawnManager : NetworkBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [SerializeField] private GameObject[] _carPrefabs;
    public uint SelectedCarIndex { get; set; } = 0;


    [SerializeField] private Transform[] _spawnPositions;
    private int _spawnPositionIndex = 0;

    private void Awake()
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

    public void Spawn()
    {
        CmdSpawn(SelectedCarIndex, Player.LocalPlayer.gameObject);
    }

    [Command(requiresAuthority = false)]
    public void CmdSpawn(GameObject carPrefab, GameObject ownerPlayer)
    {
#if UNITY_EDITOR
        if (_carPrefabs.Length <= 0)
        {
            Debug.LogError($"CarPrefabs in {nameof(SpawnManager)} doesn't contain {carPrefab}." +
                    $"Posibly you didn't add {carPrefab} to CarPrefabs or you are trying to spawn wrong prefab.");
            return;
        }
        for (int i = 0; i < _carPrefabs.Length; i++)
        {
            if (carPrefab == _carPrefabs[i]) break;

            if (i + 1 == _carPrefabs.Length)
            {
                Debug.LogError($"CarPrefabs in {nameof(SpawnManager)} doesn't contain {carPrefab}." +
                    $"Posibly you didn't add {carPrefab} to CarPrefabs or you are trying to spawn wrong prefab.");
                return;
            }
        }
#endif

        Transform spawnPositionTransform = _spawnPositions[_spawnPositionIndex];

        GameObject car = Instantiate(carPrefab,
            spawnPositionTransform.position,
            spawnPositionTransform.rotation);
        NetworkServer.Spawn(car, ownerPlayer);

        _spawnPositionIndex = (_spawnPositionIndex + 1) % _spawnPositions.Length;

        NetworkConnection connection = ownerPlayer.GetComponent<Player>().connectionToClient;
        TargetSetCameraTarget(connection, car);
    }

    [Command(requiresAuthority = false)]
    public void CmdSpawn(uint carIndex, GameObject ownerPlayer)
    {
#if UNITY_EDITOR
        if(carIndex >= _carPrefabs.Length)
        {
            Debug.LogError("carIndex out of range of carPrefabs array.");
            return;
        }
#endif

        Transform spawnPositionTransform = _spawnPositions[_spawnPositionIndex];

        GameObject car = Instantiate(_carPrefabs[carIndex],
            spawnPositionTransform.position,
            spawnPositionTransform.rotation);
        NetworkServer.Spawn(car, ownerPlayer);

        _spawnPositionIndex = (_spawnPositionIndex + 1) % _spawnPositions.Length;

        NetworkConnection connection = ownerPlayer.GetComponent<Player>().connectionToClient;
        TargetSetCameraTarget(connection, car);
    }

    [TargetRpc]
    private void TargetSetCameraTarget(NetworkConnection target, GameObject car)
    {
        Player.LocalPlayer.Car = car;
        Player.LocalPlayer.CameraManager.SetThirdPersonCamera(car.transform);
    }
}
