using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using HoloToolkit.Unity;
using DG.Tweening;


/// <summary>
/// Опции загрузки префабов
/// LoadThenIntantiate - загружает по очереди все префабы а затем все инстантиирует
/// LoadAndInstantiate - загружает по очереди и инстантиирует каждый префаб как только загрузил его
/// </summary>
public enum LoadMultiplePrefabsOptions { LoadThenIntantiate, LoadAndInstantiate }

/// <summary>
/// Опции загрузки сцен
/// Additive - предыдущая сцена остается
/// Single - предыдущая сцена удаляется (за исключением объектов, помеченных скриптом IUndestroyableOnLoad)
/// </summary>
public enum SceneLoadingMode { Additive, Single }

/// <summary>
/// Класс, помогающий в загрузке различного контента - в данный момент сцен и префабов
/// </summary>
public class Loader : Singleton<Loader>
{
    public GameObject InteractionManagers;

    #region Loading Scenes

    /// <summary>
    /// Имя первой сцены, которая будет автоматически загружена просле старта Framework-сцены
    /// </summary>
    public string SceneNameToLoadOnStart = "";

    private string previousSceneName;
    private string currentSceneName;

    private GameObject currentScene;
    private SceneLoadingMode currentMode;
    private bool currentWithLoadingScreen;

    private GameObject TrashCanObject;

    private bool canLoadScene = true;
    private int numOfSceneManagersToUnload;
    private ISceneManager currentSceneManager;

    private Queue<string> scenesToLoadNext = new Queue<string>();
    private Queue<SceneLoadingMode> wantedModesToLoadNext = new Queue<SceneLoadingMode>();
    private Queue<bool> wantedLoadingScreenToLoadNext = new Queue<bool>();

    /// <summary>
    /// Вызывается, когда новозагруженная сцена считает себя полностью загруженной, сообщает об этом лоадеру и он вызывает метод StartScene его IsceneManager'а
    /// </summary>
    public Action NewSceneStarted;
    /// <summary>
    /// Вызывается при старте загружки сцены, передает имя загружаемой сцены
    /// </summary>
    public Action<string> SceneLoadingStart;
    /// <summary>
    /// Вызывается каждый кадр во время загрузки сцены, передает текущий прогресс загрузки
    /// </summary>
    public Action<float> SceneLoadingProgress;
    /// <summary>
    /// Вызывается при окончании загрузки сцены, передает имя загруженной сцены
    /// </summary>
    public Action<string> SceneLoadingComplete;

    private AsyncOperation sceneLodingOperation;

    private void Start()
    {
        if (SceneNameToLoadOnStart != "")
        {
            LoadScene(SceneNameToLoadOnStart, SceneLoadingMode.Additive, false);
        }
    }

