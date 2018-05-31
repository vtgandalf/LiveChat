using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour {
    public GameObject chatContainer;
    public GameObject messagePrefab;

    //public string clientName;

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    //public void Start()
    //{
    //    ConnectToServer();
    //}
    public void ConnectToServer()
    {
        //if connected, ignore this function\
        if (socketReady)
        {
            return;
        }
        //default host/port values
        string host = "127.0.0.1";
        //string host = "192.168.1.105";
        int port = 6321;
        //overwrite host/port values if there is something in those boxes
        string h;
        int p;
        h = GameObject.Find("HostInput").GetComponent<InputField>().text;
        if(h != "")
        {
            host = h;
        }
        int.TryParse(GameObject.Find("PortInput").GetComponent<InputField>().text, out p);
        if (p != 0)
        {
            port = p;
        }
        //create socket
        try
        {
            socket = new TcpClient(host,port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true; 
        }
        catch(Exception e)
        {
            Debug.Log("Socket error: " + e.Message);
        }
    }

    private void Update()
    {
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                string data = reader.ReadLine();
                if (data != null)
                {
                    OnIncomingData(data);
                }
            }
        }
    }

    private void OnIncomingData(string data)
    {
        //Debug.Log("Server : " + data);
        GameObject container = GameObject.Find("Content");
        RectTransform rt = container.transform.GetComponent<RectTransform>();//.sizeDelta = new Vector2(width, height);
        container.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + 70);
        Canvas.ForceUpdateCanvases();
        ScrollRect[] sr= GameObject.Find("ChatWindow").GetComponents<ScrollRect>();
        sr[0].verticalScrollbar.value = 0f;
        //GameObject.Find("ChatWindow").GetComponents<ScrollRect>().verticalScrollbar.value = 0f;
        Canvas.ForceUpdateCanvases();
        GameObject go = Instantiate(messagePrefab, chatContainer.transform) as GameObject;
        go.transform.SetParent(GameObject.Find("Content").transform);
        go.GetComponentInChildren<Text>().text = data;
    }

    private void Send(string data)
    {
        if(!socketReady)
        {
            return;
        }
        writer.WriteLine(data);
        writer.Flush();
    }
    public void OnSendButton()
    {
        string message = GameObject.Find("ChatBox").GetComponent<InputField>().text;
        if(message != "")
        {
            GameObject.Find("ChatBox").GetComponent<InputField>().text = "";
            Send(message);
        }
    }
}
