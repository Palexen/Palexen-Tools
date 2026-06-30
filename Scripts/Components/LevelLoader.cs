/*
* -----------------------------------------------------------------------------
* Palexen Tools
* © Palexen | Xeen Render & Devward. All rights reserved.
* https://www.palexen.com/

* -----------------------------------------------------------------------------

* Developed by: Palexen & Xeen Render

* Written by: Devward

* This software is provided "as is," without warranties of any kind.

* Use of this script is subject to the terms of the Palexen Tools and other derivative products license.

* Commercial redistribution or redistribution to third parties without authorization is prohibited.

* -----------------------------------------------------------------------------
*/
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

#if PALEXEN_TOOLS
using Palexen.Tools;
#endif

namespace Palexen.Levels
{
#if PALEXEN_TOOLS
    [ScriptDescription("Level Loader", "Load scenes via the loading scene, or from here")]
#endif
    [AddComponentMenu("Palexen/Levels/Level Loader")]
    public class LevelLoader : MonoBehaviour
    {
        #region VARIABLES

        [MyHeader("Scene Behaviour")]
        [SerializeField] private LevelLoadMode _loadMode;
        [SerializeField] private LoadSceneMode _loadSceneMode;
        [SerializeField] private string loadingSceneName = "Loading";
        [SerializeField] private float _delayTimer = 5f;
        [SerializeField] private float _delayScreen = 2f;

        [MyHeader("UI")]
        public LoadingBarMode _loadingBar;
        [FieldColor(FieldPropertyColor.salmon, ShowObjectMessage.errorMessage)][SerializeField] private Slider _slider;
        [FieldColor(FieldPropertyColor.salmon, ShowObjectMessage.errorMessage)][SerializeField] private Image _imageToFill;

        [MyHeader("Misc")]
        [FieldColor(FieldPropertyColor.pink)][SerializeField] private GameObject _fadeScreen;
        [SerializeField] private UnityEvent _eventsAfterFinish;

        [SerializeField] private bool _useRootActivation = false;
        bool isSliderOperation = false;
        string loadingSceneTarget;

        #endregion

        #region PROPERTIES

        public LevelLoadMode LoadMode { get { return _loadMode; } }
        public GameObject FadeScreen { get { return _fadeScreen; } }

        #endregion

        #region UNITY METHODS

        void Start()
        {
            if (_loadMode == LevelLoadMode.catchAndLoad)
            {
                LevelManager.instance.Delay = _delayTimer;
                loadingSceneTarget = SceneManager.GetActiveScene().name;
                StartCoroutine(LoadLevelFromManager());
            }
        }

        void Update()
        {
            if (isSliderOperation)
            {
                _slider.value = Mathf.MoveTowards(_slider.value, LevelManager.instance.Delay, 1 * Time.deltaTime);
            }
        }

        #endregion

        #region API

        public void LoadLevel(string targetLevel)
        {
            LevelManager.instance.SetScene(targetLevel, _delayTimer, _useRootActivation);
            StartCoroutine(LoadFromLoadingScreen());
        }

        #endregion

        #region IENUMERATORS

        IEnumerator LoadFromLoadingScreen()
        {
            switch (_loadingBar)
            {
                case LoadingBarMode.none:
                    if (_fadeScreen != null)
                    {
                        _fadeScreen.SetActive(true);
                    }
                    yield return new WaitForSeconds(LevelManager.instance.Delay);
                    _eventsAfterFinish.Invoke();
                    AsyncOperation asyncNone = SceneManager.LoadSceneAsync(loadingSceneName, _loadSceneMode);

                    while (!asyncNone.isDone)
                    {
                        yield return null;
                    }

                    if (_loadSceneMode == LoadSceneMode.Additive)
                    {
                        FinalizeSceneTransition(loadingSceneName);
                    }
                    break;

                case LoadingBarMode.slider:
                    _slider.maxValue = LevelManager.instance.Delay + 1f;
                    isSliderOperation = true;

                    float delay = LevelManager.instance.Delay;
                    yield return new WaitForSeconds(delay);

                    isSliderOperation = false;

                    if (_slider.value >= delay)
                    {
                        AsyncOperation asyncSlider = SceneManager.LoadSceneAsync(loadingSceneName, _loadSceneMode);
                        while (!asyncSlider.isDone)
                        {
                            _slider.value = delay + asyncSlider.progress;
                            float progress = asyncSlider.progress;

                            if (progress >= .9f)
                            {
                                asyncSlider.allowSceneActivation = false;
                                if (_fadeScreen != null)
                                {
                                    _fadeScreen.SetActive(true);
                                }
                                _eventsAfterFinish.Invoke();
                                yield return new WaitForSeconds(_delayScreen);
                                asyncSlider.allowSceneActivation = true;
                            }


                            yield return null;
                        }

                        _slider.value = _slider.maxValue;

                        if (_loadSceneMode == LoadSceneMode.Additive)
                        {
                            FinalizeSceneTransition(loadingSceneName);
                        }
                    }
                    break;

                case LoadingBarMode.fill:

                    AsyncOperation asyncFill = SceneManager.LoadSceneAsync(loadingSceneName, _loadSceneMode);

                    isSliderOperation = false;

                    while (!asyncFill.isDone)
                    {
                        _imageToFill.fillAmount = asyncFill.progress;
                        float progress = asyncFill.progress;

                        if (progress >= .9f)
                        {
                            asyncFill.allowSceneActivation = false;
                            if (_fadeScreen != null)
                            {
                                _fadeScreen.SetActive(true);
                                _imageToFill.fillAmount = 1;
                            }
                            _eventsAfterFinish.Invoke();
                            yield return new WaitForSeconds(_delayScreen);
                            asyncFill.allowSceneActivation = true;
                        }


                        yield return null;
                    }

                    if (_loadSceneMode == LoadSceneMode.Additive)
                    {
                        FinalizeSceneTransition(loadingSceneName);
                    }
                    break;
            }
        }

