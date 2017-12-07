using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                            MAGIC_PLAYER_AOE, MATIC_TOP_AOE, MAGIC_RAGE, MAGIC_TELEPORT, MAGIC_SPECIAL }

    // Variables for elemental bullet magic
    [SerializeField]
    GameObject[] elementalBullet = new GameObject[3];

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

    // Variables for teleport magic
    [SerializeField]
    Transform playerTransform;

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

    public void GetMagicCirclePath(string path)
    {
        // TODO: Parse parameter 'path' to find appropriate magic function
        
        switch (path)
        {
            case "1234561":
            case "1654321":
                _DoMagic("Thunder", MagicType.MAGIC_ELEMENTAL);
                break;
            case "3456123":
            case "3216543":
                _DoMagic("Water", MagicType.MAGIC_ELEMENTAL);
                break;
            case "5612345":
            case "5432165":
                _DoMagic("Water", MagicType.MAGIC_ELEMENTAL);
                break;
            // Insert case here!
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
        Vector3 direction = Camera.main.transform.forward;   // TODO: Change direction to wand front vector
        switch (m)
        {
            // Insert case here!
            case MagicType.MAGIC_ELEMENTAL:
                switch (element)
                {
                    case "Thunder": Elemental(direction, Element.Thunder); break;
                    case "Water": Elemental(direction, Element.Water); break;
                    case "Flame": Elemental(direction, Element.Flame); break;
                    default: Debug.Log("No elemental bullet: " + element); break;
                }
                break;
            case MagicType.MAGIC_LASER:
                Laser(direction);
                break;
            case MagicType.MAGIC_TERRAIN_UP:
            case MagicType.MAGIC_TERRAIN_DOWN:
                _DoRaycastMagic(direction, element, m);
                break;
            case MagicType.MAGIC_TELEPORT:
                Teleport(direction);
                break;
            case MagicType.MAGIC_PLAYER_AOE:
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
        Instantiate(elementalBullet[(int)e / 2], Camera.main.transform.position, Quaternion.LookRotation(direction));
    }
    
    public void ElementalForTurret(Vector3 origin, Vector3 destination, Element e)
    {
        Instantiate(elementalBullet[(int)e / 2], origin, Quaternion.LookRotation(destination - origin));
    }

    void TerrainTransform(Vector3 destination, bool up)
    {
        Debug.Log(destination);
        StartCoroutine(TTBegin((int)destination.x, (int)destination.z, up));
    }

    void Laser(Vector3 direction)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    // TODO: Change ray
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(ray, 0.1f);

        Vector3 target = Vector3.zero;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.gameObject.layer == 11)
            {
                Destroy(hits[i].transform.gameObject);      // TODO: Deactivate instead of destroy
            }
        }
    }

    void Teleport(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, direction, out hit))    // TODO: No camera vector
        {
            int layer = hit.transform.gameObject.layer;
            if (layer == 10 || layer == 13)
            {
                Vector3 destination = Camera.main.transform.position;
                destination.x = hit.point.x;
                destination.z = hit.point.z;
                playerTransform.position = destination;
            }
        }
    }

    IEnumerator TTBegin(int x, int y, bool up)
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
