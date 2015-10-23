using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour
{
    IEnumerator Start()
    {
        Debug.Log("connected to server, trying to send password");
        if (!GameSettings.Instance.Local)
        {
            while (!CrystallizeNetwork.Connected)
            {
                yield return null;
            }
        }
        Debug.Log("connected to server, trying to send password");
        var client = new RPCClient(GameSettings.Instance.ServerIP);
        var pair = new UsernamePasswordPair("testing", "testing1");
        client.RequestNameFromServer(pair, null);
    }
}
