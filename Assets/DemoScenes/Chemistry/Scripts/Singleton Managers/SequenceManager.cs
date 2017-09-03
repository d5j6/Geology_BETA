using UnityEngine;
using System.Collections.Generic;
using Gemeleon;

public class SequenceManager : Singleton<SequenceManager>
{
    private bool _isPlaying;
    public bool isPlaying { get { return _isPlaying; } }

    private Dictionary<string, Sequence> _sequences = new Dictionary<string, Sequence>();
    
    public void AddSequence(string name, Sequence sequence)
    {
        Sequence seq = new Sequence();
        if(_sequences.TryGetValue(name, out seq))
        {
            Debug.Log(string.Format("Sequence with name '{0}' already exist"));
            return;
        }

        _sequences.Add(name, sequence);
    }

    public void RemoveSequence(string name)
    {
        Sequence seq = new Sequence();
        if(!_sequences.TryGetValue(name, out seq))
        {
            Debug.Log(string.Format("Sequence with name '{0}' not exist"));
            return;
        }

        _sequences.Remove(name);
    }

    public void Play(string sequenceName)
    {
        if(_isPlaying)
        {
            Debug.Log(string.Format("Can not play sequence with name '{0}', sequencer is currently playing", sequenceName));
            return;
        }

        Sequence seq = new Sequence();
        if(!_sequences.TryGetValue(sequenceName, out seq))
        {
            Debug.Log(string.Format("Sequence with name '{0}' not exist", sequenceName));
            return;
        }

        _isPlaying = true;

        seq.Play();

        PlayerManager.Instance.ChangeStateToDefault();
    }

    void Awake()
    {
        // Subscribe to update _isPlaying field when sequence complete
        EventManager.Instance.sequenceCompleteEvent.Subscribe(() => { _isPlaying = false; });
    }
}
