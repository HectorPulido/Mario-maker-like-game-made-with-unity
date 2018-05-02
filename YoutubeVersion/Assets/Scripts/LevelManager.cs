using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

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

public class LevelManager : MonoBehaviour {
    public MakerTile[] makerTilePrefabs;
    public string path = @"C:\Niveles\MyLevel.bin";

    MakerTile[] makerTiles;

    public void Save()
    {
        makerTiles = GameObject.FindObjectsOfType<MakerTile>();
        Tile[] t = new Tile[makerTiles.Length];
        for (int i = 0; i < t.Length; i++)
        {
            t[i] = new Tile(makerTiles[i].transform.position.x,
                            makerTiles[i].transform.position.y,
                            makerTiles[i].transform.position.z,
                            makerTiles[i].id);
        }

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(path,
                                     FileMode.Create,
                                     FileAccess.Write,
                                     FileShare.None);
        formatter.Serialize(stream, t);
        stream.Close();
    }
    public void Load()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(path,
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.Read);
        var obj = (Tile[])formatter.Deserialize(stream);
        stream.Close();

        for (int i = 0; i < obj.Length; i++)
        {
            Instantiate(makerTilePrefabs[obj[i].id],
                new Vector3(obj[i].x, obj[i].y, obj[i].z),
                Quaternion.identity);
        }
    }

    void Clear()
    {
        makerTiles = GameObject.FindObjectsOfType<MakerTile>();
        foreach (var i in makerTiles)
        {
            Destroy(i.gameObject);
        }
    }

}
