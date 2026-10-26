#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Tools > Castle Door > Generate Prefab
/// Builds an arched double castle gate (oak planks, iron straps, studs, ring
/// handles, stone voussoir frame with keystone), saves meshes, procedural
/// textures and materials under Assets/CastleDoor/Generated, and creates
/// Assets/CastleDoor/CastleDoor.prefab. Works with Built-in, URP and HDRP.
/// </summary>
public static class CastleDoorGenerator
{
    const string BaseDir = "Assets/CastleDoor";
    const string OutDir = "Assets/CastleDoor/Generated";

    // Dimensions in metres
    const float R = 1.1f;          // half opening width = arch radius
    const float H = 2.3f;          // height where the arch starts
    const float Overlap = 0.06f;   // leaves overlap the opening, hidden behind the frame
    const float LeafT = 0.12f;     // leaf thickness
    const float FrameD = 0.6f;     // frame depth
    const float JambW = 0.45f;     // jamb stone width (long stones)
    const float ArchW = 0.42f;     // voussoir radial depth
    const float CenterGap = 0.01f; // gap between the two leaves
    const float Mortar = 0.012f;   // gap between stones

    [MenuItem("Tools/Castle Door/Generate Prefab")]
    public static void Generate()
    {
        Directory.CreateDirectory(OutDir);
        AssetDatabase.Refresh();

        Texture2D woodTex = SaveTexture(MakeWoodTexture(512), "CastleDoor_Wood");
        Texture2D ironTex = SaveTexture(MakeIronTexture(256), "CastleDoor_Iron");
        Texture2D stoneTex = SaveTexture(MakeStoneTexture(512), "CastleDoor_Stone");

        Material wood = MakeMaterial("CastleDoor_Wood", woodTex, Color.white, 0.2f, 0f);
        Material iron = MakeMaterial("CastleDoor_Iron", ironTex, Color.white, 0.5f, 0.8f);
        Material stone = MakeMaterial("CastleDoor_Stone", stoneTex, Color.white, 0.1f, 0f);
        Material mortar = MakeMaterial("CastleDoor_Mortar", stoneTex, new Color(0.45f, 0.43f, 0.40f), 0.05f, 0f);

        float ro = R + Overlap;
        float zc = FrameD * 0.5f + 0.005f + LeafT * 0.5f;
        Vector3 leftPivot = new Vector3(-ro, 0f, zc);
        Vector3 rightPivot = new Vector3(ro, 0f, zc);

        Mesh frameMesh = SaveMesh(BuildFrame(), "CastleDoor_Frame");
        Mesh leftMesh = SaveMesh(BuildLeaf(-ro, -CenterGap * 0.5f, leftPivot, true, 11), "CastleDoor_LeafLeft");
        Mesh rightMesh = SaveMesh(BuildLeaf(CenterGap * 0.5f, ro, rightPivot, false, 23), "CastleDoor_LeafRight");

        GameObject root = new GameObject("CastleDoor");
        MakeMeshObject("Frame", root.transform, frameMesh, new[] { stone, mortar }, false);

        Transform lh = new GameObject("LeftHinge").transform;
        lh.SetParent(root.transform, false);
        lh.localPosition = leftPivot;
        MakeMeshObject("LeftLeaf", lh, leftMesh, new[] { wood, iron }, true);

        Transform rh = new GameObject("RightHinge").transform;
        rh.SetParent(root.transform, false);
        rh.localPosition = rightPivot;
        MakeMeshObject("RightLeaf", rh, rightMesh, new[] { wood, iron }, true);

        CastleDoor door = root.AddComponent<CastleDoor>();
        door.leftHinge = lh;
        door.rightHinge = rh;

        string prefabPath = BaseDir + "/CastleDoor.prefab";
        PrefabUtility.SaveAsPrefabAssetAndConnect(root, prefabPath, InteractionMode.UserAction);
        Undo.RegisterCreatedObjectUndo(root, "Create Castle Door");
        Selection.activeGameObject = root;
        AssetDatabase.SaveAssets();
        Debug.Log("Castle door prefab created at " + prefabPath);
    }