        IEnumerator LoadLevelFromManager()
        {
            string targetSceneName = LevelManager.instance.SceneName;

            switch (_loadingBar)
            {
                case LoadingBarMode.none:
                    if (_fadeScreen != null)
                    {
                        _fadeScreen.SetActive(true);
                    }
                    yield return new WaitForSeconds(LevelManager.instance.Delay);
                    _eventsAfterFinish.Invoke();
                    AsyncOperation asyncNone = SceneManager.LoadSceneAsync(targetSceneName, _loadSceneMode);

                    while (!asyncNone.isDone)
                    {
                        yield return null;
                    }

                    if (_loadSceneMode == LoadSceneMode.Additive)
                    {
                        FinalizeSceneTransition(targetSceneName);
                    }
                    break;

                case LoadingBarMode.slider:
                    _slider.maxValue = LevelManager.instance.Delay + 1f;
                    isSliderOperation = true;

                    float delay = LevelManager.instance.Delay;
                    yield return new WaitForSeconds(delay);

                    isSliderOperation = false;

                    if (_slider.value >= delay)
                    {
                        AsyncOperation asyncSlider = SceneManager.LoadSceneAsync(targetSceneName, _loadSceneMode);
                        while (!asyncSlider.isDone)
                        {
                            _slider.value = delay + asyncSlider.progress;
                            float progress = asyncSlider.progress;

                            if(progress >= .9f)
                            {
                                asyncSlider.allowSceneActivation = false;
                                if(_fadeScreen != null)
                                {
                                    _fadeScreen.SetActive(true);
                                }
                                _eventsAfterFinish.Invoke();
                                yield return new WaitForSeconds(_delayScreen);
                                asyncSlider.allowSceneActivation = true;
                            }


                            yield return null;
                        }

                        _slider.value = _slider.maxValue;

                        if (_loadSceneMode == LoadSceneMode.Additive)
                        {
                            FinalizeSceneTransition(targetSceneName);
                        }
                    }
                    break;

                case LoadingBarMode.fill:

                    AsyncOperation asyncFill = SceneManager.LoadSceneAsync(targetSceneName, _loadSceneMode);

                    isSliderOperation = false;

                    while (!asyncFill.isDone)
                    {
                        _imageToFill.fillAmount = asyncFill.progress;
                        float progress = asyncFill.progress;

                        if (progress >= .9f)
                        {
                            asyncFill.allowSceneActivation = false;
                            if (_fadeScreen != null)
                            {
                                _fadeScreen.SetActive(true);
                                _imageToFill.fillAmount = 1;
                            }
                            _eventsAfterFinish.Invoke();
                            yield return new WaitForSeconds(_delayScreen);
                            asyncFill.allowSceneActivation = true;
                        }


                        yield return null;
                    }

                    if (_loadSceneMode == LoadSceneMode.Additive)
                    {
                        FinalizeSceneTransition(targetSceneName);
                    }
                    break;
            }
        }

        void FinalizeSceneTransition(string nextSceneName)
        {
            Scene nextScene = SceneManager.GetSceneByName(nextSceneName);

            if (nextScene.IsValid())
            {
                SceneManager.SetActiveScene(nextScene);

                if (LevelManager.instance.IsRootActivation)
                {
                    foreach (GameObject rootObj in nextScene.GetRootGameObjects())
                    {
                        rootObj.SetActive(true);
                        LightProbes.TetrahedralizeAsync();
                    }
                }
            }

            SceneManager.UnloadSceneAsync(loadingSceneTarget);
        }

        #endregion
    }
}
