using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

public class SocketOpen : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        text.text = "IP List\n";
        var items = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface item in items)
        {
            foreach( var ip in item.GetIPProperties().UnicastAddresses )
            {
                if( ip.Address.AddressFamily == AddressFamily.InterNetwork )
                    text.text += ip.Address.ToString()+"\n";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
