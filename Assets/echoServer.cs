// https://docs.microsoft.com/ko-kr/dotnet/framework/network-programming/asynchronous-server-socket-example
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class echoServer : MonoBehaviour
{
    // State object for reading client data asynchronously  
    public class StateObject
    {
        // Client  socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }
    string listenIp;
    public bool waiting = false;
    public bool Opened = false;
    Text text;

    List<Socket> clients = new List<Socket>();

    private void Start()
    {
        text = GetComponent<Text>();
    }

    public void setIP(string ip)
    {
        listenIp = ip;
        text.text = "Set " + ip; 
    }

    public void StartServer()
    {
        if(Opened)
        {
            //CloseServer
            foreach(var sock in clients)
            {
                sock.Shutdown(SocketShutdown.Both);
                sock.Close();
            }
            clients.Clear();
        }

        if (listenIp.Length > 0)
            StartCoroutine(StartListening());
    }

    // Incoming data from the client.  
    public static string data = null;

    IEnumerator StartListening()
    {
        Debug.LogFormat("StartListening...");

        // Establish the local endpoint for the socket.  
        // The DNS name of the computer  
        // running the listener is "host.contoso.com".  
        //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        //IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPAddress ipAddress = IPAddress.Parse(listenIp);
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        text.text = "Open " + listenIp;

        // Create a TCP/IP socket.  
        Socket listener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to the local endpoint and listen for incoming connections.  
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(100);

            Opened = true;
        }
        catch (Exception e)
        {
            Debug.LogFormat(e.ToString());
        }
        while (true)
        {
            // Set the event to nonsignaled state.  
            waiting = false;

            // Start an asynchronous socket to listen for connections.  
            Debug.LogFormat("Waiting for a connection...");
            listener.BeginAccept(
                new AsyncCallback(AcceptCallback),
                listener);

            // Wait until a connection is made before continuing.  
            yield return new WaitUntil(()=>{ return waiting == true; });
        }
    }

    public void AcceptCallback(IAsyncResult ar)
    {
        // Signal the main thread to continue.  
        //waiting = true;

        // Get the socket that handles the client request.  
        Socket listener = (Socket)ar.AsyncState;
        Socket socket = listener.EndAccept(ar);

        //Debug.LogFormat("connected..." + socket.RemoteEndPoint);
        //text.text = "connected ";

        // Create the state object.  
        StateObject state = new StateObject();
        state.workSocket = socket;
        socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
            new AsyncCallback(ReadCallback), state);
    }

    public void ReadCallback(IAsyncResult ar)
    {
        String content = String.Empty;

        // Retrieve the state object and the handler socket  
        // from the asynchronous state object.  
        StateObject state = (StateObject)ar.AsyncState;
        Socket socket = state.workSocket;

        // Read data from the client socket.   
        int bytesRead = socket.EndReceive(ar);
        if (bytesRead > 0)
        {
            // There  might be more data, so store the data received so far.  
            state.sb.Append(Encoding.ASCII.GetString(
                state.buffer, 0, bytesRead));

            // Check for end-of-file tag. If it is not there, read   
            // more data.  
            content = state.sb.ToString();
            Debug.LogFormat("Read {0} bytes from socket. \n Data : {1}",
                content.Length, content);
            if (content.IndexOf("<EOF>") > -1)
            {
                Debug.LogFormat("Sending {0}", content);

                // All the data has been read from the   
                // client. Display it on the console.  
                // Echo the data back to the client.  
                Send(socket, content);
            }
            else
            {
                // Not all data received. Get more.  
                socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
            }
        }
    }

    private void Send(Socket socket, String data)
    {
        // Convert the string data to byte data using ASCII encoding.  
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        // Begin sending the data to the remote device.  
        socket.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), socket);
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket socket = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.  
            int bytesSent = socket.EndSend(ar);
            Debug.LogFormat("Sent {0} bytes to client.", bytesSent);

            StateObject state = new StateObject();
            state.workSocket = socket;
            socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }
        catch (Exception e)
        {
            Debug.LogFormat(e.ToString());
        }
    }
}
