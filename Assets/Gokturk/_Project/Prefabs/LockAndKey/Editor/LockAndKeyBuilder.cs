using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

/// <summary>
/// Tools > Lock And Key > Build.
/// Builds the chained padlock, the key and the HUD key indicator, and wires them together.
/// Select your door first to chain that door; with nothing selected a demo door is created.
/// </summary>
public static class LockAndKeyBuilder
{
    const string Root = "Assets/LockAndKey";
    const string Generated = Root + "/Generated";
    const string KeyId = "DoorKey";

    static Material iron, dark, gold, wood;
    static Mesh ring, halfRing;

    [MenuItem("Tools/Lock And Key/Build (select your door first)")]
    public static void Build()
    {
        Directory.CreateDirectory(Generated);
        AssetDatabase.Refresh();

        iron = MakeMaterial("Iron", new Color(0.23f, 0.21f, 0.2f), 0.85f, 0.35f);
        dark = MakeMaterial("Keyhole", new Color(0.03f, 0.03f, 0.03f), 0f, 0.1f);
        gold = MakeMaterial("KeyGold", new Color(1f, 0.76f, 0.22f), 1f, 0.7f);
        wood = MakeMaterial("DoorWood", new Color(0.36f, 0.22f, 0.12f), 0f, 0.2f);
        ring = MakeTorus("Ring", 0.028f, 0.008f, 360f);
        halfRing = MakeTorus("HalfRing", 0.04f, 0.011f, 180f);

        // --- Door ---------------------------------------------------------
        Transform door = Selection.activeTransform;
        if (door == null) door = CreateDemoDoor();

        Bounds b = GetBounds(door);
        bool thinOnX = b.size.x < b.size.z;
        float width = thinOnX ? b.size.z : b.size.x;
        float thickness = thinOnX ? b.size.x : b.size.z;
        float height = b.size.y;
        Vector3 normal = thinOnX ? Vector3.right : Vector3.forward;

        // Put the lock on the side facing the Scene view camera.
        SceneView view = SceneView.lastActiveSceneView;
        if (view != null && view.camera != null &&
            Vector3.Dot(normal, view.camera.transform.position - b.center) < 0f)
            normal = -normal;

        // --- Lock ---------------------------------------------------------
        GameObject lockRoot = new GameObject("DoorLock");
        lockRoot.transform.SetPositionAndRotation(
            b.center + normal * (thickness * 0.5f), Quaternion.LookRotation(normal, Vector3.up));

        BoxCollider zone = lockRoot.AddComponent<BoxCollider>();
        zone.isTrigger = true;
        zone.size = new Vector3(width + 0.6f, height, 3f);
        lockRoot.AddComponent<Rigidbody>().isKinematic = true;

        float halfW = width * 0.5f + 0.06f;
        float halfH = Mathf.Min(height * 0.22f, halfW);
        Transform chainA = BuildChain(lockRoot.transform, "ChainA",
            new Vector3(-halfW, -halfH, 0.03f), new Vector3(halfW, halfH, 0.03f));
        Transform chainB = BuildChain(lockRoot.transform, "ChainB",
            new Vector3(-halfW, halfH, 0.045f), new Vector3(halfW, -halfH, 0.045f));

        Transform anchors = new GameObject("Anchors").transform;
        anchors.SetParent(lockRoot.transform, false);
        foreach (float x in new[] { -halfW, halfW })
            foreach (float y in new[] { -halfH, halfH })
                Prim(PrimitiveType.Cylinder, anchors, "Bolt", new Vector3(x, y, 0.015f),
                    new Vector3(0.07f, 0.015f, 0.07f), new Vector3(90f, 0f, 0f), iron);

        Transform padlock = BuildPadlock(lockRoot.transform);
        padlock.localPosition = new Vector3(0f, 0.02f, 0.075f);

        LockedDoor locked = lockRoot.AddComponent<LockedDoor>();
        locked.keyId = KeyId;
        locked.door = door;
        locked.padlock = padlock;
        locked.fallingParts = new[] { padlock, chainA, chainB };

        // --- Key ----------------------------------------------------------
        GameObject key = BuildKey();
        key.transform.position = new Vector3(b.center.x, b.min.y + 1f, b.center.z) + normal * 3f;
        PrefabUtility.SaveAsPrefabAssetAndConnect(key, Root + "/Key.prefab", InteractionMode.AutomatedAction);

        // --- UI -----------------------------------------------------------
        GameObject ui = BuildIndicator();
        PrefabUtility.SaveAsPrefabAssetAndConnect(ui, Root + "/KeyIndicatorCanvas.prefab", InteractionMode.AutomatedAction);

        Undo.RegisterCreatedObjectUndo(lockRoot, "Build Lock And Key");
        Undo.RegisterCreatedObjectUndo(key, "Build Lock And Key");
        Undo.RegisterCreatedObjectUndo(ui, "Build Lock And Key");
        AssetDatabase.SaveAssets();
        Selection.activeGameObject = key;

        if (GameObject.FindWithTag("Player") == null)
            EditorUtility.DisplayDialog("Lock And Key",
                "Built. One thing left: tag your player object as \"Player\" so it can pick up the key and open the door.",
                "OK");
    }