    /// <summary>
    /// Основной метод для загрузки сцены. Загрузка выполняется всегда асинхронно.
    /// </summary>
    /// <param name="sceneName">Имя сцены, которую требуется загрузкить</param>
    /// <param name="mode">Режим загрузки сцены. В отличие от <see cref="LoadSceneMode"/>, контролирует, что произойдет с предыдущей сценой после загрузки новой сцены.</param>
    /// <param name="withLoadingScreen">Нужно ли показывать экран загрузки</param>
    public void LoadScene(string sceneName, SceneLoadingMode mode = SceneLoadingMode.Single, bool withLoadingScreen = true)
    {
        /*
           Процес загрузки/выгрузки может быть в 3х вариантах:
             I. У нас не загружена еще ни одна сцена - в таком случае загружаем ее и вызываем его метод StartScene.
             II. У нас загружена какая-то сцена, и мы загружаем новую ей на замену. В таком случае мы помещаем все объекты в сцене кроме объектов-исключений в отдельный TrashCanObject и загружаем новую сцену в additive-mode. Когда она загружена, проделываем следующие действия:
              а.) Ищем все SceneManager'ы в TrashCanObject и вызываем их PrepareToUnload
              б.) Когда все это сделано - уничтожаем все объекты в TrashCanObject и сам TrashCanObject
              в.) Ищем SceneManager новой сцены и вызываем его StartScene
             III. У нас загружена какая-то сцена и мы загружаем новую, не уничтожая старую. В этом случае мы просто загружаем новую сцену в additive-mode и после загрузки вызываем StartScene его SceneManager'а.

           При этом если вызываем экран загрузки - выходим из старых сцен перед появлением экрана загрузки, а если нет - выходим из старых сцен после того как загрузкили новую сцену - для сохранения эффекта непрерывности, ибо новые сцены могут грузиться довольно долго, и, если мы уберем старую сцену сначала, то пользователь может решить, что приложение не реагирует
         */

        if (canLoadScene)
        {
            canLoadScene = false;

            currentMode = mode;
            currentWithLoadingScreen = withLoadingScreen;
            if (currentSceneName == "")
            {
                //Первую сцену всегда загружаем без экрана загрузки, потому что лучше смотрится, когда первой голограмой человек видит лого, 
                //или что-нибудь иное особенное, отличающееся от стандартного, часто появляющегося в дальнейшем, экрана
                currentSceneName = sceneName;
                sceneLodingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                canLoadScene = true;
            }
            else
            {
                previousSceneName = currentSceneName;
                currentSceneName = sceneName;

                switch (mode)
                {
                    case SceneLoadingMode.Additive:
                        currentSceneName = sceneName;
                        if (withLoadingScreen)
                        {
                            //Если показываем окно загрузки - то сначала "выходим" из текущей сцены, а затем уже включаем окно загрузки, 
                            //ибо, если окно загрузки накладывается на прочий контент сцены - это выглядит некрасиво.
                            PrepareAllCurrentScenesForClosing(() =>
                            {
                                LoadingWindowScript.Instance.Show(() =>
                                {
                                    sceneLodingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                                });
                            });
                        }
                        else
                        {
                            sceneLodingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                        }
                        break;
                    case SceneLoadingMode.Single:

                        //Если показываем окно загрузки - то сначала "выходим" из текущей сцены, а затем уже включаем окно загрузки, 
                        //ибо, если окно загрузки накладывается на прочий контент сцены - это выглядит некрасиво.
                        if (withLoadingScreen)
                        {
                            PrepareAllCurrentScenesForClosing(() =>
                            {
                                LoadingWindowScript.Instance.Show(() =>
                                {
                                    PackAllToTrashCan();
                                    sceneLodingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                                });
                            });
                        }
                        else
                        {
                            PackAllToTrashCan();
                            sceneLodingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                        }
                        break;
                }
            }

            if (SceneLoadingStart != null)
            {
                SceneLoadingStart.Invoke(sceneName);
            }

            StartCoroutine(SceneLoadingObserve());
        }
        else
        {
            //Если до того как сцена полностью загружена мы даем команду загрузить еще сцену - сохраняем эту инфу
            scenesToLoadNext.Enqueue(sceneName);
            wantedModesToLoadNext.Enqueue(mode);
            wantedLoadingScreenToLoadNext.Enqueue(withLoadingScreen);
        }
    }

    private IEnumerator SceneLoadingObserve()
    {
        while (!sceneLodingOperation.isDone)
        {
            if (SceneLoadingProgress != null)
            {
                SceneLoadingProgress.Invoke(sceneLodingOperation.progress);
            }
            yield return null;
        }

        if (SceneLoadingComplete != null)
        {
            SceneLoadingComplete.Invoke(currentSceneName);
        }
    }


    private Action callAfterAllScenesPreparedForClosing;
    private int numOfScenesPreparedForClosing;
    private void PrepareAllCurrentScenesForClosing(System.Action callback = null)
    {
        callAfterAllScenesPreparedForClosing = callback;
        
        ISceneManager[] gos = findAllRootISceneManagers();

        numOfScenesPreparedForClosing = 0;
        for (int i = 0; i < gos.Length; i++)
        {
            numOfScenesPreparedForClosing++;
            gos[i].PrepareToUnload(decrNumOfScenesPreparedForClosing);
        }

        StartCoroutine(TrackPreparingAllCurrentScenesForClosing());
    }

    private ISceneManager[] findAllRootISceneManagers()
    {
        UnityEngine.Object[] gos = FindObjectsOfType(typeof(GameObject));
        List<ISceneManager> result = new List<ISceneManager>();

        for (int i = 0; i < gos.Length; i++)
        {
            if ((gos[i] as GameObject).GetComponent<ISceneManager>() != null)
            {
                result.Add((gos[i] as GameObject).GetComponent<ISceneManager>());
            }
        }

        return result.ToArray();
    }

    private void decrNumOfScenesPreparedForClosing()
    {
        numOfScenesPreparedForClosing--;
    }