    // ------------------------------------------------------------------ geometry

    static float ArchTop(float x, float r) => H + Mathf.Sqrt(Mathf.Max(0f, r * r - x * x));
    static Vector2 Arc(float r, float a) => new Vector2(Mathf.Cos(a) * r, H + Mathf.Sin(a) * r);
    static Vector2 Rand2(System.Random rnd) => new Vector2((float)rnd.NextDouble(), (float)rnd.NextDouble());

    static Mesh BuildLeaf(float x0, float x1, Vector3 pivot, bool handleOnRight, int seed)
    {
        var mb = new MeshBuilder(2) { Origin = pivot };
        var rnd = new System.Random(seed);
        float ro = R + Overlap;
        float zF = pivot.z - LeafT * 0.5f; // front face (towards -Z)
        float zB = pivot.z + LeafT * 0.5f;

        // Vertical planks, each following the arched top.
        const int planks = 4;
        const float gap = 0.006f;
        float w = (x1 - x0) / planks;
        mb.Sub = 0;
        for (int p = 0; p < planks; p++)
        {
            float a = x0 + p * w + gap * 0.5f;
            float b = x0 + (p + 1) * w - gap * 0.5f;
            var outline = new List<Vector2> { new Vector2(a, 0f), new Vector2(b, 0f) };
            const int steps = 10;
            for (int s = 0; s <= steps; s++)
            {
                float x = Mathf.Lerp(b, a, s / (float)steps);
                outline.Add(new Vector2(x, ArchTop(x, ro)));
            }
            float jitter = (float)rnd.NextDouble() * 0.008f;
            mb.UVOffset = Rand2(rnd);
            mb.Prism(outline, zF - jitter, zB);
        }

        // Iron straps with studs.
        mb.Sub = 1;
        mb.UVScale = 2f;
        float bandT = 0.014f;
        float bandZ = zF - 0.008f - bandT * 0.5f;
        float bandFront = bandZ - bandT * 0.5f;
        foreach (float y in new[] { 0.4f, 1.4f, 2.1f })
        {
            mb.UVOffset = Rand2(rnd);
            mb.Box(new Vector3((x0 + x1) * 0.5f, y, bandZ), new Vector3(x1 - x0 - 0.04f, 0.1f, bandT), Quaternion.identity);
            int count = Mathf.Max(2, Mathf.FloorToInt((x1 - x0 - 0.08f) / 0.15f));
            for (int k = 0; k < count; k++)
            {
                float x = Mathf.Lerp(x0 + 0.06f, x1 - 0.06f, k / (float)(count - 1));
                mb.CylinderZ(new Vector2(x, y), 0.02f, 0.011f, bandFront, bandFront - 0.014f, 8);
            }
        }

        // Ring handle near the centre edge.
        float hx = handleOnRight ? x1 - 0.2f : x0 + 0.2f;
        const float hy = 1.0f;
        float plateT = 0.012f;
        float plateZ = zF - 0.008f - plateT * 0.5f;
        float plateFront = plateZ - plateT * 0.5f;
        float ringZ = plateFront - 0.03f;
        mb.Box(new Vector3(hx, hy, plateZ), new Vector3(0.14f, 0.14f, plateT), Quaternion.Euler(0f, 0f, 45f));
        mb.CylinderZ(new Vector2(hx, hy), 0.014f, 0.014f, plateFront, ringZ, 8);
        mb.TorusZ(new Vector3(hx, hy - 0.085f, ringZ), 0.085f, 0.013f, 20, 8);

        return mb.ToMesh("CastleDoor_Leaf");
    }