    // ---------------------------------------------------------------------
    // Models
    // ---------------------------------------------------------------------

    static Transform BuildPadlock(Transform parent)
    {
        // Pivot is at the top of the shackle so the lock swings from where it hangs.
        Transform t = new GameObject("Padlock").transform;
        t.SetParent(parent, false);

        const float bodyY = -0.125f;
        Prim(PrimitiveType.Cube, t, "Body", new Vector3(0f, bodyY, 0f), new Vector3(0.14f, 0.11f, 0.05f), Vector3.zero, iron);
        Prim(PrimitiveType.Cube, t, "Band", new Vector3(0f, bodyY + 0.04f, 0f), new Vector3(0.146f, 0.014f, 0.056f), Vector3.zero, dark);
        Prim(PrimitiveType.Cube, t, "Plate", new Vector3(0f, bodyY - 0.008f, 0.026f), new Vector3(0.06f, 0.065f, 0.004f), Vector3.zero, gold);
        Prim(PrimitiveType.Cylinder, t, "KeyholeTop", new Vector3(0f, bodyY, 0.0285f), new Vector3(0.022f, 0.001f, 0.022f), new Vector3(90f, 0f, 0f), dark);
        Prim(PrimitiveType.Cube, t, "KeyholeSlot", new Vector3(0f, bodyY - 0.016f, 0.0285f), new Vector3(0.009f, 0.028f, 0.002f), Vector3.zero, dark);
        Prim(PrimitiveType.Cylinder, t, "LegL", new Vector3(-0.04f, -0.055f, 0f), new Vector3(0.022f, 0.016f, 0.022f), Vector3.zero, iron);
        Prim(PrimitiveType.Cylinder, t, "LegR", new Vector3(0.04f, -0.055f, 0f), new Vector3(0.022f, 0.016f, 0.022f), Vector3.zero, iron);

        GameObject shackle = new GameObject("Shackle", typeof(MeshFilter), typeof(MeshRenderer));
        shackle.transform.SetParent(t, false);
        shackle.transform.localPosition = new Vector3(0f, -0.04f, 0f);
        shackle.GetComponent<MeshFilter>().sharedMesh = halfRing;
        shackle.GetComponent<MeshRenderer>().sharedMaterial = iron;

        BoxCollider col = t.gameObject.AddComponent<BoxCollider>();
        col.center = new Vector3(0f, bodyY, 0f);
        col.size = new Vector3(0.14f, 0.11f, 0.05f);
        col.enabled = false; // switched on when the lock drops

        t.localScale = Vector3.one * 1.7f;
        return t;
    }

    static Transform BuildChain(Transform parent, string name, Vector3 from, Vector3 to)
    {
        // Local X of the chain object runs along the chain.
        Transform t = new GameObject(name).transform;
        t.SetParent(parent, false);
        Vector3 dir = to - from;
        float length = dir.magnitude;
        t.localPosition = (from + to) * 0.5f;
        t.localRotation = Quaternion.FromToRotation(Vector3.right, dir.normalized);

        const float spacing = 0.066f;
        int count = Mathf.Max(2, Mathf.RoundToInt(length / spacing));
        for (int i = 0; i < count; i++)
        {
            GameObject link = new GameObject("Link", typeof(MeshFilter), typeof(MeshRenderer));
            link.transform.SetParent(t, false);
            link.transform.localPosition = new Vector3(-length * 0.5f + (i + 0.5f) * length / count, 0f, 0f);
            link.transform.localRotation = Quaternion.Euler(i % 2 == 0 ? 0f : 90f, 0f, 0f);
            link.transform.localScale = new Vector3(1.5f, 1f, 1f);
            link.GetComponent<MeshFilter>().sharedMesh = ring;
            link.GetComponent<MeshRenderer>().sharedMaterial = iron;
        }

        BoxCollider col = t.gameObject.AddComponent<BoxCollider>();
        col.size = new Vector3(length, 0.06f, 0.06f);
        col.enabled = false;
        return t;
    }

