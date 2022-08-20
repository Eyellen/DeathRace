using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Mirror;

[DisallowMultipleComponent]
public class SpawnManager : NetworkBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [SerializeField] private GameObject[] _carPrefabs;

    // The key in this dictionary is netId of spawnedCar
    private readonly SyncDictionary<uint, GameObject> _spawnedCarsStorage = new SyncDictionary<uint, GameObject>();
    public ReadOnlyDictionary<uint, GameObject> SpawnedCars => new ReadOnlyDictionary<uint, GameObject>(_spawnedCarsStorage);

    [field: SerializeField] public Transform[] SpawnPositions { get; set; }
    private int _spawnPositionIndex = 0;

    public System.Action OnLocalCarSpawned;
    public System.Action<uint> OnLocalCarSpawnedIndex;
    public System.Action OnLocalCarDestroyed;

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
        _spawnedCarsStorage[car.GetComponent<CarInfo>().netId] = car;
        car.GetComponent<CarInfo>().Player = ownerPlayer.GetComponent<Player>();
        ownerPlayer.GetComponent<Player>().Car = car;

        _spawnPositionIndex = (_spawnPositionIndex + 1) % SpawnPositions.Length;

        NetworkConnection connection = ownerPlayer.GetComponent<Player>().connectionToClient;
        TargetOnLocalCarSpawned(connection, car, carIndex);
    }

    [TargetRpc]
    private void TargetOnLocalCarSpawned(NetworkConnection target, GameObject car, uint carIndex)
    {
        GameCanvas.Instance.SetActiveHUD(true);
        StartCoroutine(OnLocalCarSpawnedCoroutine(carIndex));
        //Player.LocalPlayer.Car = car;
        Player.LocalPlayer.CameraManager.SetThirdPersonCamera(car.transform);
    }

    /// <summary>
    /// This method written to prevent errors that appear before car being spawned
    /// It waits 1 frame (till Car will be Instantiated) and then calls OnLocalCarSpawned
    /// </summary>
    private IEnumerator OnLocalCarSpawnedCoroutine(uint carIndex)
    {
        yield return new WaitForEndOfFrame();

        OnLocalCarSpawned?.Invoke();
        OnLocalCarSpawnedIndex?.Invoke(carIndex);
    }

    [TargetRpc]
    public void TargetOnLocalCarDestroyed(NetworkConnection target)
    {
        StartCoroutine(OnLocalCarDestroyedCoroutine());
    }

    /// <summary>
    /// This method written to prevent errors that appear before car being spawned
    /// It waits 1 frame (till Car will be Destroyed) and then calls OnLocalCarDestroyed
    /// </summary>
    private IEnumerator OnLocalCarDestroyedCoroutine()
    {
        yield return new WaitForEndOfFrame();

        OnLocalCarDestroyed?.Invoke();
    }

    /// <summary>
    /// Removes car without explosion effect and without leaving a DestroyedCar
    /// </summary>
    public void RemoveCurrentCar()
    {
        CmdRemoveCar(Player.LocalPlayer.Car);
    }

    /// <summary>
    /// Server Command. Removes car without explosion effect and without leaving a DestroyedCar
    /// </summary>
    /// <param name="car"></param>
    [Command(requiresAuthority = false)]
    private void CmdRemoveCar(GameObject car)
    {
        _spawnedCarsStorage.Remove(car.GetComponent<CarInfo>().netId);
        NetworkServer.Destroy(car);
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

    public void RemoveCarFromSpawnedCars(uint netId)
    {
        _spawnedCarsStorage.Remove(netId);
    }
}
