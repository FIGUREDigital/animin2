using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Sprites;
using System.Collections.Generic;

// DefaultPackerPolicy will pack rectangles no matter what Sprite mesh type is unless their packing tag contains "[TIGHT]".
class PhiTexturePackPolicy : UnityEditor.Sprites.IPackerPolicy
{
    protected class Entry
    {
        public Sprite sprite;
        public AtlasSettings settings;
        public string atlasName;
        public SpritePackingMode packingMode;
    }
    public virtual int GetVersion() { return 1; }

    protected virtual string TagPrefix { get { return "[TIGHT]"; } }
    protected virtual bool AllowTightWhenTagged { get { return true; } }

    public void OnGroupAtlases(BuildTarget target, PackerJob job, int[] textureImporterInstanceIDs)
    {

        Debug.Log("Pack ");
        List<Entry> entries = new List<Entry>();

        foreach (int instanceID in textureImporterInstanceIDs)
        {
            TextureImporter ti = EditorUtility.InstanceIDToObject(instanceID) as TextureImporter;

            TextureFormat textureFormat;
            ColorSpace colorSpace;
            int compressionQuality;
#if UNITY_5
            ti.ReadTextureImportInstructions(target, out textureFormat, out colorSpace, out compressionQuality);
#else
            TextureImportInstructions ins = new TextureImportInstructions();
            ti.ReadTextureImportInstructions(ins, target);
            textureFormat = ins.desiredFormat;
            colorSpace = ins.colorSpace;
            compressionQuality = ins.compressionQuality;

#endif
			compressionQuality = 100;
            TextureImporterSettings tis = new TextureImporterSettings();
            ti.ReadTextureSettings(tis);

            Sprite[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(ti.assetPath).Select(x => x as Sprite).Where(x => x != null).ToArray();
            foreach (Sprite sprite in sprites)
            {

                //                Debug.Log("Pack " + sprite.name);
                Entry entry = new Entry();
                entry.sprite = sprite;
                entry.settings.format = textureFormat;
#if !UNITY_5
                entry.settings.usageMode = ins.usageMode;
#endif
                entry.settings.colorSpace = colorSpace;
                entry.settings.compressionQuality = compressionQuality;
                entry.settings.filterMode = Enum.IsDefined(typeof(FilterMode), ti.filterMode) ? ti.filterMode : FilterMode.Bilinear;
                entry.settings.maxWidth = 4096;
                entry.settings.maxHeight = 4096;
                ParsedName parsedName = ParseAtlasName(ti.spritePackingTag);
                entry.atlasName = parsedName.name;
                entry.packingMode = GetPackingMode(parsedName, tis.spriteMeshType);
                entry.settings.generateMipMaps = parsedName.generateMips;
                entry.settings.paddingPower = (uint)parsedName.padding;

                entries.Add(entry);
            }

            Resources.UnloadAsset(ti);
        }

        // First split sprites into groups based on atlas name
        var atlasGroups =
            from e in entries
            group e by e.atlasName;
        foreach (var atlasGroup in atlasGroups)
        {
            int page = 0;
            // Then split those groups into smaller groups based on texture settings
            var settingsGroups =
                from t in atlasGroup
                group t by t.settings;
            foreach (var settingsGroup in settingsGroups)
            {
                string atlasName = atlasGroup.Key;
                if (settingsGroups.Count() > 1)
                    atlasName += string.Format(" (Group {0})", page);

                job.AddAtlas(atlasName, settingsGroup.Key);
                foreach (Entry entry in settingsGroup)
                {
                    job.AssignToAtlas(atlasName, entry.sprite, entry.packingMode, SpritePackingRotation.None);
                }

                ++page;
            }
        }
    }

    protected bool IsTagPrefixed(string packingTag)
    {
        packingTag = packingTag.Trim();
        if (packingTag.Length < TagPrefix.Length)
            return false;
        return (packingTag.Substring(0, TagPrefix.Length) == TagPrefix);
    }


    class ParsedName
    {
        public string name;
        public bool tight = false;
        public int padding = 3;
        public bool generateMips = true;
    }

    private ParsedName ParseAtlasName(string packingTag)
    {
        ParsedName parsed = new ParsedName();
        // Strip off any tags
        string name = packingTag.Trim();

        string stripped = "";
        int insideTag = 0;
        string tag = "";
        foreach (char c in name)
        {
            if (c == '[')
            {
                insideTag++;
                tag = "";
            }
            else if (c == ']' && insideTag > 0)
            {
                if (tag.StartsWith("Pad", true, System.Globalization.CultureInfo.InvariantCulture))
                {
                    tag = tag.Substring(3);
                    int padding;
                    if (int.TryParse(tag, out padding))
                    {
                        parsed.padding = padding;
                    }
                }
                if (tag.Equals("Tight", StringComparison.InvariantCultureIgnoreCase))
                {
                    parsed.tight = true;
                }
                else if (tag.Equals("NoMip", StringComparison.InvariantCultureIgnoreCase))
                {
                    parsed.generateMips = false;
                }
                insideTag--;
            }
            else if (insideTag == 0)
            {
                stripped = stripped + c;
            }
            else
            {
                tag = tag + c;
            }
        }
        parsed.name = (stripped.Length == 0) ? "(unnamed)" : stripped;
        return parsed;
    }

    private SpritePackingMode GetPackingMode(ParsedName parsedName, SpriteMeshType meshType)
    {
        if (meshType == SpriteMeshType.Tight)
        {
            if (AllowTightWhenTagged && parsedName.tight)
            {
                return SpritePackingMode.Tight;
            }
        }
        return SpritePackingMode.Rectangle;
    }
}