    private IEnumerator TrackPreparingAllCurrentScenesForClosing()
    {
        //Проверяем в корутине, т.к. нужно выждать некоторое время из-за возможности мгновенного возврата PrepareToUnload одной из нескольких выгружаемых сцен.
        yield return null;

        while (true)
        {
            if (numOfScenesPreparedForClosing == 0)
            {
                callAfterAllScenesPreparedForClosing.Invoke();
                callAfterAllScenesPreparedForClosing = null;
                break;
            }
            else
            {
                yield return null;
            }
        }
    }

    private void PackAllToTrashCan()
    {
        TrashCanObject = new GameObject("TrashCanObject");

        //GameObject[] rootGOs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        GameObject[] rootGOs = findAllRootGOs();

        for (int i = 0; i < rootGOs.Length; i++)
        {
            if (rootGOs[i].gameObject.name == "[DOTween]" ||
                rootGOs[i].gameObject.name == "LineManager")
            {
                rootGOs[i].gameObject.layer = 9;
            }
        }

        for (int i = 0; i < rootGOs.Length; i++)
        {
            if ((rootGOs[i].GetComponent<IUndestroyableOnLoad>() == null) &&
                (rootGOs[i].GetComponent<LeanTween>() == null) &&
                (rootGOs[i].gameObject.layer != 9) &&
                (!rootGOs[i].Equals(TrashCanObject)))
            {
                rootGOs[i].transform.parent = TrashCanObject.transform;
            }
        }
    }

