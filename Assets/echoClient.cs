// https://docs.microsoft.com/ko-kr/dotnet/framework/network-programming/synchronous-client-socket-example
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class echoClient : MonoBehaviour
{
    public class StateObject
    {
        // Client socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 256;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }
    Text text;
    Socket socket;
    string connectIP;
    List<string> msgs = new List<string>();

    private void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        if(msgs.Count > 0)
        {
            foreach(var str in msgs)
            {
                text.text += str + "\n";
            }
            msgs.Clear();
        }
    }

    public void setIP(string ip)
    {
        connectIP = ip;
    }
    public void Connect()
    {
        if (connectIP.Length > 0)
        {
            Debug.LogFormat("StartClient...");

            // Establish the remote endpoint for the socket.  
            // This example uses port 11000 on the local computer.  
            //IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPAddress ipAddress = IPAddress.Parse(connectIP);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP  socket.  
            Socket socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.  
            try
            {
                socket.BeginConnect(remoteEP, 
                    new AsyncCallback(ConnectCallback),
                    socket);
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
    }
    void ConnectCallback(IAsyncResult ar)
    {
        // Retrieve the socket from the state object.  
        socket = (Socket)ar.AsyncState;

        msgs.Add("Connected.");

        // Complete the connection.  
        socket.EndConnect(ar);

        Receive(socket);
    }

    private void Receive(Socket socket_)
    {
        try
        {
            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = socket_;

            Debug.LogFormat("client begin read");

            // Begin receiving the data from the remote device.  
            socket_.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the state object and the client socket   
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket socket_ = state.workSocket;

            // Read data from the remote device.  
            int bytesRead = socket_.EndReceive(ar);

            Debug.LogFormat("client Read {0}",bytesRead);
            if (bytesRead > 0)
            {
                // There might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                // All the data has arrived; put it in response.  
                if (state.sb.Length > 1)
                {
                    msgs.Add(state.sb.ToString());
                    state.sb.Clear();
                }
                // Get the rest of the data.  
                socket_.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            else
            {
                Debug.LogFormat("client Read zero {0}", bytesRead);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void Send(string msg)
    {
        if (msg.Length > 0)
        {
            Send(socket, msg);
        }
    }

    private void Send(Socket client, String data)
    {
        byte[] byteData = Encoding.ASCII.GetBytes(data+"<EOF>");

        Debug.Log(byteData);

        // Begin sending the data to the remote device.  
        client.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), client);
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket client = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.  
            int bytesSent = client.EndSend(ar);
            Debug.LogFormat("Sent {0} bytes to server.", bytesSent);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
