using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public GameObject Player;
    public Player_MP Player_MP;
    private static PlayerManager _manager = null;
    public static PlayerManager I
    {
        get { return _manager; }
    }

    // Use this for initialization
    void Start () {
        // Singleton
        if (I == null)
        {
            _manager = this;
        }
        else if (I != this)
        {
            Destroy(gameObject);
        }

        Player_MP = Player.GetComponent<Player_MP>();
    }
	
	// Update is called once per frame
	void Update () {
        // example. when left ctrl down, burn mana.
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (Player_MP.BurnMana(10f))
            {
                Debug.Log("Mana used");
            }
            else
            {
                Debug.Log("Not Enough Mana");
            }
        }
	}
}
