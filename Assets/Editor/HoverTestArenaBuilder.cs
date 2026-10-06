using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// One-click test arena builder for the Hover project.
//
// Builds a large, open ground plane with scattered ramps, barriers, and pillars
// from the KayKit Platformer Pack (free tier) — sized relative to your actual
// Hover craft, not a fixed guess. The previous version used KayKit's own floor
// tiles at their native (very small, human-platformer-scale) size, which is why
// the craft dwarfed a single tile. This version measures your craft first and
// scales everything else around it.
//
// HOW TO USE:
//   1. Manually delete any existing Synty-sourced objects from the Hierarchy
//      first (select them, press Delete). This script only adds new objects —
//      it never touches or deletes anything except a previous arena it built
//      itself, so removing the old environment is a separate, deliberate step.
//   2. In Unity: Tools > Build Hover Test Arena.
//   3. A "Hover Test Arena" GameObject appears, with a large ground plane plus
//      scattered ramps/barriers/pillars already sized to your craft. Drag your
//      Hover craft into the scene above the ground and press Play.
//   4. Re-running the menu command deletes the previous arena first and
//      rebuilds, so it's safe to click again after tweaking the constants below.
//   5. If a ramp/barrier/pillar's shape isn't what you want, swap the piece
//      file name constants below for a different file from the same KayKit
//      folder (open it in the Project window to see what else is available).
public static class HoverTestArenaBuilder
{
    // Folder the KayKit Platformer Pack (free tier) imports into by default.
    private const string KitFolder = "Assets/KayKit_Platformer_Pack_1.0_FREE/Assets/fbx(unity)/neutral/";
    private const string RampPieceName = "structure_A.fbx";
    private const string BarrierPieceName = "barrier_2x1x1.fbx";
    private const string PillarPieceName = "pillar_2x2x8.fbx";

    // Path to your Hover craft prefab — used to measure its size so everything
    // else in the arena scales relative to it, instead of a guessed constant.
    private const string HoverCraftPath = "Assets/Prefab/Seaspray.prefab";

    // How many craft-lengths wide the open ground should be. Raise this for an
    // even bigger field.
    private const float GroundSpanInCraftLengths = 25f;

    // How large each prop type should read relative to the craft's length.
    // A ramp bigger than the barriers/pillars reads as a slope to drive up,
    // not just more clutter.
    private const float RampSizeInCraftLengths = 2.5f;
    private const float BarrierSizeInCraftLengths = 0.6f;
    private const float PillarSizeInCraftLengths = 1.8f;

    private const int RampCount = 6;
    private const int BarrierCount = 10;
    private const int PillarCount = 4;

    [MenuItem("Tools/Build Hover Test Arena")]
    public static void BuildArena()
    {
        // Remove a previously-built arena so repeated runs don't stack up.
        GameObject existing = GameObject.Find("Hover Test Arena");
        if (existing != null)
        {
            Undo.DestroyObjectImmediate(existing);
        }

        GameObject craftPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(HoverCraftPath);
        if (craftPrefab == null)
        {
            Debug.LogError($"Hover Test Arena Builder: couldn't find the Hover craft prefab at \"{HoverCraftPath}\". Update HoverCraftPath at the top of HoverTestArenaBuilder.cs to match its actual path, then run this again.");
            return;
        }
        float craftLength = MeasureLongestSide(craftPrefab);

        GameObject rampPrefab = LoadPiece(RampPieceName);
        GameObject barrierPrefab = LoadPiece(BarrierPieceName);
        GameObject pillarPrefab = LoadPiece(PillarPieceName);

        float rampScale = rampPrefab != null ? (craftLength * RampSizeInCraftLengths) / MeasureLongestSide(rampPrefab) : 1f;
        float barrierScale = barrierPrefab != null ? (craftLength * BarrierSizeInCraftLengths) / MeasureLongestSide(barrierPrefab) : 1f;
        float pillarScale = pillarPrefab != null ? (craftLength * PillarSizeInCraftLengths) / MeasureLongestSide(pillarPrefab) : 1f;

        float groundSpan = craftLength * GroundSpanInCraftLengths;

        GameObject root = new GameObject("Hover Test Arena");
        Undo.RegisterCreatedObjectUndo(root, "Build Hover Test Arena");

        BuildGround(root.transform, groundSpan);

        // Deterministic pseudo-random layout — same seed every run, so the
        // arena is reproducible while you're tuning the constants above.
        Random.State previousState = Random.state;
        Random.InitState(12345);

        ScatterProps(rampPrefab, root.transform, "Ramp", RampCount, groundSpan, rampScale, randomizeYRotation: true);
        ScatterProps(barrierPrefab, root.transform, "Barrier", BarrierCount, groundSpan, barrierScale, randomizeYRotation: true);
        ScatterProps(pillarPrefab, root.transform, "Pillar", PillarCount, groundSpan, pillarScale, randomizeYRotation: false);

        Random.state = previousState;

        Selection.activeGameObject = root;
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

        Debug.Log($"Hover Test Arena built: craft measured at {craftLength:F1}m, ground span {groundSpan:F0}m x {groundSpan:F0}m, " +
                  $"ramp scale {rampScale:F2}x, barrier scale {barrierScale:F2}x, pillar scale {pillarScale:F2}x. " +
                  "Drag your Hover craft into the scene above the ground and press Play. Remember to save the scene (Ctrl/Cmd+S).");
    }

