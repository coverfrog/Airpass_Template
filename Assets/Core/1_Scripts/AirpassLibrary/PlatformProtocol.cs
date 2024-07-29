using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public enum Cmd
{
    Load,
    End,
    Home,
    Restart,
    Start,
    Ready,
    Pause,
    ConGame,
    Close
}

[DefaultExecutionOrder(-1000)]
public class PlatformProtocol : MonoBehaviour
{
    private const string IpAddress = "127.0.0.1";
    private const int Port = 13809;
    
    private static UdpClient _udpClient;

    public static void SendData(Cmd cmd)
    {
        string sendingString = cmd.ToString().ToLower();
        byte[] sendingData = Encoding.UTF8.GetBytes(sendingString);
        
        _udpClient?.Send(sendingData, sendingData.Length);
    }
    
    private void Awake()
    {
        _udpClient = new UdpClient();
        _udpClient.Connect(IPAddress.Parse(IpAddress), Port);
    }
    
    private void OnApplicationQuit()
    {
        _udpClient?.Close();
        _udpClient?.Dispose();
    }
}