    static Mesh BuildFrame()
    {
        var mb = new MeshBuilder(2);
        var rnd = new System.Random(7);
        float zF = -FrameD * 0.5f;
        float zB = FrameD * 0.5f;

        // Jambs: alternating long/short stones (quoins).
        const int rows = 6;
        float rh = H / rows;
        foreach (float side in new[] { -1f, 1f })
        {
            for (int i = 0; i < rows; i++)
            {
                float w = (i % 2 == 1) ? JambW : JambW * 0.72f;
                float y0 = i * rh + (i == 0 ? 0f : Mortar * 0.5f);
                float y1 = (i + 1) * rh - Mortar * 0.5f;
                float front = zF - (float)rnd.NextDouble() * 0.025f;
                mb.Sub = 0;
                mb.UVOffset = Rand2(rnd);
                mb.Box(new Vector3(side * (R + w * 0.5f), (y0 + y1) * 0.5f, (front + zB) * 0.5f),
                       new Vector3(w, y1 - y0, zB - front), Quaternion.identity);
            }
            // Mortar core behind the gaps.
            float coreW = JambW * 0.72f - 0.02f;
            mb.Sub = 1;
            mb.Box(new Vector3(side * (R + 0.01f + coreW * 0.5f), H * 0.5f, 0.0035f),
                   new Vector3(coreW, H, FrameD - 0.017f), Quaternion.identity);
        }

        // Arch: voussoirs with a protruding keystone.
        const int n = 9;
        float ga = Mortar * 0.5f / R;
        for (int i = 0; i < n; i++)
        {
            bool key = i == n / 2;
            float a0 = i * Mathf.PI / n + ga;
            float a1 = (i + 1) * Mathf.PI / n - ga;
            float am = (a0 + a1) * 0.5f;
            float ro = R + ArchW + (key ? 0.12f : (float)rnd.NextDouble() * 0.04f);
            var o = new List<Vector2> { Arc(R, a0), Arc(ro, a0), Arc(ro, am), Arc(ro, a1), Arc(R, a1), Arc(R, am) };
            float front = zF - (key ? 0.06f : (float)rnd.NextDouble() * 0.025f);
            mb.Sub = 0;
            mb.UVOffset = Rand2(rnd);
            mb.Prism(o, front, zB);

            // Mortar behind this stone and its gaps.
            float b0 = i * Mathf.PI / n, b1 = (i + 1) * Mathf.PI / n;
            var m = new List<Vector2> { Arc(R + 0.01f, b0), Arc(R + ArchW - 0.02f, b0), Arc(R + ArchW - 0.02f, b1), Arc(R + 0.01f, b1) };
            mb.Sub = 1;
            mb.Prism(m, zF + 0.012f, zB - 0.005f);
        }

        return mb.ToMesh("CastleDoor_Frame");
    }

    // ------------------------------------------------------------------ textures

    // Tileable Perlin noise in [0,1] over the unit square.
    static float TileNoise(float u, float v, float fx, float fy, float seed)
    {
        float P(float x, float y) => Mathf.PerlinNoise((x + 10f) * fx + seed * 13.7f, (y + 10f) * fy + seed * 7.3f);
        float n = P(u, v) * (1 - u) * (1 - v) + P(u - 1, v) * u * (1 - v)
                + P(u, v - 1) * (1 - u) * v + P(u - 1, v - 1) * u * v;
        return Mathf.Clamp01((n - 0.5f) * 1.8f + 0.5f);
    }

    static float Smooth(float a, float b, float x)
    {
        float t = Mathf.InverseLerp(a, b, x);
        return t * t * (3f - 2f * t);
    }

    static Texture2D MakeWoodTexture(int s)
    {
        var tex = new Texture2D(s, s, TextureFormat.RGBA32, true);
        var px = new Color[s * s];
        Color dark = new Color(0.20f, 0.12f, 0.07f), mid = new Color(0.36f, 0.23f, 0.13f), light = new Color(0.50f, 0.34f, 0.20f);
        for (int y = 0; y < s; y++)
            for (int x = 0; x < s; x++)
            {
                float u = x / (float)s, v = y / (float)s;
                float warp = TileNoise(u, v, 2f, 0.6f, 1.3f);
                float rings = Mathf.Sin((u * 18f + warp * 2.5f) * Mathf.PI * 2f) * 0.5f + 0.5f;
                float fibers = TileNoise(u, v, 90f, 3f, 7.1f);
                float blotch = TileNoise(u, v, 3f, 3f, 4.2f);
                Color c = Color.Lerp(mid, light, fibers * 0.7f);
                c = Color.Lerp(c, dark, Mathf.Pow(rings, 4f) * 0.75f);
                c *= Mathf.Lerp(0.8f, 1.1f, blotch);
                c.a = 1f;
                px[y * s + x] = c;
            }
        tex.SetPixels(px);
        tex.Apply();
        return tex;
    }