    private static void BuildGround(Transform parent, float span)
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.SetParent(parent);
        // Unity's default Plane mesh is 10x10 units at scale 1 — divide the
        // target span by 10 to get the scale that produces it.
        float scale = span / 10f;
        ground.transform.localScale = new Vector3(scale, 1f, scale);
        ground.transform.localPosition = Vector3.zero;

        // Primitives default to a Built-in-pipeline material, which renders
        // pink/magenta under URP — assign a URP-compatible material instead.
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            shader = Shader.Find("Standard"); // fallback if this project isn't URP after all
        }
        Material groundMaterial = new Material(shader) { color = new Color(0.55f, 0.72f, 0.45f) };
        ground.GetComponent<Renderer>().sharedMaterial = groundMaterial;

        Undo.RegisterCreatedObjectUndo(ground, "Build Hover Test Arena");
    }

    private static void ScatterProps(GameObject prefab, Transform parent, string baseName, int count, float groundSpan, float scale, bool randomizeYRotation)
    {
        if (prefab == null)
        {
            return; // LoadPiece already logged why.
        }

        // Keep props a little inside the ground's edge rather than right at it.
        float margin = groundSpan * 0.1f;
        float usableSpan = groundSpan - margin * 2f;

        for (int i = 0; i < count; i++)
        {
            float x = -groundSpan / 2f + margin + Random.value * usableSpan;
            float z = -groundSpan / 2f + margin + Random.value * usableSpan;
            Quaternion rotation = randomizeYRotation
                ? Quaternion.Euler(0f, Random.Range(0, 4) * 90f, 0f)
                : Quaternion.identity;

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
            instance.transform.localPosition = new Vector3(x, 0f, z);
            instance.transform.localRotation = rotation;
            instance.transform.localScale = Vector3.one * scale;
            instance.name = $"{baseName}_{i}";
            AddMeshColliders(instance);
            Undo.RegisterCreatedObjectUndo(instance, "Build Hover Test Arena");
        }
    }

    // KayKit's FBX files import with no colliders at all (Unity's Model
    // Importer has "Generate Colliders" off by default) — without this, the
    // hover raycast and the craft's Rigidbody pass straight through every
    // placed piece, which is why nothing in the scene was interactive. Add a
    // MeshCollider to every mesh in the piece's hierarchy. A non-convex
    // MeshCollider is fine here because these are static scenery with no
    // Rigidbody of their own — it also means the ramp's actual sloped shape
    // is collidable, not just a bounding box, so it genuinely works as a ramp.
    private static void AddMeshColliders(GameObject go)
    {
        foreach (MeshFilter meshFilter in go.GetComponentsInChildren<MeshFilter>())
        {
            GameObject target = meshFilter.gameObject;
            if (target.GetComponent<Collider>() == null && meshFilter.sharedMesh != null)
            {
                MeshCollider collider = target.AddComponent<MeshCollider>();
                collider.sharedMesh = meshFilter.sharedMesh;
            }
        }
    }

    private static float MeasureLongestSide(GameObject prefab)
    {
        GameObject temp = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        Bounds bounds = CalculateBounds(temp);
        Object.DestroyImmediate(temp);
        return Mathf.Max(bounds.size.x, bounds.size.z);
    }

    private static Bounds CalculateBounds(GameObject go)
    {
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            return new Bounds(go.transform.position, Vector3.one);
        }
        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }
        return bounds;
    }

    private static GameObject LoadPiece(string fbxFileName)
    {
        string path = KitFolder + fbxFileName;
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (prefab == null)
        {
            Debug.LogError($"Hover Test Arena Builder: couldn't find \"{path}\". Confirm the KayKit Platformer Pack is imported at this path, or update the piece name constants at the top of HoverTestArenaBuilder.cs to match.");
        }
        return prefab;
    }
}
