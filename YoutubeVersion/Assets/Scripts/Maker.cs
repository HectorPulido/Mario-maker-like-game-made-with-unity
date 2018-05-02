using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Maker : MonoBehaviour
{
    public MakerTile[] tiles;
    public GameObject buttonPrefab;
    public Transform layout;
    public SpriteRenderer preview;
    public GameObject[] playingObjects;

    int id;

    public static bool playing;

	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            int u = i;
            var t = Instantiate(buttonPrefab, layout);
            t.GetComponent<Image>().sprite = tiles[u].sprite;
            t.GetComponent<Button>().onClick.AddListener(()=> 
            {
                id = u;
                preview.sprite = tiles[u].sprite;
            });
        }	
	}

    public void TogglePlaying()
    {
        playing = !playing;
        preview.enabled = !playing;
        for (int i = 0; i < playingObjects.Length; i++)
        {
            playingObjects[i].SetActive(!playing);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;
        if (playing)
            return;

        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        pos.x = Mathf.RoundToInt(pos.x);
        pos.y = Mathf.RoundToInt(pos.y);

        preview.transform.position = pos;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var c = Physics2D.CircleCast(pos, 0.4f, Vector2.zero);
            if (c.collider == null)
            {
                Instantiate(tiles[id].gameObject, pos, Quaternion.identity);
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            var c = Physics2D.CircleCast(pos, 0.4f, Vector2.zero);
            if (c.collider != null)
            {
                Destroy(c.collider.gameObject);
            }
        }

    }
}