    static Texture2D MakeStoneTexture(int s)
    {
        var tex = new Texture2D(s, s, TextureFormat.RGBA32, true);
        var px = new Color[s * s];
        var rnd = new System.Random(3);
        Color dark = new Color(0.30f, 0.29f, 0.27f), light = new Color(0.62f, 0.60f, 0.55f);
        for (int y = 0; y < s; y++)
            for (int x = 0; x < s; x++)
            {
                float u = x / (float)s, v = y / (float)s;
                float n = 0.5f * TileNoise(u, v, 4f, 4f, 2.1f) + 0.3f * TileNoise(u, v, 11f, 11f, 5.5f) + 0.2f * TileNoise(u, v, 37f, 37f, 9.9f);
                Color c = Color.Lerp(dark, light, n);
                float speck = ((float)rnd.NextDouble() - 0.5f) * 0.06f;
                c = new Color(c.r + speck, c.g + speck, c.b + speck, 1f);
                px[y * s + x] = c;
            }
        tex.SetPixels(px);
        tex.Apply();
        return tex;
    }

    static Texture2D MakeIronTexture(int s)
    {
        var tex = new Texture2D(s, s, TextureFormat.RGBA32, true);
        var px = new Color[s * s];
        Color a = new Color(0.10f, 0.10f, 0.11f), b = new Color(0.24f, 0.24f, 0.25f), rust = new Color(0.32f, 0.17f, 0.08f);
        for (int y = 0; y < s; y++)
            for (int x = 0; x < s; x++)
            {
                float u = x / (float)s, v = y / (float)s;
                Color c = Color.Lerp(a, b, TileNoise(u, v, 20f, 20f, 3.3f));
                c = Color.Lerp(c, rust, Smooth(0.55f, 0.8f, TileNoise(u, v, 5f, 5f, 8.8f)) * 0.7f);
                c.a = 1f;
                px[y * s + x] = c;
            }
        tex.SetPixels(px);
        tex.Apply();
        return tex;
    }

    // ------------------------------------------------------------------ assets

    static Texture2D SaveTexture(Texture2D tex, string name)
    {
        string path = OutDir + "/" + name + ".png";
        File.WriteAllBytes(path, tex.EncodeToPNG());
        Object.DestroyImmediate(tex);
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        var imp = (TextureImporter)AssetImporter.GetAtPath(path);
        imp.wrapMode = TextureWrapMode.Repeat;
        imp.anisoLevel = 4;
        imp.SaveAndReimport();
        return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
    }

    static Shader PickShader()
    {
        Shader s = null;
        RenderPipelineAsset rp = GraphicsSettings.currentRenderPipeline;
        if (rp != null)
        {
            if (rp.GetType().Name.Contains("HDRender")) s = Shader.Find("HDRP/Lit");
            if (s == null) s = Shader.Find("Universal Render Pipeline/Lit");
        }
        if (s == null) s = Shader.Find("Standard");
        return s;
    }

    static Material MakeMaterial(string name, Texture2D tex, Color tint, float smoothness, float metallic)
    {
        var m = new Material(PickShader()) { name = name };
        m.mainTexture = tex;
        m.color = tint;
        if (m.HasProperty("_Smoothness")) m.SetFloat("_Smoothness", smoothness);
        if (m.HasProperty("_Glossiness")) m.SetFloat("_Glossiness", smoothness);
        if (m.HasProperty("_Metallic")) m.SetFloat("_Metallic", metallic);

        string path = OutDir + "/" + name + ".mat";
        var existing = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (existing != null)
        {
            EditorUtility.CopySerialized(m, existing);
            Object.DestroyImmediate(m);
            return existing;
        }
        AssetDatabase.CreateAsset(m, path);
        return m;
    }

