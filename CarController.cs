using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Net;
using System.Text;

public class CarController : MonoBehaviour
{
    public List<Vector2> rays;
    public float movementSpeed = 2;
    public float maxRayDistance = 50f;
    //rotation rate per second in angles
    public float rotationRate = 1;
    //min and max angles in degrees 
    public Vector2 rotationRange = new Vector2(-45, 45);
    private Rigidbody2D rb;
    private List<Vector2> hitPoints;
    private const int HEADER_SIZE = 10;
    private Socket sender;
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

       // IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
       // IPAddress ipAddr = ipHost.AddressList[0];
       // IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 2183);

      //  Debug.LogWarning("ipHost " + ipHost + " ipAddr" + ipAddr + " localEndPoint" + localEndPoint);
        sender = new Socket(AddressFamily.InterNetwork,
                   SocketType.Stream, ProtocolType.Tcp);
        sender.Connect(Dns.GetHostName(), 2183);

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && Mathf.Sin(Mathf.Deg2Rad * (transform.eulerAngles.z + (rotationRate * Time.fixedDeltaTime))) <= Mathf.Sin(Mathf.Deg2Rad * rotationRange.y))
            transform.eulerAngles += new Vector3(0, 0, rotationRate * Time.fixedDeltaTime);
        else if (Input.GetKey(KeyCode.RightArrow) && Mathf.Sin(Mathf.Deg2Rad * (transform.eulerAngles.z - (rotationRate * Time.fixedDeltaTime))) >= Mathf.Sin(Mathf.Deg2Rad * rotationRange.x))
            transform.eulerAngles -= new Vector3(0, 0, rotationRate * Time.fixedDeltaTime);

        rb.velocity = new Vector2(-movementSpeed * Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.z), movementSpeed * Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.z));
        hitPoints = new List<Vector2>();
        //raycasting code here
        string data = $"{"\"angle\""}:{transform.eulerAngles.z}, {"\"points\""}:[";
        foreach (Vector2 ray in rays)
        {
            Vector2 newRay = Quaternion.Euler(0, 0, transform.eulerAngles.z) * ray;
            Vector2 hitPoint = Physics2D.Raycast(transform.position, newRay, maxRayDistance).point;
            hitPoints.Add(hitPoint);
            data += $"[{(hitPoint.x - transform.position.x) / maxRayDistance},{(hitPoint.y - transform.position.y) / maxRayDistance}],";
            Debug.DrawLine(transform.position, hitPoint, Color.green);
        }
        data = data.Remove(data.Length - 1);
        data += "]";
        data = "{" + data + "}";
        Debug.Log(data);
        sendData(data);
    }

    int sendData(string data)
    {
        string header = data.Length.ToString();
        for (int i = 0; i < HEADER_SIZE - data.Length.ToString().Length;i++)
            header += " ";
        string message = header + data;
        try
        {
            try
            {
              

                

              //  Debug.Log("Socket connected to -> {0} ",
                //              sender.RemoteEndPoint.ToString());

                byte[] messageSent = Encoding.ASCII.GetBytes(message);
                int byteSent = sender.Send(messageSent);

                byte[] messageReceived = new byte[4];


                int byteRecv = sender.Receive(messageReceived);
               // Debug.Log("Message from Server -> {0}",
                 //     Encoding.ASCII.GetString(messageReceived,
                                //                 0, byteRecv));
          

            }

            // Manage of Socket's Exceptions
            catch (ArgumentNullException ane)
            {

               // Debug.LogError("ArgumentNullException : {0}", ane.ToString());
            }

            catch (SocketException se)
            {

               // Debug.LogError("SocketException : {0}", se.ToString());
            }

            catch (Exception e)
            {
               // Debug.LogError("Unexpected exception : {0}", e.ToString());
            }
        }

        catch (Exception e)
        {

            Debug.LogError(e.ToString());
        }
        return 0;
    }

    void OnDestroy()
    {
              sender.Shutdown(SocketShutdown.Both);
        sender.Close();
    }
}
