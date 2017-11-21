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

    // Magic informations


    // Variables for elemental bullet magic
    enum Element { Water, Fire, Electricity }
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

    // Use this for initialization
    void Start () {
        myTerrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        basicTransformArray = new float[transformSize, transformSize];
        for (int i = 0; i < transformSize; i++)
        {
            for ( int j = 0; j < transformSize; j++)
            {
                basicTransformArray[i, j] = transformHeight * (1 - Mathf.Cos(2 * Mathf.PI * i / transformSize)) * (1 - Mathf.Cos(2 * Mathf.PI * j / transformSize)) / 4;
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
	void Update () {
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
    }

    void Elemental(Vector3 destination, Element e)
    {
        Transform initialTransform = gameObject.transform;
        initialTransform.forward = destination - gameObject.transform.position;

        Instantiate(elementalBullet[(int)e], gameObject.transform); // TODO: Replace Instantiate() to setActive() with memory pool
    }

    void TerrainTransform(Vector3 destination, bool up)  // TODO: Exception for edge selection case
    {
        StartCoroutine(TTBegin((int)destination.x, (int)destination.z, up));
    }

    IEnumerator TTBegin(int x, int y, bool up)    // Getting higer/lower from default terrain
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

        Debug.Log("Transform begin " + defaultHeight[25, 25]);
        yield return StartCoroutine(TTCore(x, y, defaultHeight, targetHeight));
        Debug.Log("Transform end");
        yield return new WaitForSeconds(timeToDefault);

        float[,] resultHeight = myTerrain.terrainData.GetHeights(x - (transformSize / 2), y - (transformSize / 2), transformSize, transformSize);
        Debug.Log("Revert begin " + resultHeight[25,25]);
        yield return StartCoroutine(TTCore(x, y, resultHeight, defaultHeight));
        Debug.Log("Revert end");
    }

    IEnumerator TTCore(int x, int y, float[,] from, float[,] to)
    {
        float[,] tmp = new float[transformSize, transformSize];
        float[,] difference = new float[transformSize, transformSize];

        for (int i = 0; i < transformSize; i++)
        {
            for (int j = 0; j < transformSize; j++)
            {
                tmp[i, j] = from[i, j];
                difference[i, j] = (to[i, j] - from[i, j]) / frameToTransform;
            }
        }

        for (float frameCount = 0; frameCount <= frameToTransform; frameCount++)
        {
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
