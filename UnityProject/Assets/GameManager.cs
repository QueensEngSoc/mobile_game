using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

[Serializable]
public class vehicle_desc
{
    List<Node> nodes;
    List<NodeConnection> connections;
}

public class GameManager : MonoBehaviour
{
    public GameObject vehiclePrefab;
    public static GameManager gm;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this; // this instance of the GameManager is now the one true GameManager
            DontDestroyOnLoad(gameObject); // make sure it persists through scene changes
        }
        else
            Destroy(gameObject); // this is NOT the true GameManager, kill it now
    }

    // Start is called before the first frame update
    void Start()
    {
        //Vehicle newVehicle = Instantiate(vehiclePrefab, Vector3.zero, Quaternion.identity).GetComponent<Vehicle>();
        //newVehicle.LoadVehicle("resource");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveVehicle(vehicle_desc vehicle, string fileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + fileName, FileMode.Create);
        bf.Serialize(stream, vehicle);
        stream.Close();
    }

    public vehicle_desc LoadVehicle(string fileName)
    {
        string filePath = Application.persistentDataPath + "/" + fileName;

        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);
            vehicle_desc desc = (vehicle_desc)bf.Deserialize(stream);
            return desc;
        }

        Debug.Log("Hey dude, it didn't load.");

        return null;
    }
}
