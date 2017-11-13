using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;
using System.IO;

public class MnistAgent : Agent {

	public float x_value = 0.0f;
	public float W_value = 0.0f;
	public float b_value = 0.0f;

	public float[] result;

	public Texture tex;


	private string weightDataPath = "weight" ;
	private string biasDataPath = "bias";
	private float[,] weightData;
	private float[,] biasData;

	public TextAsset graphModel;


	public TFGraph graph;
	public TFSession session;

	TFOutput x;
	TFOutput W;
	TFOutput b;
	TFOutput y;

	bool isfirst = true;

	public override List<float> CollectState()
	{

		List<float> state = new List<float>();

		for (int i = 0; i < result.Length; i++) {
			state.Add (result [i]);
		}
	
		return state;
	}

	public override void AgentStep(float[] act)
	{
		if (brain.brainParameters.actionSpaceType == StateType.discrete) {
			if ((int)act [0] > 0) {
				
				List<int> keyList = new List<int> (brain.agents.Keys);

				//tex = brain.ObservationToTex (observations [0], 28, 28);
				List<float[,,,]> texmatrix = brain.GetObservationMatrixList (keyList);
				float [,] data = new float[1, texmatrix [0].GetLength (1)*texmatrix [0].GetLength (2)];

				for (int i = 0; i < texmatrix [0].GetLength (1); i++) {
					for (int j = 0; j < texmatrix [0].GetLength (2); j++) {
						data [0, i * texmatrix [0].GetLength (1) + j] = texmatrix[0] [0, i, j, 0];
					}
				}

				if (isfirst) {
					isfirst = false;

					TFSession.Runner runner = session.GetRunner ();

					runner.AddInput (graph ["input_data"] [0], data);
					runner.AddInput (graph ["weight"] [0], weightData);
					runner.AddInput (graph ["bias"] [0], biasData);		
					runner.Fetch (y);
					runner.Run ();

					float[,] output = runner.Run () [0].GetValue () as float [,];

					for (int i = 0; i < 10; i++) {
						Debug.Log (i+" : "+output [0, i]);
					}

					isfirst = true;
				}


			}
		}
	}

	public override void AgentReset()
	{
		Debug.Log ("Enter");
		result = new float[10];

		graph = new TFGraph ();

		x = graph.Placeholder (TFDataType.Float, new TFShape(1, 784), "input_data");	W = graph.Placeholder ( TFDataType.Float, new TFShape(784,10) , "weight");
		W = graph ["weight"] [0];
		b = graph.Placeholder ( TFDataType.Float, new TFShape(1,10) , "bias");
		y = graph.Softmax ( graph.Add( graph.MatMul (x, W) , b) , "result" );

/*		if (graphModel != null) {
			try{
				graph.Import (graphModel.bytes);
			}
			catch(TFException ex){
				Debug.Log (ex.Message);
			}
		}
*/
		weightData = new float[784,10];
		biasData = new float[10,1];
		if (weightDataPath != "") {
			TextAsset asset = Resources.Load<TextAsset> (weightDataPath);
			if (asset != null) {
				weightData = Parse (weightDataPath, 784,10);
			}

			asset = Resources.Load<TextAsset> (biasDataPath);
			if (asset != null) {
				biasData = Parse (biasDataPath, 784, 10);
			}
		}
	
		session = new TFSession (graph);

	}

	float[,] Parse(string path, int r, int c){

		StreamReader sr = new StreamReader ("Assets/Resources/"+path+".txt");

		bool startarr = false;
		bool endarr = false;

		// [ 0. 0. 0. ... 0. 0. ]

		string source = sr.ReadLine ();

		string splitsource = "";

		string[,] data = new string[r,c];
		int row = 0;

		while(true){

			if (source.Contains ("["))
				startarr = true;

			if (startarr) {

				splitsource += source;
			}

			if (source.Contains ("]")) 
				endarr = true;
			
			if (startarr && endarr) {

				string[] arr = splitsource.Substring (1, splitsource.Length - 2).Split (' ');

				for (int i = 0; i < arr.Length; i++) {
					data [row, i] = arr [i];
				}

				startarr = false;
				endarr = false;
				splitsource = "";
				row++;
			}
				
			source = sr.ReadLine ();

			if (source == null) {
				break;
			}
		}


		float [,] floatdata = new float[r,c];

		for (int i = 0; i < r; i++){ 
			for (int j = 0; j < c; j++){
				float.TryParse (data [i, j], out floatdata [i, j]);
			}
		}

		return floatdata;

	}

	public override void AgentOnDone()
	{

	}
}
