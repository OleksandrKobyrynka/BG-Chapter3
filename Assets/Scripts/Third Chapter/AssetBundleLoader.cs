using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetBundleLoader : MonoBehaviour
{
    [Header("AssetBundle")]
    [SerializeField] private string _bundleName = "enemies";

    [SerializeField]
    private string[] _enemyAssetNames =
    {
        "Enemy1_Prefab",
        "Enemy2_Prefab",
        "Enemy3_Prefab"
    };

    [Header("Spawn")]
    [SerializeField] private float _spawnSpacing = 2f;

    private AssetBundle _loadedBundle;
    private readonly List<AssetBundle> _loadedDependencies = new();

    private readonly Dictionary<string, GameObject> _enemyPrefabs = new();

    private string _assetBundleDirectory;
    private bool _assetsLoaded;

    private readonly List<GameObject> _spawnedEnemies = new();

    private void Start()
    {
        _assetBundleDirectory = Path.Combine(Application.streamingAssetsPath, "AssetBundles");

        StartCoroutine(LoadBundlesAndAssets());
    }

    private IEnumerator LoadBundlesAndAssets()
    {
        yield return LoadManifestAndDependencies();
        yield return LoadMainBundle();

        if (_loadedBundle == null)
        {
            yield break;
        }

        yield return LoadEnemyPrefabs();

        if (_enemyPrefabs.Count == 0)
        {
            Debug.Log("No enemy prefabs were loaded.");
            yield break;
        }

        _assetsLoaded = true;

        UnloadMainBundleButKeepAssets();

        Debug.Log($"Loaded {_enemyPrefabs.Count} enemy prefabs. AssetBundles can now be unloaded.");
    }

    private IEnumerator LoadManifestAndDependencies()
    {
        string manifestPath = Path.Combine(_assetBundleDirectory, "AssetBundles");

        if (!File.Exists(manifestPath))
        {
            Debug.Log($"Manifest bundle not found: {manifestPath}");
            yield break;
        }

        AssetBundleCreateRequest manifestRequest = AssetBundle.LoadFromFileAsync(manifestPath);

        yield return manifestRequest;

        AssetBundle manifestBundle = manifestRequest.assetBundle;

        if (manifestBundle == null)
        {
            Debug.Log("Failed to load manifest bundle.");
            yield break;
        }

        AssetBundleRequest manifestAssetRequest = manifestBundle.LoadAssetAsync<AssetBundleManifest>("AssetBundleManifest");

        yield return manifestAssetRequest;

        AssetBundleManifest manifest = manifestAssetRequest.asset as AssetBundleManifest;

        if (manifest == null)
        {
            Debug.Log("Failed to load AssetBundleManifest.");
            manifestBundle.Unload(false);
            yield break;
        }

        string[] dependencies = manifest.GetAllDependencies(_bundleName);

        foreach (string dependencyName in dependencies)
        {
            yield return LoadDependency(dependencyName);
        }

        manifestBundle.Unload(false);

        Debug.Log($"Loaded {dependencies.Length} dependencies for '{_bundleName}'.");
    }

    private IEnumerator LoadDependency(string dependencyName)
    {
        string dependencyPath = Path.Combine(_assetBundleDirectory, dependencyName);

        if (!File.Exists(dependencyPath))
        {
            Debug.Log($"Dependency bundle not found: {dependencyPath}");

            yield break;
        }

        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(dependencyPath);

        yield return request;

        AssetBundle dependencyBundle = request.assetBundle;

        if (dependencyBundle == null)
        {
            Debug.Log($"Failed to load dependency: {dependencyName}");

            yield break;
        }

        _loadedDependencies.Add(dependencyBundle);

        Debug.Log($"Loaded dependency: {dependencyName}");
    }

    private IEnumerator LoadMainBundle()
    {
        string bundlePath = Path.Combine(_assetBundleDirectory, _bundleName);

        if (!File.Exists(bundlePath))
        {
            Debug.Log($"Main bundle not found: {bundlePath}");
            yield break;
        }

        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(bundlePath);

        yield return request;

        _loadedBundle = request.assetBundle;

        if (_loadedBundle == null)
        {
            Debug.Log($"Failed to load bundle: {_bundleName}");
            yield break;
        }

        Debug.Log($"Loaded main bundle: {_bundleName}");
    }

    private IEnumerator LoadEnemyPrefabs()
    {
        foreach (string assetName in _enemyAssetNames)
        {
            AssetBundleRequest request = _loadedBundle.LoadAssetAsync<GameObject>(assetName);

            yield return request;

            GameObject prefab = request.asset as GameObject;

            if (prefab == null)
            {
                Debug.Log($"Asset '{assetName}' not found in bundle '{_bundleName}'.");

                continue;
            }

            _enemyPrefabs[assetName] = prefab;

            Debug.Log($"Loaded prefab asset: {assetName}");
        }
    }

    [ContextMenu("Spawn All Enemies")]
    public void SpawnAllEnemies()
    {
        if (!_assetsLoaded)
        {
            Debug.Log("Assets are not loaded yet.");
            return;
        }

        int index = 0;

        foreach (GameObject prefab in _enemyPrefabs.Values)
        {
            Vector3 position = new Vector3(index * _spawnSpacing, 0f, 0f);

            GameObject instance = Instantiate(prefab, position, Quaternion.identity);

            _spawnedEnemies.Add(instance);

            index++;
        }

        Debug.Log($"Spawned {index} enemies.");
    }

    [ContextMenu("Destroy Spawned Enemies")]
    public void DestroySpawnedEnemies()
    {
        foreach (GameObject enemy in _spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        _spawnedEnemies.Clear();
    }

    private void UnloadMainBundleButKeepAssets()
    {
        if (_loadedBundle != null)
        {
            _loadedBundle.Unload(false);
            _loadedBundle = null;
        }

        Debug.Log("AssetBundles unloaded. Loaded prefab assets remain available.");
    }

    private void OnDestroy()
    {
        StopAllCoroutines();

        DestroySpawnedEnemies();

        _enemyPrefabs.Clear();

        foreach (AssetBundle dependency in _loadedDependencies)
        {
            if (dependency != null)
            {
                dependency.Unload(false);
            }
        }

        _loadedDependencies.Clear();
    }
}