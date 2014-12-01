using UnityEngine;
using UnityEditor;
using System.Collections;

public class TextureImportSettings : MonoBehaviour
{
    [MenuItem("Custom/Texture/Toggle Texture/Automatic Compressed")]
    static void ToggleCompression_Enable_Automatic_Compressed()
    {
        SelectedCompressionSettings(TextureImporterFormat.AutomaticCompressed);
    }
    [MenuItem("Custom/Texture/Toggle Texture/Automatic Truecolor")]
    static void ToggleCompression_Enable_Automatic_Truecolor()
    {
        SelectedCompressionSettings(TextureImporterFormat.AutomaticTruecolor);
    }


    [MenuItem("Custom/Texture/Max Size/Set size 64")]
    static void ToggleCompression_Enable_64()
    {
        SelectedTextureSizeSettings(64);
    }
    [MenuItem("Custom/Texture/Max Size/Set size 128")]
    static void ToggleCompression_Enable_128()
    {
        SelectedTextureSizeSettings(128);
    }
    [MenuItem("Custom/Texture/Max Size/Set size 256")]
    static void ToggleCompression_Enable_256()
    {
        SelectedTextureSizeSettings(256);
    }

    /*
    static void SelectedToggleCompressionSettings(TextureImporterFormat newFormat, int compressionQuality, int maxTextureSize)
    {

        Object[] textures = GetSelectedTextures();
        Selection.objects = new Object[0];
        foreach (Texture tex in textures)
        {
            string path = AssetDatabase.GetAssetPath(tex);
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            textureImporter.textureFormat = newFormat;
            textureImporter.maxTextureSize = maxTextureSize;
            Debug.Log("Texture : [" + tex.name + "];");
            //textureImporter.compressionQuality = compressionQuality;

            AssetDatabase.ImportAsset(path);
        }
        Debug.Log("Finished Importing " + textures.Length + " Textures");
    }
    */

    static void SelectedCompressionSettings(TextureImporterFormat newFormat)
    {
        Object[] textures = GetSelectedTextures();
        Selection.objects = new Object[0];
        foreach (Texture tex in textures)
        {
            string path = AssetDatabase.GetAssetPath(tex);
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            textureImporter.textureFormat = newFormat;
            Debug.Log("Texture : [" + tex.name + "];");
            AssetDatabase.ImportAsset(path);
        }
    }

    static void SelectedTextureSizeSettings(int maxTextureSize)
    {
        Object[] textures = GetSelectedTextures();
        Selection.objects = new Object[0];
        foreach (Texture tex in textures)
        {
            string path = AssetDatabase.GetAssetPath(tex);
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            textureImporter.maxTextureSize = maxTextureSize;
            AssetDatabase.ImportAsset(path);
        }
        Debug.Log("Finished Importing " + textures.Length + " Textures");
    }





    static Object[] GetSelectedTextures()
    {
        return Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
    }
}
