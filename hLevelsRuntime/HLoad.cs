using System.Collections;
using RSG;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace hLevelsRuntime
{
    public static class HLoad
    {
        public static void LoadAdditiveScenesFromList(string[] levelList)
        {
            var asyncLoadList = new IPromise[levelList.Length];

            for (int index = 0; index < levelList.Length; index++)
                asyncLoadList[index] = PromiseLoadScene(levelList[index], LoadSceneMode.Additive);
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
    }
}