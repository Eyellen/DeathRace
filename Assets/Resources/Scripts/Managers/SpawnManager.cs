using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[DisallowMultipleComponent]
public class SpawnManager : NetworkBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [SerializeField] private GameObject[] _carPrefabs;
    public GameObject SelectedCar { get; set; }

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
    public void CmdSpawn(uint carIndex, GameObject ownerPlayer)
    {
        Transform spawnPositionTransform = _spawnPositions[_spawnPositionIndex];

        GameObject car = Instantiate(_carPrefabs[carIndex], 
            spawnPositionTransform.position, 
            spawnPositionTransform.rotation);
        NetworkServer.Spawn(car, ownerPlayer);

        _spawnPositionIndex = (_spawnPositionIndex + 1) % _spawnPositions.Length;
    }

    [Command(requiresAuthority = false)]
    public void CmdSpawn(GameObject carPrefab, GameObject ownerPlayer)
    {
#if UNITY_EDITOR
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
    }

    [Command(requiresAuthority = false)]
    public void CmdSpawn(GameObject ownerPlayer)
    {
#if UNITY_EDITOR
        for (int i = 0; i < _carPrefabs.Length; i++)
        {
            if (SelectedCar == _carPrefabs[i]) break;

            if (i + 1 == _carPrefabs.Length)
            {
                Debug.LogError($"CarPrefabs in {nameof(SpawnManager)} doesn't contain {SelectedCar}." +
                    $"Posibly you didn't add {SelectedCar} to CarPrefabs or you are trying to spawn wrong prefab.");
                return;
            }
        }
#endif

        Transform spawnPositionTransform = _spawnPositions[_spawnPositionIndex];

        GameObject car = Instantiate(SelectedCar,
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
        Player.LocalPlayer.CameraManager.SetThirdPersonCamera(car.transform);
    }
}
