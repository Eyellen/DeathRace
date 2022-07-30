using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[DisallowMultipleComponent]
public class SpawnManager : NetworkBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [SerializeField] private GameObject[] _carPrefabs;

    [field: SerializeField] public Transform[] SpawnPositions { get; set; }
    private int _spawnPositionIndex = 0;

    private void Awake()
    {
        InitializeInstance();
        //InitializeSpawnPositions();
    }

    private void InitializeInstance()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
#if UNITY_EDITOR || DEBUG_BUILD
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
        SpawnPositions = new Transform[positions.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            SpawnPositions[i] = positions[i].transform;
        }
    }

    public void SpawnLocalPlayer()
    {
        CmdSpawn((uint)Player.LocalPlayer.SelectedCarIndex, Player.LocalPlayer.gameObject);
    }

    [Command(requiresAuthority = false)]
    private void CmdSpawn(GameObject carPrefab, GameObject ownerPlayer)
    {
#if UNITY_EDITOR || DEBUG_BUILD
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

        Transform spawnPositionTransform = SpawnPositions[_spawnPositionIndex];

        GameObject car = Instantiate(carPrefab,
            spawnPositionTransform.position,
            spawnPositionTransform.rotation);
        NetworkServer.Spawn(car, ownerPlayer);

        _spawnPositionIndex = (_spawnPositionIndex + 1) % SpawnPositions.Length;

        NetworkConnection connection = ownerPlayer.GetComponent<Player>().connectionToClient;
        TargetSetCameraTarget(connection, car);
    }

    [Command(requiresAuthority = false)]
    private void CmdSpawn(uint carIndex, GameObject ownerPlayer)
    {
        if(carIndex >= _carPrefabs.Length)
        {
#if UNITY_EDITOR || DEBUG_BUILD
            Debug.LogError("carIndex out of range of carPrefabs array.");
#endif
            return;
        }

        Transform spawnPositionTransform = SpawnPositions[_spawnPositionIndex];

        GameObject car = Instantiate(_carPrefabs[carIndex],
            spawnPositionTransform.position,
            spawnPositionTransform.rotation);
        NetworkServer.Spawn(car, ownerPlayer);

        _spawnPositionIndex = (_spawnPositionIndex + 1) % SpawnPositions.Length;

        NetworkConnection connection = ownerPlayer.GetComponent<Player>().connectionToClient;
        TargetSetCameraTarget(connection, car);
    }

    [TargetRpc]
    private void TargetSetCameraTarget(NetworkConnection target, GameObject car)
    {
        Player.LocalPlayer.Car = car;
        Player.LocalPlayer.CameraManager.SetThirdPersonCamera(car.transform);
    }

    public void DestroyCurrentCar()
    {
        CmdDestroyCurrentCar(Player.LocalPlayer.Car);
    }

    [Command(requiresAuthority = false)]
    private void CmdDestroyCurrentCar(GameObject currentCar)
    {
        NetworkServer.Destroy(currentCar);
    }

    [Server]
    public void RespawnAllPlayers()
    {
        RemoveAllPlayersCars();
        _spawnPositionIndex = 0;

        Player[] players = FindObjectsOfType<Player>();

        foreach (var player in players)
        {
            if (player.SelectedCarIndex == -1) continue;

            CmdSpawn((uint)player.SelectedCarIndex, player.gameObject);
        }
    }

    [Server]
    private void RemoveAllPlayersCars()
    {
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");

        foreach (var car in cars)
        {
            NetworkServer.Destroy(car);
        }
    }
}
