using System.Linq;
using RSG;
using UnityEngine;

namespace hLevels
{
    [CreateAssetMenu(fileName = "Additive Scene", menuName = "hLevels/AdditiveScene")]
    public class AdditiveScene : ScriptableObject
    {
        public bool DebugMeOnLoad;
        public Object[] SceneLayers;

        public IPromise Load() => HLoad.LoadAdditiveScenesFromList(
            SceneLayers.Select(x=>x.ToString()
                .Replace(" (UnityEngine.SceneAsset)", string.Empty))
                .ToArray(), 
            DebugMeOnLoad);
    }
}