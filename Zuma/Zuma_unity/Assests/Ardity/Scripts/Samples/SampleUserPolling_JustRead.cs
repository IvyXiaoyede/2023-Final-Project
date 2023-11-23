using UnityEngine;
using System.Collections;

public class SampleUserPolling_JustRead : MonoBehaviour
{
    public SerialController serialController;
    public GameObject Cube;
    public Shooter shooter;

    // Initialization
    void Start()
    {
     serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
     shooter = GameObject.Find("Shooter").GetComponent<Shooter>();
      //  shooter = Shooter.Instance;
    }

    // Executed each frame
    void Update()
    {
        string message = serialController.ReadSerialMessage();

        if (message == null)
            return;

        // Check if the message is plain data or a connect/disconnect event.
        if (message.Equals("On"))
        {
            shooter.Shoot();
            Debug.Log("Received On signal. Performing action...");
            // Add your code here to specify what action you want to take when "On" is received.
            // For example, you can change the color of the Cube:
            //Cube.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected");
        else
        {
            // Convert the received message to a float
            float zRotationValue;

            if (float.TryParse(message, out zRotationValue))
            {
                // Set the z-rotation of the Cube
                Cube.transform.rotation = Quaternion.Euler(0, 0, -zRotationValue);

                // Log the message and the corresponding rotation value
             //   Debug.Log("Message arrived: " + message + " | Z Rotation: " + zRotationValue);
            }
            else
            {
                Debug.LogWarning("Failed to parse message as float: " + message);
            }
        }
    }
}