    static Mesh SaveMesh(Mesh mesh, string name)
    {
        mesh.name = name;
        string path = OutDir + "/" + name + ".asset";
        var existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        if (existing != null)
        {
            EditorUtility.CopySerialized(mesh, existing);
            Object.DestroyImmediate(mesh);
            return existing;
        }
        AssetDatabase.CreateAsset(mesh, path);
        return mesh;
    }

    static GameObject MakeMeshObject(string name, Transform parent, Mesh mesh, Material[] mats, bool convexCollider)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.AddComponent<MeshFilter>().sharedMesh = mesh;
        go.AddComponent<MeshRenderer>().sharedMaterials = mats;
        var mc = go.AddComponent<MeshCollider>();
        mc.sharedMesh = mesh;
        mc.convex = convexCollider;
        return go;
    }

    // ------------------------------------------------------------------ mesh builder

    class MeshBuilder
    {
        readonly List<Vector3> verts = new List<Vector3>();
        readonly List<Vector3> norms = new List<Vector3>();
        readonly List<Vector2> uvs = new List<Vector2>();
        readonly List<int>[] subs;

        public int Sub;
        public Vector3 Origin;
        public float UVScale = 1f;
        public Vector2 UVOffset;

        public MeshBuilder(int subCount)
        {
            subs = new List<int>[subCount];
            for (int i = 0; i < subCount; i++) subs[i] = new List<int>();
        }

        public void Tri(Vector3 a, Vector3 b, Vector3 c, Vector3 n)
        {
            // Unity front faces are clockwise; their Cross(b-a, c-a) points at the viewer.
            if (Vector3.Dot(Vector3.Cross(b - a, c - a), n) < 0f) { Vector3 t = b; b = c; c = t; }
            int i = verts.Count;
            Add(a, n); Add(b, n); Add(c, n);
            subs[Sub].Add(i); subs[Sub].Add(i + 1); subs[Sub].Add(i + 2);
        }

        void Add(Vector3 p, Vector3 n)
        {
            verts.Add(p - Origin);
            norms.Add(n);
            Vector3 an = new Vector3(Mathf.Abs(n.x), Mathf.Abs(n.y), Mathf.Abs(n.z));
            Vector2 uv;
            if (an.z >= an.x && an.z >= an.y) uv = new Vector2(p.x, p.y);
            else if (an.x >= an.y) uv = new Vector2(p.z, p.y);
            else uv = new Vector2(p.x, p.z);
            uvs.Add(uv * UVScale + UVOffset);
        }

        public void Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 n)
        {
            Tri(a, b, c, n);
            Tri(a, c, d, n);
        }

        public void QuadAuto(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 hint)
        {
            Vector3 n = Vector3.Cross(b - a, d - a);
            if (n.sqrMagnitude < 1e-12f) n = hint;
            n.Normalize();
            if (Vector3.Dot(n, hint) < 0f) n = -n;
            Quad(a, b, c, d, n);
        }

        public void Poly(IList<Vector3> pts, Vector3 n)
        {
            Vector3 cen = Vector3.zero;
            foreach (var p in pts) cen += p;
            cen /= pts.Count;
            for (int i = 0; i < pts.Count; i++)
                Tri(cen, pts[i], pts[(i + 1) % pts.Count], n);
        }

        // Extrudes a star-shaped XY outline between zFront (< zBack) and zBack.
        public void Prism(IList<Vector2> o, float zFront, float zBack)
        {
            int count = o.Count;
            Vector2 cen = Vector2.zero;
            foreach (var p in o) cen += p;
            cen /= count;

            var f = new List<Vector3>(count);
            var b = new List<Vector3>(count);
            for (int i = 0; i < count; i++)
            {
                f.Add(new Vector3(o[i].x, o[i].y, zFront));
                b.Add(new Vector3(o[i].x, o[i].y, zBack));
            }
            Poly(f, Vector3.back);
            Poly(b, Vector3.forward);

            for (int i = 0; i < count; i++)
            {
                int j = (i + 1) % count;
                Vector2 e = o[j] - o[i];
                if (e.sqrMagnitude < 1e-10f) continue;
                Vector3 n = new Vector3(e.y, -e.x, 0f).normalized;
                Vector2 mid = (o[i] + o[j]) * 0.5f;
                if (Vector2.Dot(new Vector2(n.x, n.y), mid - cen) < 0f) n = -n;
                Quad(f[i], f[j], b[j], b[i], n);
            }
        }

        public void Box(Vector3 c, Vector3 size, Quaternion rot)
        {
            Vector3 h = size * 0.5f;
            var p = new Vector3[8];
            for (int i = 0; i < 8; i++)
                p[i] = c + rot * new Vector3((i & 1) == 0 ? -h.x : h.x, (i & 2) == 0 ? -h.y : h.y, (i & 4) == 0 ? -h.z : h.z);
            Quad(p[0], p[2], p[6], p[4], rot * Vector3.left);
            Quad(p[1], p[3], p[7], p[5], rot * Vector3.right);
            Quad(p[0], p[1], p[5], p[4], rot * Vector3.down);
            Quad(p[2], p[3], p[7], p[6], rot * Vector3.up);
            Quad(p[0], p[1], p[3], p[2], rot * Vector3.back);
            Quad(p[4], p[5], p[7], p[6], rot * Vector3.forward);
        }

        // Frustum along Z from z0 (radius r0) to z1 (radius r1), capped at z1.
        public void CylinderZ(Vector2 c, float r0, float r1, float z0, float z1, int seg)
        {
            var cap = new List<Vector3>(seg);
            for (int i = 0; i < seg; i++)
            {
                float a0 = i * Mathf.PI * 2f / seg, a1 = (i + 1) * Mathf.PI * 2f / seg, am = (a0 + a1) * 0.5f;
                Vector3 p00 = new Vector3(c.x + Mathf.Cos(a0) * r0, c.y + Mathf.Sin(a0) * r0, z0);
                Vector3 p10 = new Vector3(c.x + Mathf.Cos(a1) * r0, c.y + Mathf.Sin(a1) * r0, z0);
                Vector3 p01 = new Vector3(c.x + Mathf.Cos(a0) * r1, c.y + Mathf.Sin(a0) * r1, z1);
                Vector3 p11 = new Vector3(c.x + Mathf.Cos(a1) * r1, c.y + Mathf.Sin(a1) * r1, z1);
                QuadAuto(p00, p10, p11, p01, new Vector3(Mathf.Cos(am), Mathf.Sin(am), 0f));
                cap.Add(p01);
            }
            Poly(cap, z1 < z0 ? Vector3.back : Vector3.forward);
        }

        // Torus lying in the XY plane (axis along Z).
        public void TorusZ(Vector3 c, float major, float minor, int segU, int segV)
        {
            Vector3 P(int i, int j)
            {
                float u = i * Mathf.PI * 2f / segU, v = j * Mathf.PI * 2f / segV;
                float rr = major + minor * Mathf.Cos(v);
                return c + new Vector3(Mathf.Cos(u) * rr, Mathf.Sin(u) * rr, minor * Mathf.Sin(v));
            }
            for (int i = 0; i < segU; i++)
                for (int j = 0; j < segV; j++)
                {
                    Vector3 a = P(i, j), b = P(i + 1, j), cc = P(i + 1, j + 1), d = P(i, j + 1);
                    float um = (i + 0.5f) * Mathf.PI * 2f / segU;
                    Vector3 ring = c + new Vector3(Mathf.Cos(um), Mathf.Sin(um), 0f) * major;
                    QuadAuto(a, b, cc, d, (a + b + cc + d) * 0.25f - ring);
                }
        }

        public Mesh ToMesh(string name)
        {
            var m = new Mesh { name = name };
            if (verts.Count > 65000) m.indexFormat = IndexFormat.UInt32;
            m.SetVertices(verts);
            m.SetNormals(norms);
            m.SetUVs(0, uvs);
            m.subMeshCount = subs.Length;
            for (int i = 0; i < subs.Length; i++) m.SetTriangles(subs[i], i);
            m.RecalculateBounds();
            m.RecalculateTangents();
            return m;
        }
    }
}
#endif