    static GameObject BuildKey()
    {
        GameObject key = new GameObject("Key");
        SphereCollider trigger = key.AddComponent<SphereCollider>();
        trigger.isTrigger = true;
        trigger.radius = 0.6f;
        key.AddComponent<Rigidbody>().isKinematic = true;
        key.AddComponent<KeyPickup>().keyId = KeyId;

        Light glow = key.AddComponent<Light>();
        glow.type = LightType.Point;
        glow.color = new Color(1f, 0.8f, 0.35f);
        glow.range = 2.5f;
        glow.intensity = 1.5f;

        Transform m = new GameObject("Model").transform;
        m.SetParent(key.transform, false);
        m.localRotation = Quaternion.Euler(0f, 0f, 25f);
        m.localScale = Vector3.one * 1.8f;

        GameObject bow = new GameObject("Bow", typeof(MeshFilter), typeof(MeshRenderer));
        bow.transform.SetParent(m, false);
        bow.transform.localPosition = new Vector3(0f, 0.12f, 0f);
        bow.transform.localScale = Vector3.one * 1.6f;
        bow.GetComponent<MeshFilter>().sharedMesh = ring;
        bow.GetComponent<MeshRenderer>().sharedMaterial = gold;

        Prim(PrimitiveType.Cylinder, m, "Collar", new Vector3(0f, 0.065f, 0f), new Vector3(0.036f, 0.008f, 0.036f), Vector3.zero, gold);
        Prim(PrimitiveType.Cylinder, m, "Shaft", new Vector3(0f, -0.04f, 0f), new Vector3(0.022f, 0.11f, 0.022f), Vector3.zero, gold);
        Prim(PrimitiveType.Cube, m, "ToothA", new Vector3(0.03f, -0.13f, 0f), new Vector3(0.05f, 0.02f, 0.012f), Vector3.zero, gold);
        Prim(PrimitiveType.Cube, m, "ToothB", new Vector3(0.024f, -0.095f, 0f), new Vector3(0.036f, 0.02f, 0.012f), Vector3.zero, gold);
        return key;
    }

    static Transform CreateDemoDoor()
    {
        Vector3 pos = Vector3.zero;
        SceneView view = SceneView.lastActiveSceneView;
        if (view != null) pos = new Vector3(view.pivot.x, 0f, view.pivot.z);

        // The hinge object is what rotates; the door panel is offset so the pivot is on its edge.
        Transform hinge = new GameObject("Door (hinge)").transform;
        hinge.position = pos;
        GameObject panel = GameObject.CreatePrimitive(PrimitiveType.Cube);
        panel.name = "DoorPanel";
        panel.transform.SetParent(hinge, false);
        panel.transform.localPosition = new Vector3(0.6f, 1.1f, 0f);
        panel.transform.localScale = new Vector3(1.2f, 2.2f, 0.1f);
        panel.GetComponent<MeshRenderer>().sharedMaterial = wood;
        Undo.RegisterCreatedObjectUndo(hinge.gameObject, "Build Lock And Key");
        return hinge;
    }

    // ---------------------------------------------------------------------
    // UI
    // ---------------------------------------------------------------------

    static GameObject BuildIndicator()
    {
        GameObject canvasGO = new GameObject("KeyIndicatorCanvas", typeof(Canvas), typeof(CanvasScaler));
        canvasGO.layer = 5;
        canvasGO.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        Image slot = UIImage("KeySlot", canvasGO.transform, 120f,
            AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd"),
            new Color(0.09f, 0.07f, 0.05f, 0.8f));
        slot.type = Image.Type.Sliced;
        RectTransform rt = slot.rectTransform;
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(1f, 1f); // top-right corner
        rt.anchoredPosition = new Vector2(-40f, -40f);
        Outline border = slot.gameObject.AddComponent<Outline>();
        border.effectColor = new Color(0.72f, 0.53f, 0.2f, 0.9f);
        border.effectDistance = new Vector2(2f, -2f);

        Image glow = UIImage("Glow", slot.transform, 150f,
            AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd"),
            new Color(1f, 0.82f, 0.25f, 0f));

        Image icon = UIImage("KeyIcon", slot.transform, 88f, LoadKeySprite(), new Color(1f, 1f, 1f, 0.18f));
        icon.preserveAspect = true;

        KeyIndicatorUI indicator = canvasGO.AddComponent<KeyIndicatorUI>();
        indicator.keyId = KeyId;
        indicator.slot = rt;
        indicator.icon = icon;
        indicator.glow = glow;
        return canvasGO;
    }

