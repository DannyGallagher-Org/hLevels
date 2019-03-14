using RSG;
using UnityEngine;

namespace hLevelsRuntime
{
    [CreateAssetMenu(fileName = "Additive Scene", menuName = "hLevels/AdditiveScene")]
    public class AdditiveScene : ScriptableObject
    {
        public string[] SceneLayers;

        public void Load() => HLoad.LoadAdditiveScenesFromList(SceneLayers);
        public IPromise PromiseLoad => HLoad.PromiseLoadAdditiveScenesFromList(SceneLayers);
    }
}