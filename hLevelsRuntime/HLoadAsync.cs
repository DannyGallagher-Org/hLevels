using System.Threading.Tasks;
using RSG;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace hLevelsRuntime
{
    public static class HLoadAsync
    {
        public static IPromise LoadAdditiveScenesFromListAsync(string[] levelList)
        {
            var promise = new Promise();
            var asyncLoadList = new IPromise[levelList.Length];

            for (var index = 0; index < levelList.Length; index++)
                asyncLoadList[index] = PromiseLoadSceneAsync(levelList[index], LoadSceneMode.Additive);
            
            Promise.All(asyncLoadList)
                .Then(() =>
                {
                    Debug.Log("Scenes are all loaded.");
                })
                .Finally(() => promise.Resolve());

            return promise;
        }
        
        private static IPromise PromiseLoadSceneAsync(string sceneName, LoadSceneMode loadMode)
        {
            var promise = new Promise();
            LoadSceneAsync(sceneName, loadMode, promise);
            return promise;
        }

        private static async void LoadSceneAsync(string sceneName, LoadSceneMode loadMode, Promise promise)
        {
            await SceneLoadTask(sceneName, loadMode);
            promise.Resolve();
        }

        private static Task SceneLoadTask(string sceneName, LoadSceneMode loadMode)
        {
            return Task.Run(() => SceneManager.LoadSceneAsync(sceneName, loadMode));
        }
    }
}