    static Image UIImage(string name, Transform parent, float size, Sprite sprite, Color color)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
        go.layer = 5;
        go.transform.SetParent(parent, false);
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
        Image img = go.GetComponent<Image>();
        img.sprite = sprite;
        img.color = color;
        img.raycastTarget = false;
        return img;
    }

    static Sprite LoadKeySprite()
    {
        string[] guids = AssetDatabase.FindAssets("KeyIcon t:Texture2D");
        if (guids.Length == 0)
        {
            Debug.LogWarning("Lock And Key: KeyIcon.png not found, assign a sprite to the KeyIcon image yourself.");
            return null;
        }
        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);
        if (importer.textureType != TextureImporterType.Sprite)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.alphaIsTransparency = true;
            importer.SaveAndReimport();
        }
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }

    // ---------------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------------

    static GameObject Prim(PrimitiveType type, Transform parent, string name,
        Vector3 pos, Vector3 scale, Vector3 euler, Material mat)
    {
        GameObject go = GameObject.CreatePrimitive(type);
        go.name = name;
        Object.DestroyImmediate(go.GetComponent<Collider>());
        go.transform.SetParent(parent, false);
        go.transform.localPosition = pos;
        go.transform.localRotation = Quaternion.Euler(euler);
        go.transform.localScale = scale;
        go.GetComponent<MeshRenderer>().sharedMaterial = mat;
        return go;
    }

    static Bounds GetBounds(Transform t)
    {
        Renderer[] renderers = t.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds(t.position + Vector3.up * 1.1f, new Vector3(1.2f, 2.2f, 0.1f));
        Bounds b = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++) b.Encapsulate(renderers[i].bounds);
        return b;
    }

    static Material MakeMaterial(string name, Color color, float metallic, float smoothness)
    {
        string path = Generated + "/" + name + ".mat";
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat != null) return mat;

        // Works for Built-in, URP and HDRP.
        RenderPipelineAsset pipeline = GraphicsSettings.currentRenderPipeline;
        Shader shader = pipeline != null ? pipeline.defaultShader : null;
        if (shader == null) shader = Shader.Find("Standard");

        mat = new Material(shader);
        mat.color = color;
        if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", metallic);
        if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", smoothness);
        if (mat.HasProperty("_Glossiness")) mat.SetFloat("_Glossiness", smoothness);
        AssetDatabase.CreateAsset(mat, path);
        return mat;
    }

    /// <summary>Torus (or an arc of one) lying in the XY plane.</summary>
    static Mesh MakeTorus(string name, float radius, float tube, float arcDegrees)
    {
        string path = Generated + "/" + name + ".asset";
        Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        if (mesh != null) return mesh;

        const int segments = 24, sides = 10;
        int stride = sides + 1;
        Vector3[] verts = new Vector3[(segments + 1) * stride];
        Vector3[] normals = new Vector3[verts.Length];
        int[] tris = new int[segments * sides * 6];
        float arc = arcDegrees * Mathf.Deg2Rad;

        for (int i = 0; i <= segments; i++)
        {
            float a = arc * i / segments;
            Vector3 centre = new Vector3(Mathf.Cos(a), Mathf.Sin(a), 0f) * radius;
            for (int j = 0; j <= sides; j++)
            {
                float bAngle = 2f * Mathf.PI * j / sides;
                Vector3 n = new Vector3(Mathf.Cos(a) * Mathf.Cos(bAngle), Mathf.Sin(a) * Mathf.Cos(bAngle), Mathf.Sin(bAngle));
                verts[i * stride + j] = centre + n * tube;
                normals[i * stride + j] = n;
            }
        }

        int k = 0;
        for (int i = 0; i < segments; i++)
        {
            for (int j = 0; j < sides; j++)
            {
                int v0 = i * stride + j;
                int v1 = v0 + stride;
                tris[k++] = v0; tris[k++] = v1; tris[k++] = v0 + 1;
                tris[k++] = v0 + 1; tris[k++] = v1; tris[k++] = v1 + 1;
            }
        }

        mesh = new Mesh { name = name };
        mesh.vertices = verts;
        mesh.normals = normals;
        mesh.triangles = tris;
        mesh.RecalculateBounds();
        AssetDatabase.CreateAsset(mesh, path);
        return mesh;
    }
}
