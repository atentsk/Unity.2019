// https://docs.microsoft.com/ko-kr/dotnet/framework/network-programming/synchronous-client-socket-example
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class SocketClient : MonoBehaviour
{
    string connectIP;
    public void setIP(string ip)
    {
        connectIP = ip;
    }

    public void Connect()
    {
        if(connectIP.Length>0)
            StartCoroutine( StartClient());
    }
    IEnumerator StartClient()
    {
        // Data buffer for incoming data.  
        byte[] bytes = new byte[1024];

        // Connect to a remote device.  
        try
        {
            Debug.LogFormat("StartClient...");

            // Establish the remote endpoint for the socket.  
            // This example uses port 11000 on the local computer.  
            //IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPAddress ipAddress = IPAddress.Parse(connectIP);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP  socket.  
            Socket sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.  
            try
            {
                sender.Connect(remoteEP);

                Debug.LogFormat("Socket connected to {0}",
                    sender.RemoteEndPoint.ToString());

                // Encode the data string into a byte array.  
                byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

                // Send the data through the socket.  
                int bytesSent = sender.Send(msg);

                // Receive the response from the remote device.  
                int bytesRec = sender.Receive(bytes, SocketFlags.Peek);
                Debug.LogFormat("Echoed test = {0}",
                    Encoding.ASCII.GetString(bytes, 0, bytesRec));

                // Release the socket.  
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch (ArgumentNullException ane)
            {
                Debug.LogException(ane);
            }
            catch (SocketException se)
            {
                Debug.LogException(se);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        yield break;
    }
}