    private GameObject[] findAllRootGOs()
    {
        UnityEngine.Object[] gos = FindObjectsOfType(typeof(GameObject));
        List<GameObject> result = new List<GameObject>();

        for (int i = 0; i < gos.Length; i++)
        {
            if ((gos[i] as GameObject).transform.parent == null)
            {
                result.Add((gos[i] as GameObject));
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Эту функцию необходимо вызывать каждой новозагруженной сцене, когда она решит, что она загружена. Обычно - в старте SceneManager'а, или после подгрузки всего необходимого для успешного старта.
    /// </summary>
    /// <param name="sceneManager">Передаем ISceneManager загруженной сцены</param>
    public void IThinkIWasLoadedCompletelyAndCanStart(ISceneManager sceneManager)
    {
        currentSceneManager = sceneManager;

        if (TrashCanObject != null)
        {
            //Если мы работаем с окном загрузки - значит мы уже "вышли" из всех сцен и этот этап можно пропустить
            if (!currentWithLoadingScreen)
            {
                numOfSceneManagersToUnload = 0;
                ISceneManager[] sceneManagers = TrashCanObject.GetComponentsInChildren<ISceneManager>();
                for (int i = 0; i < sceneManagers.Length; i++)
                {
                    numOfSceneManagersToUnload++;
                    sceneManagers[i].PrepareToUnload(decrNumOfSceneManagersToUnload);
                }

                StartCoroutine(StartChechingUnloadingOfAllScenes());
            }
            else
            {
                StartNewScene();
            }
        }
        else
        {
            StartNewScene();
        }
    }

    private void decrNumOfSceneManagersToUnload()
    {
        numOfSceneManagersToUnload--;
    }

    private IEnumerator StartChechingUnloadingOfAllScenes()
    {
        //Проверяем в корутине, т.к. нужно выждать некоторое время из-за возможности мгновенного возврата PrepareToUnload одной из нескольких выгружаемых сцен.
        yield return null;

        while (true)
        {
            if (numOfSceneManagersToUnload == 0)
            {
                StartNewScene();
                break;
            }
            else
            {
                yield return null;
            }
        }
    }

    private void StartNewScene()
    {
        if (TrashCanObject)
        {
            Destroy(TrashCanObject);
            TrashCanObject = null;
        }

        if (currentWithLoadingScreen)
        {
            LoadingWindowScript.Instance.Hide(() =>
            {
                InvokeStartScene(currentSceneManager);
            });
        }
        else
        {
            InvokeStartScene(currentSceneManager);
        }

        
    }

    private void InvokeStartScene(ISceneManager sceneManager)
    {
        if (sceneManager != null)
        {
            sceneManager.StartScene();
        }

        currentSceneManager = null;
        canLoadScene = true;

        if (NewSceneStarted != null)
        {
            NewSceneStarted.Invoke();
        }

        checkNextScenesInQueue();
    }

    private void checkNextScenesInQueue()
    {
        if (scenesToLoadNext.Count > 0)
        {
            LoadScene(scenesToLoadNext.Dequeue(), wantedModesToLoadNext.Dequeue(), wantedLoadingScreenToLoadNext.Dequeue());
        }
    }

    public void GoToPreviousScene(SceneLoadingMode mode = SceneLoadingMode.Single, bool withLoadingScreen = true)
    {
        LoadScene(previousSceneName, mode, withLoadingScreen);
    }

    #endregion

    #region Loading Prefabs

    private List<string> loadPrefabNamesQueue = new List<string>();
    private List<Action<GameObject>> loadPrefabCallbackFuncQueue = new List<Action<GameObject>>();
    private Action<GameObject> loadPrefabCallbackFunc;
    private List<Action<float>> loadPrefabProgressFuncsQueue = new List<Action<float>>();
    private Action<float> loadPrefabProgressFunc;
    private ResourceRequest resReq;

    private List<LoadMultiplePrefabsOptions> multipleLoadOptionses = new List<LoadMultiplePrefabsOptions>();
    private LoadMultiplePrefabsOptions multipleLoadOptions;
    private UnityEngine.Object[] gosToInstantiate;
    private List<Action<GameObject[]>> loadMultiplePrefabCallbackFunctions = new List<Action<GameObject[]>>();
    private Action<GameObject[]> loadMultiplePrefabCallbackFunc;
    private List<Action<float>> loadMultiplePrefabProgressFunctions = new List<Action<float>>();
    private Action<float> loadMultiplePrefabProgressFunc;
    private GameObject[] multipleGOs;
    private List<string[]> multiplePrefabNamesArrays = new List<string[]>();
    private string[] multiplePrefabNames;
    private int currentLoadingPrefab;
    private ResourceRequest multipleResReq;

    /// <summary>
    /// Загружает и инстантиирует несколько префабов асинхронно
    /// </summary>
    /// <param name="PrefabsNames">Имена требуемых префабов</param>
    /// <param name="callbackFunc">Функция для передачи назад ссылки на инстантиированные геймобджекты</param>
    /// <param name="options">Режим загрузки</param>
    /// <param name="progressFunc">Коллбэк для прогресса загрузки</param>
    public void LoadInstantiateMultiplePrefabs(string[] PrefabsNames, Action<GameObject[]> callbackFunc, LoadMultiplePrefabsOptions options = LoadMultiplePrefabsOptions.LoadThenIntantiate, Action<float> progressFunc = null)
    {
        if (loadMultiplePrefabCallbackFunc == null)
        {
            multipleLoadOptions = options;
            if (multipleLoadOptions == LoadMultiplePrefabsOptions.LoadThenIntantiate)
            {
                gosToInstantiate = new UnityEngine.Object[PrefabsNames.Length];
            }
            multipleGOs = new GameObject[PrefabsNames.Length];
            loadMultiplePrefabCallbackFunc = callbackFunc;
            loadMultiplePrefabProgressFunc = progressFunc;
            multiplePrefabNames = PrefabsNames;
            currentLoadingPrefab = 0;

            LoadNextRes();
        }
        else
        {
            multipleLoadOptionses.Add(options);
            loadMultiplePrefabCallbackFunctions.Add(callbackFunc);
            loadMultiplePrefabProgressFunctions.Add(progressFunc);
            multiplePrefabNamesArrays.Add(PrefabsNames);
        }
    }

    private void goToNextArray()
    {
        multipleLoadOptions = multipleLoadOptionses[0];
        if (multipleLoadOptions == LoadMultiplePrefabsOptions.LoadThenIntantiate)
        {
            gosToInstantiate = new UnityEngine.Object[multiplePrefabNamesArrays[0].Length];
        }
        multipleGOs = new GameObject[multiplePrefabNamesArrays[0].Length];
        loadMultiplePrefabCallbackFunc = loadMultiplePrefabCallbackFunctions[0];
        loadMultiplePrefabProgressFunc = loadMultiplePrefabProgressFunctions[0];
        multiplePrefabNames = multiplePrefabNamesArrays[0];
        currentLoadingPrefab = 0;

        multiplePrefabNamesArrays.RemoveAt(0);
        loadMultiplePrefabCallbackFunctions.RemoveAt(0);
        loadMultiplePrefabProgressFunctions.RemoveAt(0);
        multipleLoadOptionses.RemoveAt(0);

        LoadNextRes();
    }

    private void LoadNextRes()
    {
        if (currentLoadingPrefab < multiplePrefabNames.Length)
        {
            multipleResReq = Resources.LoadAsync(multiplePrefabNames[currentLoadingPrefab]);
        }
        else
        {
            if (multipleLoadOptions == LoadMultiplePrefabsOptions.LoadThenIntantiate)
            {
                for (int i = 0; i < gosToInstantiate.Length; i++)
                {
                    multipleGOs[i] = (GameObject)Instantiate(gosToInstantiate[i], Vector3.zero, Quaternion.identity);
                }
            }

            loadMultiplePrefabCallbackFunc.Invoke(multipleGOs);

            if (multiplePrefabNamesArrays.Count > 0)
            {
                goToNextArray();
            }
            else
            {
                loadMultiplePrefabCallbackFunc = null;
                loadMultiplePrefabProgressFunc = null;
                multipleResReq = null;
            }
        }
    }

    /// <summary>
    /// Загружает и инстантиирует один префаб асинхронно
    /// </summary>
    /// <param name="PrefabName">Имя требуемого префаба</param>
    /// <param name="callbackFunc">Функция для передачи назад ссылки на инстантиированный геймобджект</param>
    /// <param name="progressFunc">Коллбэк для прогресса загрузки</param>
    public void LoadAndIntsntiateGOPrefab(string PrefabName, Action<GameObject> callbackFunc, Action<float> progressFunc = null)
    {
        if (loadPrefabCallbackFunc == null)
        {
            loadPrefabCallbackFunc = callbackFunc;
            loadPrefabProgressFunc = progressFunc;

            LoadRes(PrefabName);
        }
        else
        {
            loadPrefabCallbackFuncQueue.Add(callbackFunc);
            loadPrefabProgressFuncsQueue.Add(progressFunc);
            loadPrefabNamesQueue.Add(PrefabName);
        }
    }

    private void LoadRes(string n)
    {
        resReq = Resources.LoadAsync(n);
    }

    private void RememberAssetToInstantiateLater()
    {
        gosToInstantiate[currentLoadingPrefab] = multipleResReq.asset;

        currentLoadingPrefab++;

        LoadNextRes();
    }

    private void InstantiateGOPrefabFromMultiple()
    {
        multipleGOs[currentLoadingPrefab] = (GameObject)Instantiate(multipleResReq.asset, Vector3.zero, Quaternion.identity);

        currentLoadingPrefab++;

        LoadNextRes();
    }

    private void instantiateGOPrefab()
    {
        loadPrefabCallbackFunc.Invoke((GameObject)Instantiate(resReq.asset, Vector3.zero, Quaternion.identity));

        if (loadPrefabNamesQueue.Count > 0)
        {
            loadPrefabCallbackFunc = loadPrefabCallbackFuncQueue[0];
            loadPrefabProgressFunc = loadPrefabProgressFuncsQueue[0];

            LoadRes(loadPrefabNamesQueue[0]);

            loadPrefabCallbackFuncQueue.RemoveAt(0);
            loadPrefabProgressFuncsQueue.RemoveAt(0);
            loadPrefabNamesQueue.RemoveAt(0);
        }
        else
        {
            loadPrefabCallbackFunc = null;
            loadPrefabProgressFunc = null;
            resReq = null;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (multipleResReq != null)
        {
            if (loadMultiplePrefabProgressFunc != null)
            {
                loadMultiplePrefabProgressFunc.Invoke(currentLoadingPrefab * (1 / multiplePrefabNames.Length) + multipleResReq.progress / multiplePrefabNames.Length);
            }
            if (multipleResReq.isDone)
            {
                if (multipleLoadOptions == LoadMultiplePrefabsOptions.LoadAndInstantiate)
                {
                    InstantiateGOPrefabFromMultiple();
                }
                else
                {
                    RememberAssetToInstantiateLater();
                }
            }
        }

        if (resReq != null)
        {
            if (loadPrefabProgressFunc != null)
            {
                loadPrefabProgressFunc.Invoke(resReq.progress);
            }
            if (resReq.isDone)
            {
                instantiateGOPrefab();
            }
        }
    }
    #endregion

    // Andrew Milko
    public void TurnOffManagers()
    {   
        SourceOfGestures.Instance.gestureRecognizer.CancelGestures();
        SourceOfGestures.Instance.gestureRecognizer.StopCapturingGestures();
        InteractionManagers.SetActive(false);
    }

    // Andrew Milko
    public void TurnOnManagers()
    {
        OwnGestureManager.Instance.tapGestureRecognizer.CancelGestures();
        OwnGestureManager.Instance.tapGestureRecognizer.StopCapturingGestures();

        InteractionManagers.SetActive(true);
        SourceOfGestures.Instance.gestureRecognizer.StartCapturingGestures();
    }
}