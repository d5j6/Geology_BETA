using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

namespace Gemeleon
{
    public class Sequence
    {
        private int _nextSubSequenceIndex = 1;

        List<SubSequence> _subSequences = new List<SubSequence>();

        public void AddSubSequence(SubSequence subSeq)
        {
            subSeq.subSequence.OnComplete(() =>
            {
                // If subSequence is not last
                if(subSeq.nextSubSequenceIndex != -1)
                {
                    // nextSubSequenceIndex can be changed in runtime, need call with Restart
                    _subSequences[subSeq.nextSubSequenceIndex].subSequence.Restart();
                }
                else
                {
                    EventManager.Instance.sequenceCompleteEvent.Publish();
                }
            });

            _subSequences.Add(subSeq);
        }

        public void RemoveSubSequence(SubSequence subSeq)
        {
            _subSequences.Remove(subSeq);
        }

        public void Play()
        {
            if(_subSequences.Count > 0)
            {
                // Start first subSequense with restart command
                _subSequences[0].subSequence.Restart();
            }
        }
    }
}