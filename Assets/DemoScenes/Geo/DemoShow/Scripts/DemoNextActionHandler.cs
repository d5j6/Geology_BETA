using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;
using System.Linq;
using HoloToolkit.Unity;

public class DemoNextActionHandler : Singleton<DemoNextActionHandler> {

    IDemoShowState NextScene;
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    public void PrepareNextAction(IDemoShowState next)
    {
        NextScene = next;
#if !UNITY_EDITOR
        initSpeechCommands();
#endif
        initTapAction();
    }

    public void UnprepareActions()
    {
#if !UNITY_EDITOR
        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
            keywordRecognizer = null;
        }
#endif
        TapsListener.Instance.UserSingleTapped -= OnTapped;
        NextScene = null;
    }

    void OnDestroy()
    {
#if !UNITY_EDITOR
        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
            keywordRecognizer = null;
        }
#endif
        if (TapsListener.Instance != null)
        {
            TapsListener.Instance.UserSingleTapped -= OnTapped;
        }
    }

    private void initTapAction()
    {
        TapsListener.Instance.UserSingleTapped += OnTapped;
    }

    private void OnTapped(GameObject obj)
    {
        DemoShowStateMachine.Instance.GotoState(NextScene);
        UnprepareActions();
    }

    void initSpeechCommands()
    {
        keywords = new Dictionary<string, System.Action>();
        keywords.Add("Next", () =>
        {
            DemoShowStateMachine.Instance.GotoState(NextScene);
            NextScene = null;
            UnprepareActions();
        });

        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}
