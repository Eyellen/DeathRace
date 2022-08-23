using System;
using System.Net;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
using Mirror.Discovery;

/*
    Documentation: https://mirror-networking.gitbook.io/docs/components/network-discovery
    API Reference: https://mirror-networking.com/docs/api/Mirror.Discovery.NetworkDiscovery.html
*/

public class DiscoveryRequest : NetworkMessage
{
    // Add properties for whatever information you want sent by clients
    // in their broadcast messages that servers will consume.
}

public class DiscoveryResponse : NetworkMessage
{
    // The server that sent this
    // this is a property so that it is not serialized,  but the
    // client fills this up after we receive it
    public IPEndPoint EndPoint { get; set; }

    public Uri uri;

    // Prevent duplicate server appearance when a connection can be made via LAN on multiple NICs
    public long serverId;



    // Add properties for whatever information you want the server to return to
    // clients for them to display or consume for establishing a connection.
    public string GameVersion;
    public string ServerName;
    public int GameModeIndex;
    public int MaxPlayersCount;
    public int CurrentPlayersCount;
    public int MaxPing;
}

[Serializable]
public class ServerFoundUnityEvent : UnityEvent<DiscoveryResponse> { };

[DisallowMultipleComponent]
public class NewNetworkDiscovery : NetworkDiscoveryBase<DiscoveryRequest, DiscoveryResponse>
{
    #region Server

    public long ServerId { get; private set; }

    [Tooltip("Transport to be advertised during discovery")]
    public Transport transport;

    [Tooltip("Invoked when a server is found")]
    public ServerFoundUnityEvent OnServerFound;

    public override void Start()
    {
        ServerId = RandomLong();

        // active transport gets initialized in awake
        // so make sure we set it here in Start()  (after awakes)
        // Or just let the user assign it in the inspector
        if (transport == null)
            transport = Transport.activeTransport;

        base.Start();
    }

    /// <summary>
    /// Reply to the client to inform it of this server
    /// </summary>
    /// <remarks>
    /// Override if you wish to ignore server requests based on
    /// custom criteria such as language, full server game mode or difficulty
    /// </remarks>
    /// <param name="request">Request coming from client</param>
    /// <param name="endpoint">Address of the client that sent the request</param>
    protected override void ProcessClientRequest(DiscoveryRequest request, IPEndPoint endpoint)
    {
        base.ProcessClientRequest(request, endpoint);
    }

    /// <summary>
    /// Process the request from a client
    /// </summary>
    /// <remarks>
    /// Override if you wish to provide more information to the clients
    /// such as the name of the host player
    /// </remarks>
    /// <param name="request">Request coming from client</param>
    /// <param name="endpoint">Address of the client that sent the request</param>
    /// <returns>A message containing information about this server</returns>
    protected override DiscoveryResponse ProcessRequest(DiscoveryRequest request, IPEndPoint endpoint) 
    {
        //return new DiscoveryResponse();
        try
        {
            Debug.Log("Process Request called");
            // this is an example reply message,  return your own
            // to include whatever is relevant for your game
            return new DiscoveryResponse
            {
                serverId = ServerId,
                uri = transport.ServerUri(),

                GameVersion = Application.version,
                ServerName = ServerData.ServerName,
                GameModeIndex = ServerData.GameModeIndex,
                MaxPlayersCount = ServerData.MaxPlayersCount,
                CurrentPlayersCount = ServerData.CurrentPlayersCount,
                MaxPing = ServerData.MaxPing
            };
        }
        catch (NotImplementedException)
        {
            Debug.LogError($"Transport {transport} does not support network discovery");
            throw;
        }
    }

    #endregion

    #region Client

    /// <summary>
    /// Create a message that will be broadcasted on the network to discover servers
    /// </summary>
    /// <remarks>
    /// Override if you wish to include additional data in the discovery message
    /// such as desired game mode, language, difficulty, etc... </remarks>
    /// <returns>An instance of ServerRequest with data to be broadcasted</returns>
    protected override DiscoveryRequest GetRequest()
    {
        return new DiscoveryRequest();
    }

    /// <summary>
    /// Process the answer from a server
    /// </summary>
    /// <remarks>
    /// A client receives a reply from a server, this method processes the
    /// reply and raises an event
    /// </remarks>
    /// <param name="response">Response that came from the server</param>
    /// <param name="endpoint">Address of the server that replied</param>
    protected override void ProcessResponse(DiscoveryResponse response, IPEndPoint endpoint) 
    {
        if (response.GameVersion != Application.version)
            return;

        Debug.Log("Process Response called");
        // we received a message from the remote endpoint
        response.EndPoint = endpoint;

        // although we got a supposedly valid url, we may not be able to resolve
        // the provided host
        // However we know the real ip address of the server because we just
        // received a packet from it,  so use that as host.
        UriBuilder realUri = new UriBuilder(response.uri)
        {
            Host = response.EndPoint.Address.ToString()
        };
        response.uri = realUri.Uri;

        OnServerFound?.Invoke(response);
    }

    #endregion

    public new void AdvertiseServer()
    {
        StartCoroutine(AdvertiseServerCoroutine());
    }

    public IEnumerator AdvertiseServerCoroutine()
    {
        while (NetworkServer.isLoadingScene)
            yield return null;

        base.AdvertiseServer();
    }
}
