using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;
using System.Text;

public class ImageProcessor : MonoBehaviour {

    public TextAsset frozenGraph;

    TFGraph graph;
    TFSession session;
    
	// Use this for initialization
	void Start () {
        graph = new TFGraph();
        
        if(frozenGraph != null)
        {
            graph.Import(frozenGraph.bytes);
        }

        session = new TFSession(graph);
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator StartPredict(byte[] bytes)
    {
        TFSession.Runner runner = session.GetRunner();

        
        runner.AddInput(graph["binary"][0], TFTensor.CreateString(bytes) );
        runner.Fetch(graph["InceptionV3/Predictions/Reshape_1"][0]);
        
        float[,] result = runner.Run()[0].GetValue() as float[,];
        float[] output = new float[result.GetLength(1)];

        for (int i = 0; i < result.GetLength(1); i++) {

            output[i] = result[0, i];
            Debug.Log(i + " :" + result[0, i]);
        }

        MagicCircleInputManager.I.predictions = output;

        yield return null;

    }


}
