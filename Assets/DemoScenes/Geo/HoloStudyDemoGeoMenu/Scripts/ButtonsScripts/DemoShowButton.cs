using System.Collections.Generic;
using UnityEngine.Windows.Speech;
using System.Linq;
using System;
using UnityEngine;

public class DemoShowButton : RingedAndTintedButton
{
    /*
     * Кнопка демо-режима имеет следующую логику - она становится неактивной, когда профессор читает лекцию, показывается MoreInfo, показывается пирожок или меняется Земля
     * Когда демо-режим запущен, она становися выделенной, все остальное время - нет
     */

    #region word recognizing

    private KeywordRecognizer keywordRecognizer = null;
    private Dictionary<string, Action> keywords = null;

    private void uninitSpeechCommands()
    {
#if !UNITY_EDITOR
        if (keywords != null)
        {
            keywords.Clear();
            keywords = null;
        }

        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
            keywordRecognizer = null;
        }
#endif
    }

    private void initSpeechCommands()
    {
        if (keywordRecognizer == null)
        {
#if !UNITY_EDITOR
            keywords = new Dictionary<string, Action>();
            keywords.Add("Stop", () =>
            {
                toggleDemo();
            });

            keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

            keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
            keywordRecognizer.Start();
#endif
        }
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

#endregion

    protected override void singleTapAction()
    {
        base.singleTapAction();

        toggleDemo();
    }

    private void toggleDemo()
    {
        if (DemoShowStateMachine.playing)
        {
            DemoShowStateMachine.Instance.Interrupt();
        }
        else
        {
            DemoShowStateMachine.Instance.StartDemoShow();
        }
    }

    private void Start()
    {
        ProfessorsLecturesController.Instance.LectureStarted += onLectureStarted;
        ProfessorsLecturesController.Instance.LectureEnded += onLectureEnded;

        DemoShowStateMachine.Instance.DemoShowStartedPlaying += onDemoShowStartedPlaying;
        DemoShowStateMachine.Instance.DemoShowEndedPlaying += onDemoShowEndedPlaying;

        MoreInfoController.Instance.MoreInfoShowed += onMoreInfoShowed;
        MoreInfoController.Instance.MoreInfoHided += onMoreInfoHided;

        EarthController.Instance.ChangesStarted += onEarthChangeStarted;
        EarthController.Instance.ChangesEnded += onEarthChangesEnded;

        PiePolygon.Instance.Showed += onPieShowed;
        PiePolygon.Instance.Hided += onPieHided;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (ProfessorsLecturesController.Instance != null)
        {
            ProfessorsLecturesController.Instance.LectureStarted -= onLectureStarted;
            ProfessorsLecturesController.Instance.LectureEnded -= onLectureEnded;
        }

        if (DemoShowStateMachine.Instance != null)
        {
            DemoShowStateMachine.Instance.DemoShowStartedPlaying -= onDemoShowStartedPlaying;
            DemoShowStateMachine.Instance.DemoShowEndedPlaying -= onDemoShowEndedPlaying;
        }

        if (MoreInfoController.Instance != null)
        {
            MoreInfoController.Instance.MoreInfoShowed -= onMoreInfoShowed;
            MoreInfoController.Instance.MoreInfoHided -= onMoreInfoHided;
        }

        if (EarthController.Instance != null)
        {
            EarthController.Instance.ChangesStarted -= onEarthChangeStarted;
            EarthController.Instance.ChangesEnded -= onEarthChangesEnded;
        }

        if (PiePolygon.Instance != null)
        {
            PiePolygon.Instance.Showed -= onPieShowed;
            PiePolygon.Instance.Hided -= onPieHided;
        }

        uninitSpeechCommands();
    }

    private void onDemoShowStartedPlaying()
    {
        reactingOnEarth = false;
        initSpeechCommands();
        MyRings.Show();
    }

    private void onDemoShowEndedPlaying()
    {
        reactingOnEarth = true;
        uninitSpeechCommands();
        MyRings.Hide();
    }

    private void onLectureStarted()
    {
        reactingOnEarth = false;
        disable();
    }

    private void onLectureEnded()
    {
        reactingOnEarth = true;
        enable();
    }

    private void onMoreInfoShowed()
    {
        reactingOnEarth = false;
        disable();
    }

    private void onMoreInfoHided()
    {
        reactingOnEarth = true;
        enable();
    }

    private void onPieShowed()
    {
        reactingOnEarth = false;
        disable();
    }

    private void onPieHided()
    {
        reactingOnEarth = true;
        enable();
    }

    //На изменения Земли реагируем только если мы не в демо-режиме и не внутри лекции, ибо в этих режимах часто идет работа с Землей
    private bool reactingOnEarth = true;

    private void onEarthChangeStarted()
    {
        if (reactingOnEarth)
        {
            enabledByFunctionality = false;
            MyTinter.Tint();
        }
    }

    private void onEarthChangesEnded()
    {
        if (reactingOnEarth)
        {
            enabledByFunctionality = true;
            MyTinter.Untint();
        }
    }
}
