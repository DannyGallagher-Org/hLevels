using System.Collections;
using RSG;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace hLevelsRuntime
{
    public static class HLoad
    {
        public static IPromise LoadAdditiveScenesFromList(string[] levelList)
        {
            var promise = new Promise();
            var asyncLoadList = new IPromise[levelList.Length];

            for (int index = 0; index < levelList.Length; index++)
                asyncLoadList[index] = PromiseLoadScene(levelList[index], LoadSceneMode.Additive);

            Promise.All(asyncLoadList)
                .Then(() =>
                {
                    Debug.Log("Scenes are all loaded.");
                })
                .Finally(() => promise.Resolve());

            return promise;
        }

        private static IPromise PromiseLoadScene(string sceneName, LoadSceneMode loadMode)
        {
            var promise = new Promise();
            StaticCoroutinableObject.StartACoroutine(SceneManager.LoadSceneAsync(sceneName, loadMode).Await(promise));
            return promise;
        }
        
        public static IEnumerator Await(this AsyncOperation operation, Promise promise) {
            while(!operation.isDone)
                yield return operation;
            
            promise.Resolve();
        }

        public static IPromise PromiseLoadAdditiveScenesFromList(string[] sceneLayers)
        {
            throw new System.NotImplementedException();
        }
    }
}