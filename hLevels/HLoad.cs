using System.Collections;
using System.Diagnostics;
using RSG;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace hLevels
{
    public static class HLoad
    {
        public static IPromise LoadAdditiveScenesFromList(string[] levelList, bool debugMeOnLoad)
        {
            var promise = new Promise();
            var asyncLoadList = new IPromise[levelList.Length];

            for (var index = 0; index < levelList.Length; index++)
                asyncLoadList[index] = PromiseLoadScene(levelList[index], LoadSceneMode.Additive, debugMeOnLoad);

            Promise.All(asyncLoadList)
                .Then(() =>
                {
                    Debug.Log("Scenes are all loaded.");
                })
                .Finally(() => promise.Resolve());

            return promise;
        }

        private static IPromise PromiseLoadScene(string sceneName, LoadSceneMode loadMode, bool debugMeOnLoad)
        {
            var promise = new Promise();
            
            if(debugMeOnLoad)
                Debug.Log($"<color=white>⬤</color>  StartLoad <color=orange>{sceneName}</color>");
            
            StaticCoroutinableObject.StartACoroutine(SceneManager.LoadSceneAsync(sceneName, loadMode).Await(promise, sceneName, debugMeOnLoad));
            return promise;
        }

        private static IEnumerator Await(this AsyncOperation operation, IPendingPromise promise, string sceneName, bool debugMeOnLoad)
        {
            var timer = new Stopwatch();
            timer.Start();
            
            while(!operation.isDone)
                yield return operation;

            if (debugMeOnLoad)
            {
                timer.Stop();
                Debug.Log($"<color=green>⬤</color> Load <color=orange>{sceneName}</color> took: <color=white>{timer.Elapsed.Milliseconds}</color>ms");
            }
            
            promise.Resolve();
        }
    }
}