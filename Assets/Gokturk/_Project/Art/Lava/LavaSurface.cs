using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

/// <summary>
/// Animated lava surface. Put it on an empty GameObject over the red area of your terrain.
/// At runtime it builds:
///   - a procedural lava texture (dark crust + glowing cracks) with emission
///   - a wavy mesh that slowly bubbles
///   - flowing / pulsing glow animation
///   - optional flickering orange light and rising ember particles
///   - a trigger that kills the player on contact
/// Works with URP and the Built-in render pipeline.
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LavaSurface : MonoBehaviour
{
    [Header("Size")]
    [Tooltip("Width (X) and length (Z) of the lava area in meters.")]
    public Vector2 size = new Vector2(50f, 50f);
    [Range(10, 200)] public int meshResolution = 80;

    [Header("Look")]
    [Tooltip("Optional. Assign a Lit (URP) or Standard (Built-in) material here if the lava turns pink in a build.")]
    public Material baseMaterial;
    [Range(64, 1024)] public int textureSize = 256;
    [Tooltip("How many times the lava pattern repeats across the surface.")]
    public float tiling = 4f;
    public Color crustColor = new Color(0.08f, 0.02f, 0.01f);
    public Color hotRed = new Color(0.75f, 0.08f, 0.0f);
    public Color hotOrange = new Color(1.0f, 0.42f, 0.0f);
    public Color hotYellow = new Color(1.0f, 0.85f, 0.25f);
    [ColorUsage(false, true)] public Color glowColor = new Color(3.5f, 1.1f, 0.15f);
    [Range(0f, 1f)] public float crustAmount = 0.45f;
    public int seed = 7;

    [Header("Animation")]
    public Vector2 flowSpeed = new Vector2(0.015f, 0.008f);
    public float glowPulseSpeed = 1.2f;
    [Range(0f, 1f)] public float glowPulseAmount = 0.3f;
    public float waveHeight = 0.12f;
    public float waveSpeed = 0.8f;
    public float waveScale = 0.25f;

    [Header("Extras")]
    public bool addLight = true;
    public bool addEmbers = true;

    [Header("Death")]
    public string playerTag = "Player";
    [Tooltip("Hook your existing death function here (e.g. PlayerHealth.Die).")]
    public UnityEvent<GameObject> onPlayerTouch;
    [Tooltip("Also calls a method named 'Die' on the player, if one exists.")]
    public bool sendDieMessage = true;

    Mesh mesh;
    Vector3[] baseVerts, verts;
    Material mat;
    Light lavaLight;
    float lightBase;

    void Awake()
    {
        BuildMesh();
        BuildMaterial();
        BuildTrigger();
        if (addLight) BuildLight();
        if (addEmbers) BuildEmbers();
    }

    // ---------------------------------------------------------------- mesh
    void BuildMesh()
    {
        int r = meshResolution;
        mesh = new Mesh { name = "LavaMesh" };
        mesh.indexFormat = IndexFormat.UInt32;
        mesh.MarkDynamic();

        baseVerts = new Vector3[(r + 1) * (r + 1)];
        var uvs = new Vector2[baseVerts.Length];
        var tris = new int[r * r * 6];

        for (int z = 0, i = 0; z <= r; z++)
            for (int x = 0; x <= r; x++, i++)
            {
                float u = (float)x / r, v = (float)z / r;
                baseVerts[i] = new Vector3((u - 0.5f) * size.x, 0f, (v - 0.5f) * size.y);
                uvs[i] = new Vector2(u, v);
            }

        for (int z = 0, t = 0; z < r; z++)
            for (int x = 0; x < r; x++)
            {
                int i = z * (r + 1) + x;
                tris[t++] = i; tris[t++] = i + r + 1; tris[t++] = i + 1;
                tris[t++] = i + 1; tris[t++] = i + r + 1; tris[t++] = i + r + 2;
            }

        verts = (Vector3[])baseVerts.Clone();
        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    // ------------------------------------------------------------ material
    void BuildMaterial()
    {
        BuildTextures(out Texture2D albedo, out Texture2D emission);

        if (baseMaterial != null)
        {
            mat = new Material(baseMaterial);
        }
        else
        {
            bool srp = GraphicsSettings.currentRenderPipeline != null;
            Shader sh = Shader.Find(srp ? "Universal Render Pipeline/Lit" : "Standard");
            if (sh == null) sh = Shader.Find("Standard");
            mat = new Material(sh);
        }

        mat.name = "LavaMaterial (runtime)";
        mat.mainTexture = albedo;                     // _BaseMap (URP) / _MainTex (Built-in)
        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", Color.white);
        if (mat.HasProperty("_Color")) mat.SetColor("_Color", Color.white);
        mat.SetTexture("_EmissionMap", emission);
        mat.SetColor("_EmissionColor", glowColor);
        mat.EnableKeyword("_EMISSION");
        mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", 0.55f);
        if (mat.HasProperty("_Glossiness")) mat.SetFloat("_Glossiness", 0.55f);
        if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", 0f);
        mat.mainTextureScale = new Vector2(tiling, tiling * size.y / Mathf.Max(0.01f, size.x));

        var mr = GetComponent<MeshRenderer>();
        mr.sharedMaterial = mat;
        mr.shadowCastingMode = ShadowCastingMode.Off;
    }

    void BuildTextures(out Texture2D albedo, out Texture2D emission)
    {
        int n = textureSize;
        float[] baseN = new float[n * n];
        float[] veins = new float[n * n];
        var rng = new System.Random(seed);
        Vector2 o1 = new Vector2(rng.Next(0, 10000), rng.Next(0, 10000));
        Vector2 o2 = new Vector2(rng.Next(0, 10000), rng.Next(0, 10000));
        Vector2 o3 = new Vector2(rng.Next(0, 10000), rng.Next(0, 10000));
        const float W = 6f; // noise-space period, makes the texture tile seamlessly

        for (int y = 0; y < n; y++)
            for (int x = 0; x < n; x++)
            {
                float px = (float)x / n * W, py = (float)y / n * W;
                // domain warp gives a swirly, "flowed" look
                float warp = TileNoise(px, py, W, o3, 2) * 1.2f;
                baseN[y * n + x] = TileNoise(px + warp, py + warp, W, o1, 5);
                veins[y * n + x] = TileNoise(px * 1.4f + warp, py * 1.4f - warp, W * 1.4f, o2, 4);
            }

        Normalize(baseN);
        Normalize(veins);

        var cA = new Color[n * n];
        var cE = new Color[n * n];
        for (int i = 0; i < cA.Length; i++)
        {
            float ridge = 1f - Mathf.Abs(veins[i] * 2f - 1f);   // bright cracks
            ridge = Mathf.Pow(ridge, 7f);
            float heat = Mathf.Clamp01((baseN[i] - crustAmount) * 1.8f + ridge * 1.1f);
            Color c = Ramp(heat);
            cA[i] = c;
            float e = heat * heat;
            cE[i] = new Color(c.r * e, c.g * e, c.b * e, 1f);
        }

        albedo = MakeTex(n, cA, "LavaAlbedo");
        emission = MakeTex(n, cE, "LavaEmission");
    }

    Color Ramp(float h)
    {
        if (h < 0.35f) return Color.Lerp(crustColor * 0.6f, crustColor, h / 0.35f);
        if (h < 0.55f) return Color.Lerp(crustColor, hotRed, (h - 0.35f) / 0.2f);
        if (h < 0.8f) return Color.Lerp(hotRed, hotOrange, (h - 0.55f) / 0.25f);
        return Color.Lerp(hotOrange, hotYellow, (h - 0.8f) / 0.2f);
    }

    static Texture2D MakeTex(int n, Color[] px, string name)
    {
        var t = new Texture2D(n, n, TextureFormat.RGBA32, true)
        {
            name = name,
            wrapMode = TextureWrapMode.Repeat,
            filterMode = FilterMode.Trilinear,
            anisoLevel = 4
        };
        t.SetPixels(px);
        t.Apply(true);
        return t;
    }

    // tileable fractal noise: blends 4 shifted samples so edges match
    static float TileNoise(float x, float y, float w, Vector2 off, int octaves)
    {
        x = Mod(x, w); y = Mod(y, w);
        float u = x / w, v = y / w;
        float a = Fbm(x, y, off, octaves), b = Fbm(x - w, y, off, octaves);
        float c = Fbm(x, y - w, off, octaves), d = Fbm(x - w, y - w, off, octaves);
        return Mathf.Lerp(Mathf.Lerp(a, b, u), Mathf.Lerp(c, d, u), v);
    }

    static float Fbm(float x, float y, Vector2 off, int octaves)
    {
        float sum = 0f, amp = 0.5f, f = 1f;
        for (int i = 0; i < octaves; i++)
        {
            sum += Mathf.PerlinNoise(x * f + off.x + 100f, y * f + off.y + 100f) * amp;
            f *= 2f; amp *= 0.5f;
        }
        return sum;
    }

    static float Mod(float a, float m) => a - m * Mathf.Floor(a / m);

    static void Normalize(float[] arr)
    {
        float min = float.MaxValue, max = float.MinValue;
        foreach (var v in arr) { if (v < min) min = v; if (v > max) max = v; }
        float r = Mathf.Max(1e-5f, max - min);
        for (int i = 0; i < arr.Length; i++) arr[i] = (arr[i] - min) / r;
    }

    // ------------------------------------------------------------- trigger
    void BuildTrigger()
    {
        var box = gameObject.AddComponent<BoxCollider>();
        box.isTrigger = true;
        box.center = new Vector3(0f, -0.5f, 0f);
        box.size = new Vector3(size.x, 1.5f, size.y);
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject go = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;
        if (!go.CompareTag(playerTag) && !other.CompareTag(playerTag)) return;

        onPlayerTouch?.Invoke(go);
        if (sendDieMessage) go.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
    }

    // ------------------------------------------------------------- extras
    void BuildLight()
    {
        var lgo = new GameObject("LavaLight");
        lgo.transform.SetParent(transform, false);
        lgo.transform.localPosition = new Vector3(0f, 2f, 0f);
        lavaLight = lgo.AddComponent<Light>();
        lavaLight.type = LightType.Point;
        lavaLight.color = new Color(1f, 0.45f, 0.1f);
        lavaLight.range = Mathf.Max(size.x, size.y) * 0.75f;
        lavaLight.intensity = 3f;
        lavaLight.shadows = LightShadows.None;
        lightBase = lavaLight.intensity;
    }

    void BuildEmbers()
    {
        var ego = new GameObject("LavaEmbers");
        ego.transform.SetParent(transform, false);
        var ps = ego.AddComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = ps.main;
        main.loop = true;
        main.startLifetime = new ParticleSystem.MinMaxCurve(1.5f, 3.5f);
        main.startSpeed = 0f;
        main.startSize = new ParticleSystem.MinMaxCurve(0.04f, 0.12f);
        main.startColor = new ParticleSystem.MinMaxGradient(new Color(1f, 0.35f, 0f), new Color(1f, 0.8f, 0.2f));
        main.maxParticles = 400;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var em = ps.emission;
        em.rateOverTime = Mathf.Clamp(size.x * size.y * 0.03f, 5f, 80f);

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(size.x, 0.1f, size.y);

        var vel = ps.velocityOverLifetime;
        vel.enabled = true;
        vel.space = ParticleSystemSimulationSpace.World;
        vel.x = new ParticleSystem.MinMaxCurve(-0.3f, 0.3f);
        vel.y = new ParticleSystem.MinMaxCurve(0.6f, 2.0f);
        vel.z = new ParticleSystem.MinMaxCurve(-0.3f, 0.3f);

        var col = ps.colorOverLifetime;
        col.enabled = true;
        var g = new Gradient();
        g.SetKeys(
            new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(new Color(1f, 0.3f, 0.1f), 1f) },
            new[] { new GradientAlphaKey(0f, 0f), new GradientAlphaKey(1f, 0.15f), new GradientAlphaKey(0f, 1f) });
        col.color = g;

        var sol = ps.sizeOverLifetime;
        sol.enabled = true;
        sol.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0.2f));

        var pr = ego.GetComponent<ParticleSystemRenderer>();
        var sh = Shader.Find("Sprites/Default");
        if (sh != null) pr.sharedMaterial = new Material(sh);

        ps.Play();
    }

    // ------------------------------------------------------------- animate
    void Update()
    {
        float t = Time.time;

        // flowing texture (with a slow wobble so it doesn't look like a conveyor belt)
        mat.mainTextureOffset = flowSpeed * t +
            new Vector2(Mathf.Sin(t * 0.21f), Mathf.Cos(t * 0.17f)) * 0.02f;

        // pulsing glow
        float pulse = 1f + Mathf.Sin(t * glowPulseSpeed) * glowPulseAmount * 0.6f
                         + (Mathf.PerlinNoise(t * 0.7f, 3.3f) - 0.5f) * glowPulseAmount;
        mat.SetColor("_EmissionColor", glowColor * pulse);

        // light flicker
        if (lavaLight)
            lavaLight.intensity = lightBase * (0.8f + Mathf.PerlinNoise(t * 2.5f, 0f) * 0.4f) * pulse;

        // slow bubbling waves
        if (waveHeight > 0f)
        {
            float ts = t * waveSpeed;
            for (int i = 0; i < verts.Length; i++)
            {
                Vector3 p = baseVerts[i];
                float h = Mathf.Sin(p.x * waveScale + ts) * 0.5f
                        + Mathf.Sin(p.z * waveScale * 1.3f + ts * 0.8f) * 0.3f
                        + (Mathf.PerlinNoise(p.x * waveScale * 0.5f + ts * 0.3f, p.z * waveScale * 0.5f) - 0.5f);
                verts[i].y = h * waveHeight;
            }
            mesh.vertices = verts;
            mesh.RecalculateNormals();
        }
    }

    void OnDestroy()
    {
        if (mat) Destroy(mat);
        if (mesh) Destroy(mesh);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.4f, 0f, 0.6f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(size.x, 0.2f, size.y));
    }
#endif
}