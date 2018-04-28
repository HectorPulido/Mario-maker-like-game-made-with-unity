using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Maker : MonoBehaviour
{
    public MakerTile[] tiles;
    public GameObject buttonPrefab;
    public Transform layout;

    public SpriteRenderer testSprite;
    int id;

    public static bool Play;
    public GameObject[] MakerUi;

    public void TogglePlay()
    {
        Play = !Play;

        testSprite.transform.position = Vector3.up * 9999;
        for (int i = 0; i < MakerUi.Length; i++)
        {
            MakerUi[i].SetActive(!Play);
        }        

    }

    void Start()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            int u = i;

            var button = Instantiate(buttonPrefab, layout);
            button.GetComponent<Image>().sprite = tiles[i].tileSprite;
            button.GetComponent<Button>().onClick.AddListener(() => 
            {
                testSprite.sprite = tiles[u].tileSprite;
                id = u;
            });
        }

	}

	void Update ()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;
        if (Play)
            return;
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        pos.x = Mathf.RoundToInt(pos.x);
        pos.y = Mathf.RoundToInt(pos.y);

        if (Input.GetMouseButtonDown(0))
        {                       
            var i = Physics2D.CircleCast(pos, 0.4f, Vector2.zero);
            if (i.collider == null)
            {
                Instantiate(tiles[id], pos, Quaternion.identity);
            }
        }

        testSprite.transform.position = pos;
	}
}
