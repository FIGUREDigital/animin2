using UnityEngine;
using UnityEditor;
using System.Collections;

public class ModelImportSettings : MonoBehaviour
{


	[MenuItem ("Custom/Model/Toggle Model compression/Enable")]
	static void ToggleCompression_Enable ()
	{
		SelectedToggleCompressionSettings (ModelImporterMeshCompression.High, ModelImporterAnimationCompression.Optimal);
	}

	static void SelectedToggleCompressionSettings (ModelImporterMeshCompression modelFormat, ModelImporterAnimationCompression animationFormat)
	{

		Object[] meshes = GetSelectedModels ();
		Selection.objects = new Object[0];
		foreach (Mesh mesh in meshes) {
			string path = AssetDatabase.GetAssetPath (mesh);
			ModelImporter modelImporter = AssetImporter.GetAtPath (path) as ModelImporter;
			modelImporter.meshCompression = modelFormat;
			modelImporter.animationCompression = animationFormat;
			Debug.Log ("Texture : [" + mesh.name + "];");

			AssetDatabase.ImportAsset (path);
		}
		Debug.Log ("Finished Importing " + meshes.Length + " Meshes");
	}

	static Object[] GetSelectedModels ()
	{
		return Selection.GetFiltered (typeof(Mesh), SelectionMode.Editable);
	}
}
