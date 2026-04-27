using UnityEngine;

[DefaultExecutionOrder(-9999)]
public class Bootstrapper : MonoBehaviour
{
    private static GameObject _serviceHolder;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnBeforeSceneLoad()
    {
        _serviceHolder = new GameObject("---Services---");
        Object.DontDestroyOnLoad(_serviceHolder);
        
        R.InitAudio();
        
        G.ResourceManager = CreateSimpleService<ResourceManager>();
        G.TrialSystem = CreateSimpleService<TrialSystem>();
        G.CurseManager = CreateSimpleService<CurseManager>();
        G.AudioManager = CreateSimpleService<AudioManager>();
        // G.TooltipManager = CreateSimpleService<TooltipManager>();
    }

    private static T CreateSimpleService<T>() where T : Component, IService
    {
        GameObject gameObject = new GameObject(typeof(T).ToString())
            { transform = { parent = _serviceHolder.transform } };

        T component = gameObject.AddComponent<T>();
        component.Init();
        return gameObject.GetComponent<T>();
    }
}