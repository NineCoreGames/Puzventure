using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Tools > Castle Materials > Build.
/// Creates one material per texture set in CastleMaterials/Textures, using the
/// default lit shader of whichever render pipeline the project uses (Built-in, URP or HDRP).
/// </summary>
public static class CastleMaterialBuilder
{
    struct Def
    {
        public string name, texture;
        public Color color, emission;
        public float metallic, smoothness;
        public Def(string name, string texture, float metallic, float smoothness, Color color, Color emission)
        {
            this.name = name; this.texture = texture; this.metallic = metallic;
            this.smoothness = smoothness; this.color = color; this.emission = emission;
        }
    }

    static readonly Color none = Color.black;

    static readonly Def[] defs =
    {
        //      material name        texture set        metal  smooth  tint                            emission
        new Def("StoneBricks",       "StoneBricks",      0f,   0.15f, Color.white,                     none),
        new Def("StoneBricksMossy",  "StoneBricksMossy", 0f,   0.15f, Color.white,                     none),
        new Def("DungeonStone",      "DungeonStone",     0f,   0.25f, Color.white,                     none),
        new Def("Flagstone",         "Flagstone",        0f,   0.2f,  Color.white,                     none),
        new Def("MarbleTiles",       "MarbleTiles",      0f,   0.8f,  Color.white,                     none),
        new Def("Plaster",           "Plaster",          0f,   0.1f,  Color.white,                     none),
        new Def("WoodPlanks",        "WoodPlanks",       0f,   0.3f,  Color.white,                     none),
        new Def("DarkWoodBeam",      "DarkWoodBeam",     0f,   0.25f, Color.white,                     none),
        new Def("IronPlate",         "IronPlate",        0.9f, 0.45f, Color.white,                     none),
        new Def("RustyIron",         "RustyIron",        0.5f, 0.25f, Color.white,                     none),
        new Def("RedCarpet",         "RedCarpet",        0f,   0.05f, Color.white,                     none),
        new Def("BlueBanner",        "BlueBanner",       0f,   0.1f,  Color.white,                     none),
        // Untextured extras
        new Def("GoldTrim",          null,               1f,   0.75f, new Color(1f, 0.76f, 0.24f),     none),
        new Def("TorchEmber",        null,               0f,   0.3f,  new Color(1f, 0.45f, 0.1f),      new Color(1f, 0.4f, 0.08f) * 3f),
        new Def("WindowGlow",        null,               0f,   0.6f,  new Color(0.75f, 0.85f, 1f),     new Color(0.6f, 0.75f, 1f) * 1.5f),
    };

    [MenuItem("Tools/Castle Materials/Build")]
    public static void Build()
    {
        string[] hits = AssetDatabase.FindAssets("StoneBricks_Normal t:Texture2D");
        if (hits.Length == 0)
        {
            EditorUtility.DisplayDialog("Castle Materials", "Could not find the CastleMaterials/Textures folder.", "OK");
            return;
        }
        string texDir = Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(hits[0])).Replace('\\', '/');
        string matDir = Path.GetDirectoryName(texDir).Replace('\\', '/') + "/Materials";
        Directory.CreateDirectory(matDir);
        AssetDatabase.Refresh();

        RenderPipelineAsset pipeline = GraphicsSettings.currentRenderPipeline;
        Shader shader = pipeline != null ? pipeline.defaultShader : null;
        if (shader == null) shader = Shader.Find("Standard");

        foreach (Def d in defs)
        {
            string path = matDir + "/" + d.name + ".mat";
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            bool isNew = mat == null;
            if (isNew) mat = new Material(shader);

            mat.color = d.color;
            SetFloat(mat, d.metallic, "_Metallic");
            SetFloat(mat, d.smoothness, "_Smoothness", "_Glossiness");

            if (d.texture != null)
            {
                Texture2D albedo = AssetDatabase.LoadAssetAtPath<Texture2D>(texDir + "/" + d.texture + ".png");
                Texture2D normal = LoadNormal(texDir + "/" + d.texture + "_Normal.png");
                mat.mainTexture = albedo;
                foreach (string prop in new[] { "_BumpMap", "_NormalMap" })
                    if (mat.HasProperty(prop)) mat.SetTexture(prop, normal);
                mat.EnableKeyword("_NORMALMAP");
            }

            if (d.emission != none)
            {
                foreach (string prop in new[] { "_EmissionColor", "_EmissiveColor" })
                    if (mat.HasProperty(prop)) mat.SetColor(prop, d.emission);
                mat.EnableKeyword("_EMISSION");
                mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.BakedEmissive;
            }

            if (isNew) AssetDatabase.CreateAsset(mat, path);
            else EditorUtility.SetDirty(mat);
        }

        AssetDatabase.SaveAssets();
        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(matDir));
        Debug.Log("Castle Materials: built " + defs.Length + " materials in " + matDir);
    }

    static void SetFloat(Material mat, float value, params string[] props)
    {
        foreach (string prop in props)
            if (mat.HasProperty(prop)) mat.SetFloat(prop, value);
    }

    static Texture2D LoadNormal(string path)
    {
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null && importer.textureType != TextureImporterType.NormalMap)
        {
            importer.textureType = TextureImporterType.NormalMap;
            importer.SaveAndReimport();
        }
        return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
    }
}
