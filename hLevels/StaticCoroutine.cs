using System;
using System.Collections;
using JetBrains.Annotations;
using RSG;
using UnityEngine;
using Object = UnityEngine.Object;

namespace hLevels
{
    public class StaticCoroutinableObject : MonoBehaviour
    {
        private static StaticCoroutinableObject _coroutinableObject;

        public static StaticCoroutinableObject CoroutinableObject
        {
            get
            {
                if (_coroutinableObject)
                    return _coroutinableObject;

                var go = new GameObject("CoroutineableObject");
                DontDestroyOnLoad(go);
                _coroutinableObject = go.AddComponent<StaticCoroutinableObject>();
                return _coroutinableObject;
            }
        }

        public static void StartACoroutine(IEnumerator waitForSceneLoad)
        {
            CoroutinableObject.StartCoroutine(waitForSceneLoad);
        }

        public static IPromise<Object> WaitForSecondsAsPromise(float f)
        {
            var p = new Promise<Object>();
            StartACoroutine(CoroutinableObject.WaitForSeconds(f, p));
            return p;
        }

        public static IPromise<Object> WaitForEndOfFrameAsPromise(int count)
        {
            var p = new Promise<Object>();
            StartACoroutine(CoroutinableObject.WaitForEndOfFrameAsPromise(p, count));
            return p;
        }

        public static IPromise<Object> YieldAsPromise()
        {
            var p = new Promise<Object>();
            StartACoroutine(CoroutinableObject.YieldAsPromise(p));
            return p;
        }

        public static IPromise<Object> WaitUntilAsPromise(Func<bool> evaluator)
        {
            var p = new Promise<Object>();
            StartACoroutine(WaitUntilAsPromise(evaluator, p));
            return p;
        }

        public static IPromise<Object> WaitForSecondsWhileAsPromise(float f, Func<bool> evaluator)
        {
            var p = new Promise<Object>();
            StartACoroutine(CoroutinableObject.WaitForSecondsWhile(f, evaluator, p));
            return p;
        }

        public static IPromise<Object> TweenAsPromise(float time, Action<float> onUpdate)
        {
            var p = new Promise<Object>();
            StartACoroutine(CoroutinableObject.TweenAsPromise(time, onUpdate, p));
            return p;
        }

        private IEnumerator WaitForSeconds(float f, [NotNull] Promise<Object> p)
        {
            if (p == null) throw new ArgumentNullException(p.Name);
            yield return new WaitForSeconds(f);
            p.Resolve(null);
        }

        private IEnumerator YieldAsPromise([NotNull] Promise<Object> p)
        {
            if (p == null) throw new ArgumentNullException(p.Name);
            yield return null;
            p.Resolve(null);
        }

        private static IEnumerator WaitUntilAsPromise(Func<bool> evaluator, IPendingPromise<Object> p)
        {
            yield return new WaitUntil(evaluator);
            p.Resolve(null);
        }

        private IEnumerator WaitForSecondsWhile(float f, Func<bool> evaluator, [NotNull] Promise<Object> p)
        {
            if (p == null) throw new ArgumentNullException(p.Name);
            var time = 0f;

            while (time < f)
            {
                if (evaluator())
                    time += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            p.Resolve(null);
        }

        private IEnumerator TweenAsPromise(float duration, Action<float> onUpdate, [NotNull] Promise<Object> p)
        {
            if (p == null) throw new ArgumentNullException(p.Name);
            var time = 0f;

            while (time < duration)
            {
                onUpdate(time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            onUpdate(1f);
            p.Resolve(null);
        }

        private IEnumerator WaitForEndOfFrameAsPromise([NotNull] Promise<Object> p, int count)
        {
            if (p == null) throw new ArgumentNullException(p.Name);
            for (var i = 0; i < count; i++) yield return new WaitForEndOfFrame();
            p.Resolve(null);
        }
    }
}