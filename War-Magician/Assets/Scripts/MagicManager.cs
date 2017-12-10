using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MagicManager : MonoBehaviour {
    // Static variables for singleton
    private static MagicManager _manager = null;
    public static MagicManager I
    {
        get { return _manager; }
    }

    // Magic types
    public enum Element { Thunder, Air, Flame, Soil, Water, Ice }
    public enum MagicType { MAGIC_ELEMENTAL, MAGIC_TERRAIN_DOWN, MAGIC_TERRAIN_UP, MAGIC_TURRET, MAGIC_LASER,
                            MAGIC_PLAYER_AOE, MAGIC_TOP_AOE, MAGIC_RAGE, MAGIC_TELEPORT, MAGIC_SPECIAL }

    // Variables for elemental bullet magic
    [SerializeField]
    Transform Wand;

    // Variables for elemental bullet magic
    [SerializeField]
    GameObject[] elementalBullet = new GameObject[3];

    // Variables for elemental bullet magic
    [SerializeField]
    GameObject LaserEffect;

    // Variables for turret magic
    [SerializeField]
    GameObject[] Turret = new GameObject[5];
    [SerializeField]
    GameObject[] TurretBullet = new GameObject[5];

    // Variables for terrain transform magic
    Terrain myTerrain;
    float[,] basicTransformArray;   // Reference array for transform. Initialized in Start()

    [SerializeField]
    float transformHeight;    // How tall? [0, 1]
    [SerializeField]
    int transformSize;      // How large? (x, y)
    [SerializeField]
    int frameToTransform;  // Frame to modify terrain
    [SerializeField]
    float timeToDefault;    // Time to change modified terrain into default
    [SerializeField]
    GameObject Obstacle;    // GameObject for Navmesh Obstacle



    // Variables for teleport magic
    [SerializeField]
    Transform playerTransform;
    [SerializeField]
    float minDistance;
    [SerializeField]
    float teleportDistance;    // Time to change modified terrain into default

    // Variables for AOE magic
    [SerializeField]
    GameObject[] AOEBullet = new GameObject[6];
    float fallingHeight;

    // Variables for AOE Top magic
    [SerializeField]
    public AOETop AOETopObject;

    // Use this for initialization
    void Start () {
        myTerrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        basicTransformArray = new float[transformSize, transformSize];
        for (int i = 0; i < transformSize; i++)
        {
            for ( int j = 0; j < transformSize; j++)
            {
                basicTransformArray[i, j] = transformHeight * (1 - Mathf.Cos(2 * Mathf.PI * i / transformSize)) * (1 - Mathf.Cos(2 * Mathf.PI * j / transformSize)) / 4;    // basicTransformArray initialization
            }
        }

        // Singleton
		if ( I == null )
        {
            _manager = this;
        }
        else if ( I != this )
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _DoMagic("Thunder", MagicType.MAGIC_TELEPORT);
        }
        /*
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
                TerrainTransform(hit.point, true);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
                TerrainTransform(hit.point, false);
        }
        */
    }

    public void GetMagicCirclePath(string element, string path)
    {
        // TODO: Parse parameter 'path' to find appropriate magic function

        Debug.Log(path);
    
        switch (path)
        {
            case "1234561":
            case "1654321":
                _DoMagic(element, MagicType.MAGIC_ELEMENTAL);
                break;
            case "3456123":
            case "3216543":
                _DoMagic(element, MagicType.MAGIC_ELEMENTAL);
                break;
            case "5612345":
            case "5432165":
                _DoMagic(element, MagicType.MAGIC_ELEMENTAL);
                break;
            // Insert case here!
            case "4":
                _DoMagic(element, MagicType.MAGIC_TERRAIN_DOWN);
                break;
            case "41":
                _DoMagic(element, MagicType.MAGIC_TERRAIN_UP);
                break;
            case "126354":
                _DoMagic(element, MagicType.MAGIC_LASER);
                break;
            case "1261351":
            case "1261531":
            case "1621351":
            case "1621531":
            case "2132462":
            case "2132642":
            case "2312462":
            case "2312642":
            case "3243513":
            case "3243153":
            case "3423513":
            case "3423153":
            case "5465315":
            case "5465135":
            case "5645135":
            case "5645315":
            case "6156246":
            case "6156426":
            case "6516246":
            case "6516426":
                _DoMagic(element, MagicType.MAGIC_TURRET);
                break;
            case "23652":
            case "25632":
                _DoMagic(element, MagicType.MAGIC_TOP_AOE);
                break;
            default:
                Debug.Log("No magic matched with path");
                break;
        }
    }

    public void GetMagicCircleImageType(string element, MagicType m)
    {
        _DoMagic(element, m);
    }

    private void _DoMagic(string element, MagicType m)
    {
        Element e;
        Enum.TryParse(element, out e);
        //Vector3 direction = Camera.main.transform.forward;   // TODO: Change direction to wand front vector
        Vector3 direction = Wand.forward;

        switch (m)
        {
            // Insert case here!
            case MagicType.MAGIC_ELEMENTAL:
                if ((int)e % 2 == 0)
                    Elemental(direction, e);
                else
                    Debug.Log("No elemental bullet: " + element);
                break;
            case MagicType.MAGIC_LASER:
                Laser(direction);
                break;
            case MagicType.MAGIC_TERRAIN_UP:
            case MagicType.MAGIC_TERRAIN_DOWN:
                _DoRaycastMagic(direction, element, m);
                break;
            case MagicType.MAGIC_PLAYER_AOE:
                AOE(direction, e);
                break;
            case MagicType.MAGIC_TOP_AOE:
                AOETop(direction);
                break;
            case MagicType.MAGIC_TURRET:
                CallTurret(element);
                break;
            default:
                Debug.Log("No magic matched");
                break;
        }
        
    }

    private void _DoRaycastMagic(Vector3 direction, string element, MagicType m)
    {

        RaycastHit[] hits;
        hits = Physics.RaycastAll(Camera.main.transform.position, direction);
        Debug.Log(hits);

        Vector3 target = Vector3.zero;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.gameObject.layer != 10)
            {
                target = hits[i].point;
                Debug.Log(hits[i].transform.gameObject.name);
                break;
            }
        }

        if (target != Vector3.zero)
        {
            switch (m)
            {
                case MagicType.MAGIC_TERRAIN_UP:
                    TerrainTransform(target, true);
                    break;
                case MagicType.MAGIC_TERRAIN_DOWN:
                    TerrainTransform(target, false);
                    break;
                // Insert case here!
                default:
                    Debug.Log("No magic matched");
                    break;
            }
        }
        else
            Debug.Log("Raycast failed");
    }

    void Elemental(Vector3 direction, Element e)
    {
        //Instantiate(elementalBullet[(int)e / 2], Wand.position, Quaternion.LookRotation(direction));
        GameObject g = (GameObject) Instantiate(elementalBullet[(int)e / 2], Wand.position, Quaternion.identity);
        g.transform.LookAt(Wand.forward + Wand.position);
    }
    
    public void ElementalForTurret(Vector3 origin, Vector3 destination, Element e)
    {
        Instantiate(elementalBullet[(int)e / 2], origin, Quaternion.LookRotation(destination - origin));
    }

    void TerrainTransform(Vector3 destination, bool up)
    {
        Debug.Log(destination);
        
        GameObject g = (GameObject)Instantiate(Obstacle, destination + Vector3.up * transformHeight, Quaternion.identity);
        g.transform.SetParent(this.transform);
        g.GetComponent<NavMeshObstacle>().radius = transformSize / 2;
        g.GetComponent<NavMeshObstacle>().height = transformHeight * 2;

        StartCoroutine(TTBegin((int)destination.x, (int)destination.z, up, g));
    }

    void Laser(Vector3 direction)
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    // TODO: Change ray
        Ray ray = new Ray(Wand.position, Wand.forward);
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(ray, 0.1f);

        GameObject g = (GameObject) Instantiate(LaserEffect, Wand.position, Quaternion.identity);
        g.transform.LookAt(Wand.forward + Wand.position);

        Vector3 target = Vector3.zero;
        for (int i = 0; i < hits.Length; i++)
        {
            //if (hits[i].transform.gameObject.layer == 11)
            if (hits[i].transform.tag == "FieldMonster" || hits[i].transform.tag == "AirMonster")
            {
                //Destroy(hits[i].transform.gameObject);      // TODO: Deactivate instead of destroy]
                hits[i].transform.gameObject.GetComponent<Monster_HP>().GetDamaged(20);
            }
        }
    }

    void CallTurret(string element)
    {
        GameObject target = null;
        GameObject targetbullet = null;

        switch (element)
        {
            case "Thunder":
                target = Turret[0];
                targetbullet = TurretBullet[0];
                break;
            case "Air":
                target = Turret[1];
                targetbullet = TurretBullet[1];
                break;
            case "Flame":
                target = Turret[2];
                targetbullet = TurretBullet[2];
                break;
            case "Soil":
                break;
            case "Water":
                target = Turret[3];
                targetbullet = TurretBullet[3];
                break;
            case "Ice":
                target = Turret[4];
                targetbullet = TurretBullet[4];
                break;
        }

        if (target == null)
            return;

        GameObject g = (GameObject) Instantiate(target, playerTransform.position, Quaternion.identity);
        
    }

    public void Teleport()
    {
        Vector3 direction = Camera.main.transform.forward;
        //Vector3 direction = Wand.forward;

        RaycastHit hit;
        if (Physics.Raycast(playerTransform.position, direction, out hit))    // TODO: No camera vector
        {
            Debug.Log(hit.point);

            int layer = hit.transform.gameObject.layer;
            if (layer == 10 || layer == 12)
            {
                Vector3 destination = playerTransform.position;

                Vector3 dir = hit.point - destination;
                dir.y = 0;

                destination.x = hit.point.x;
                destination.z = hit.point.z;

               
                float length = (destination - playerTransform.position).magnitude;
                if (length < minDistance)
                {
                    return;
                }
                else if (length > teleportDistance)
                {
                    destination = (dir).normalized * teleportDistance + playerTransform.position;
                    playerTransform.position = destination;
                    //playerTransform.GetComponent<CharacterController>().SimpleMove(destination);
                }
                  
                     
            }
        }
    }

    public void GetTopAOEMagic(Vector3 direction)
    {
        int e = UnityEngine.Random.Range(0, 2) * 2;

        RaycastHit hit;
        if (Physics.Raycast(AOETopObject.transform.position, direction, out hit))
        {
            Instantiate(AOEBullet[e], hit.point, Quaternion.identity);
        }
    }


    void AOETop(Vector3 direction)
    {

        AOETopObject.enabled = true;
        VRInputManager.I.JoystickOn = true;
        VRInputManager.I.cameramanager.MainToTop();
     
    }

    public void AOETopEnd() //Callback when AOE Attack is over
    {
        AOETopObject.enabled = false;
        VRInputManager.I.JoystickOn = false;
        VRInputManager.I.cameramanager.TopToMain();
    }

    void AOE(Vector3 direction, Element e)
    {
        if ((int)e % 2 == 0)            // Ground attack only
        {
            Vector3 from = direction;
            from.y = fallingHeight;
            Instantiate(AOEBullet[(int)e / 2], from, Quaternion.LookRotation(Vector3.down));
        }
        else if (e == Element.Soil)     // No AOE
            Debug.Log("No soil aoe");
        else                            // Ground & air
            Instantiate(AOEBullet[(int)e], Camera.main.transform.position, Quaternion.LookRotation(direction));
    }

    IEnumerator TTBegin(int x, int y, bool up, GameObject obstcl)
    {
        float[,] defaultHeight = myTerrain.terrainData.GetHeights(x - (transformSize / 2), y - (transformSize / 2), transformSize, transformSize);
        float[,] targetHeight = new float[transformSize, transformSize];
        Debug.Log(basicTransformArray[25, 25]);

        for (int i = 0; i < transformSize; i++)
        {
            for (int j = 0; j < transformSize; j++)
            {
                if (up)
                    targetHeight[i, j] = defaultHeight[i, j] + basicTransformArray[i, j];
                else
                    targetHeight[i, j] = defaultHeight[i, j] - basicTransformArray[i, j];
            }
        }
        
        yield return StartCoroutine(TTCore(x, y, defaultHeight, targetHeight));
        yield return new WaitForSeconds(timeToDefault);

        for (int i = 0; i < transformSize; i++)
        {
            for (int j = 0; j < transformSize; j++)
            {
                targetHeight[i, j] = Mathf.Clamp(targetHeight[i, j], 0.0f, 1.0f);
            }
        }
        // float[,] resultHeight = myTerrain.terrainData.GetHeights(x - (transformSize / 2), y - (transformSize / 2), transformSize, transformSize);
        yield return StartCoroutine(TTCore(x, y, targetHeight, defaultHeight));

        Destroy(gameObject);
        yield return null;

    }

    IEnumerator TTCore(int x, int y, float[,] from, float[,] to)
    {
        float[,] tmp = new float[transformSize, transformSize];
        float[,] difference = new float[transformSize, transformSize];

        for (int i = 0; i < transformSize; i++)
        {
            for (int j = 0; j < transformSize; j++)
            {
                difference[i, j] = (to[i, j] - from[i, j]) / frameToTransform;
            }
        }

        for (float frameCount = 0; frameCount <= frameToTransform; frameCount++)
        {
            tmp = myTerrain.terrainData.GetHeights(x - (transformSize / 2), y - (transformSize / 2), transformSize, transformSize);
            for (int i = 0; i < transformSize; i++)
            {
                for (int j = 0; j < transformSize; j++)
                {
                    tmp[i, j] += difference[i, j];
                }
            }
            myTerrain.terrainData.SetHeights(x - (transformSize / 2), y - (transformSize / 2), tmp);
            yield return null;
        }
        
    }

}
