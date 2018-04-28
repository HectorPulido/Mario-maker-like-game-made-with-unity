using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public struct Tile
{
    public float x, y, z;
    public int id;

    public Tile(float x, float y, float z, int id)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.id = id;
    }
}


public class SaveEvoriment : MonoBehaviour
{
    public GameObject[] availableTiles;
    public string path = @"C:\Niveles\MyFile.bin";

    MakerTile[] tiles;
    Tile[] serializeTiles;
    
    public void OnSave()
    {
        tiles = GameObject.FindObjectsOfType<MakerTile>();
        serializeTiles = new Tile[tiles.Length];
        for (int i = 0; i < tiles.Length; i++)
        {
            serializeTiles[i] = new Tile(
                tiles[i].transform.position.x,
                tiles[i].transform.position.y,
                tiles[i].transform.position.z,
                tiles[i].id);
        }
        print(tiles.Length);       

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(path,
                                     FileMode.Create,
                                     FileAccess.Write, 
                                     FileShare.None);
        formatter.Serialize(stream, serializeTiles);
        stream.Close();
    }

    public void OnRead()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(path,
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.Read);
        var obj = (Tile[])formatter.Deserialize(stream);
        stream.Close();

        Clean();
        for (int i = 0; i < obj.Length; i++)
        {
            Instantiate(
                availableTiles[obj[i].id],
                new Vector3(
                obj[i].x,
                obj[i].y,
                obj[i].z), 
                Quaternion.identity);           

        }
    }

    void Clean()
    {
        tiles = GameObject.FindObjectsOfType<MakerTile>();
        foreach (var i in tiles)
        {
            Destroy(i.gameObject);
        }
    }
}
