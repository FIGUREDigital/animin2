using UnityEngine;
using System.Collections;
using UnityEditor;

public class ModelPostProcessor : AssetPostprocessor 
{
		void OnPostprocessModel (GameObject go  )
		{

				if (!assetImporter) return;

				ModelImporter mi = assetImporter as ModelImporter;


				if (!mi) return;


				mi.meshCompression = ModelImporterMeshCompression.High;
				mi.animationCompression = ModelImporterAnimationCompression.KeyframeReductionAndCompression;

				Debug.Log ("Imported model :" + mi.name);

		}

}
