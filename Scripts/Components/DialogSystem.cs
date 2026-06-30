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
using TMPro;
using UnityEngine;
using System.Collections.Generic;

#if PALEXEN_TOOLS
using Palexen.Tools;
#endif

namespace Palexen.Sequences
{
    #if PALEXEN_TOOLS
    [ScriptDescription("Dialog System", "Handles the management and retrieval of dialog sequences.")]
#endif
    [AddComponentMenu("Palexen/Sequences/Dialog System")]
    public class DialogSystem : MonoBehaviour
    {
        #region VARIABLES

        [MyHeader("Language")]
        [SerializeField] private Language _lang;
        [SerializeField] private Initializer _catchLang;

        [MyHeader("Audio Feature")]
        [SerializeField] private DialogAudioFeature _dialogAudioFeature = DialogAudioFeature.useAudio;
        [FieldColor(FieldPropertyColor.orange, ShowObjectMessage.warningMessage)][SerializeField] private AudioSource _langAudioSource;

        [MyHeader("Finish")]
        [SerializeField] private ObjectManagerInteractionMode _afterComplete = ObjectManagerInteractionMode.destroy;

        [MyHeader("Subtitles UI")]
        [FieldColor(FieldPropertyColor.pink, ShowObjectMessage.errorMessage)][SerializeField] private TMP_Text _subtitles;

        [SerializeField] private DialogOrder _order;
        [SerializeField] private List<DialogSequencer> _dialogSequencer;

        [Header("Debug")]
        [SerializeField] private bool debugMode;
        [SerializeField] private bool isPlaying = false;
        [SerializeField] private int playback;
        [SerializeField] private int currentSequence;
        [SerializeField] private bool dialogComplete;
        [SerializeField] private float playbackTimer;
        [SerializeField] private int nextToPlay;

        #endregion

        #region UNITY METHODS

        void Start()
        {
            _subtitles.text = "";

            UpdateLang();
        }

        void Update()
        {
            OnPlayDialogs();
        }

        #endregion

        #region MECHANICS

        /// <summary>
        /// This method is responsible for managing the playback of dialog sequences. It checks if a dialog is 
        /// currently playing and if the audio source has finished playing. If the current dialog has finished, it 
        /// advances to the next dialog in the sequence or restores the system if the sequence is complete.
        /// </summary>
        void OnPlayDialogs()
        {
            if (_dialogAudioFeature == DialogAudioFeature.useAudio)
            {
                if (!dialogComplete && isPlaying)
                {
                    if (!_langAudioSource.isPlaying)
                    {
                        int nextPlayback = playback + 1;

                        if (nextPlayback >= _dialogSequencer[currentSequence]._sequence.Count)
                        {
                            Restore();
                        }
                        else
                        {
                            playback = nextPlayback;
                            PlayDialog();
                        }
                    }
                }
            }

            if(Order == DialogOrder.random)
            {
                if (!_langAudioSource.isPlaying)
                {
                    _subtitles.text = "";
                }
            }
        }


        /// <summary>
        /// sets the dialog system to its initial state, stopping any ongoing dialog and clearing the text.
        /// </summary>
        void Restore()
        {
            playback = 0;
            isPlaying = false;
            dialogComplete = true;

            if(_dialogAudioFeature == DialogAudioFeature.noAudio)
            {
                nextToPlay = 0;
            }

            _subtitles.text = "";
            _langAudioSource.Stop();
            OnCompleteActions();
        }

        /// <summary>
        /// This method performs actions based on the selected interaction mode after a dialog sequence is 
        /// completed. It can either destroy the game object, deactivate it, or do nothing, depending on the selected mode.
        /// </summary>
        void OnCompleteActions()
        {
            switch (_afterComplete)
            {
                case ObjectManagerInteractionMode.destroy:
                    Destroy(gameObject);
                    break;
                case ObjectManagerInteractionMode.deactivate:
                    gameObject.SetActive(false);
                    break;
                case ObjectManagerInteractionMode.activate:
                    // Do nothing
                    break;
                default:
                    Debug.LogWarning("Invalid interaction mode.");
                    break;
            }
        }

        void InterPlay()
        {
            isPlaying = true;

            switch (_lang)
            {
                case Language.english:

                    _subtitles.text = $"<color={"#" + _dialogSequencer[0]._sequence[playback]._dialogContainer._actorColor.ConvertToHex() + ">"}" +
                        _dialogSequencer[0]._sequence[playback]._dialogContainer._actorName + "</color>" +
                        _dialogSequencer[0]._sequence[playback]._dialogContainer._dialogText;

                    if (_dialogAudioFeature == DialogAudioFeature.useAudio)
                    {
                        _langAudioSource.clip = _dialogSequencer[0]._sequence[playback]._dialogContainer._langClip;
                        _langAudioSource.Play();
                    }
                    else
                    {
                        playbackTimer = _dialogSequencer[0]._sequence[playback]._dialogContainer._onScreenTimeDialog;
                        PlayNextDialogQueue();
                    }

                    currentSequence = 0;
                    break;

                case Language.spanish:

                    _subtitles.text = $"<color={"#" + _dialogSequencer[1]._sequence[playback]._dialogContainer._actorColor.ConvertToHex() + ">"}" +
                    _dialogSequencer[1]._sequence[playback]._dialogContainer._actorName + "</color>" +
                    _dialogSequencer[1]._sequence[playback]._dialogContainer._dialogText;

                    if (_dialogAudioFeature == DialogAudioFeature.useAudio)
                    {
                        _langAudioSource.clip = _dialogSequencer[1]._sequence[playback]._dialogContainer._langClip;
                        _langAudioSource.Play();
                    }
                    else
                    {
                        playbackTimer = _dialogSequencer[1]._sequence[playback]._dialogContainer._onScreenTimeDialog;
                        PlayNextDialogQueue();
                    }

                    currentSequence = 1;
                    break;

                case Language.french:

                    _subtitles.text = $"<color={"#" + _dialogSequencer[2]._sequence[playback]._dialogContainer._actorColor.ConvertToHex() + ">"}" +
                    _dialogSequencer[2]._sequence[playback]._dialogContainer._actorName + "</color>" +
                    _dialogSequencer[2]._sequence[playback]._dialogContainer._dialogText;

                    if (_dialogAudioFeature == DialogAudioFeature.useAudio)
                    {
                        _langAudioSource.clip = _dialogSequencer[2]._sequence[playback]._dialogContainer._langClip;
                        _langAudioSource.Play();
                    }
                    else
                    {
                        playbackTimer = _dialogSequencer[2]._sequence[playback]._dialogContainer._onScreenTimeDialog;
                        PlayNextDialogQueue();
                    }

                    currentSequence = 2;
                    break;

                case Language.german:

                    _subtitles.text = $"<color={"#" + _dialogSequencer[3]._sequence[playback]._dialogContainer._actorColor.ConvertToHex() + ">"}" +
                    _dialogSequencer[3]._sequence[playback]._dialogContainer._actorName + "</color>" +
                    _dialogSequencer[3]._sequence[playback]._dialogContainer._dialogText;

                    if (_dialogAudioFeature == DialogAudioFeature.useAudio)
                    {
                        _langAudioSource.clip = _dialogSequencer[3]._sequence[playback]._dialogContainer._langClip;
                        _langAudioSource.Play();
                    }
                    else
                    {
                        playbackTimer = _dialogSequencer[3]._sequence[playback]._dialogContainer._onScreenTimeDialog;
                        PlayNextDialogQueue();
                    }

                    currentSequence = 3;
                    break;

                case Language.japanese:

                    _subtitles.text = $"<color={"#" + _dialogSequencer[4]._sequence[playback]._dialogContainer._actorColor.ConvertToHex() + ">"}" +
                    _dialogSequencer[4]._sequence[playback]._dialogContainer._actorName + "</color>" +
                    _dialogSequencer[4]._sequence[playback]._dialogContainer._dialogText;

                    if (_dialogAudioFeature == DialogAudioFeature.useAudio)
                    {
                        _langAudioSource.clip = _dialogSequencer[4]._sequence[playback]._dialogContainer._langClip;
                        _langAudioSource.Play();
                    }
                    else
                    {
                        playbackTimer = _dialogSequencer[4]._sequence[playback]._dialogContainer._onScreenTimeDialog;
                        PlayNextDialogQueue();
                    }

                    currentSequence = 4;
                    break;

                case Language.chinese:

                    _subtitles.text = $"<color={"#" + _dialogSequencer[5]._sequence[playback]._dialogContainer._actorColor.ConvertToHex() + ">"}" +
                    _dialogSequencer[5]._sequence[playback]._dialogContainer._actorName + "</color>" +
                    _dialogSequencer[5]._sequence[playback]._dialogContainer._dialogText;

                    if (_dialogAudioFeature == DialogAudioFeature.useAudio)
                    {
                        _langAudioSource.clip = _dialogSequencer[5]._sequence[playback]._dialogContainer._langClip;
                        _langAudioSource.Play();
                    }
                    else
                    {
                        playbackTimer = _dialogSequencer[5]._sequence[playback]._dialogContainer._onScreenTimeDialog;
                        PlayNextDialogQueue();
                    }

                    currentSequence = 5;
                    break;

                case Language.korean:

                    _subtitles.text = $"<color={"#" + _dialogSequencer[6]._sequence[playback]._dialogContainer._actorColor.ConvertToHex() + ">"}" +
                    _dialogSequencer[6]._sequence[playback]._dialogContainer._actorName + "</color>" +
                    _dialogSequencer[6]._sequence[playback]._dialogContainer._dialogText;

                    if (_dialogAudioFeature == DialogAudioFeature.useAudio)
                    {
                        _langAudioSource.clip = _dialogSequencer[6]._sequence[playback]._dialogContainer._langClip;
                        _langAudioSource.Play();
                    }
                    else
                    {
                        playbackTimer = _dialogSequencer[6]._sequence[playback]._dialogContainer._onScreenTimeDialog;
                        PlayNextDialogQueue();
                    }

                    currentSequence = 6;
                    break;

                case Language.russian:

                    _subtitles.text = $"<color={"#" + _dialogSequencer[7]._sequence[playback]._dialogContainer._actorColor.ConvertToHex() + ">"}" +
                    _dialogSequencer[7]._sequence[playback]._dialogContainer._actorName + "</color>" +
                    _dialogSequencer[7]._sequence[playback]._dialogContainer._dialogText;

                    if (_dialogAudioFeature == DialogAudioFeature.useAudio)
                    {
                        _langAudioSource.clip = _dialogSequencer[7]._sequence[playback]._dialogContainer._langClip;
                        _langAudioSource.Play();
                    }
                    else
                    {
                        playbackTimer = _dialogSequencer[7]._sequence[playback]._dialogContainer._onScreenTimeDialog;
                        PlayNextDialogQueue();
                    }

                    currentSequence = 7;
                    break;

                default:
                    Debug.LogWarning("Language not supported.");
                    break;
            }
        }


        void InterRandomPlay()
        {
            switch (_lang)
            {
                case Language.english:

                    int a = Random.Range(0, _dialogSequencer[0]._sequence.Count);

                    _subtitles.text = $"<color={"#" + _dialogSequencer[0]._sequence[a]._dialogContainer._actorColor.ConvertToHex() + ">"}" +
                        _dialogSequencer[0]._sequence[a]._dialogContainer._actorName + "</color>" +
                        _dialogSequencer[0]._sequence[a]._dialogContainer._dialogText;

                    if (_dialogAudioFeature == DialogAudioFeature.useAudio)
                    {
                        _langAudioSource.clip = _dialogSequencer[0]._sequence[a]._dialogContainer._langClip;
                        _langAudioSource.Play();
                    }
                    else
                    {
                        playbackTimer = _dialogSequencer[0]._sequence[a]._dialogContainer._onScreenTimeDialog;
                    }
                    break;

                case Language.spanish:

                    int b = Random.Range(0, _dialogSequencer[1]._sequence.Count);

                    _subtitles.text = $"<color={"#" + _dialogSequencer[1]._sequence[b]._dialogContainer._actorColor.ConvertToHex() + ">"}" +
                        _dialogSequencer[1]._sequence[b]._dialogContainer._actorName + "</color>" +
                        _dialogSequencer[1]._sequence[b]._dialogContainer._dialogText;

                    if (_dialogAudioFeature == DialogAudioFeature.useAudio)
                    {
                        _langAudioSource.clip = _dialogSequencer[1]._sequence[b]._dialogContainer._langClip;
                        _langAudioSource.Play();
                    }
                    else
                    {
                        playbackTimer = _dialogSequencer[1]._sequence[b]._dialogContainer._onScreenTimeDialog;
                    }
                    break;

                case Language.french:

                    int c = Random.Range(0, _dialogSequencer[2]._sequence.Count);

                    _subtitles.text = $"<color={"#" + _dialogSequencer[2]._sequence[c]._dialogContainer._actorColor.ConvertToHex() + ">"}" +
                        _dialogSequencer[2]._sequence[c]._dialogContainer._actorName + "</color>" +
                        _dialogSequencer[2]._sequence[c]._dialogContainer._dialogText;

                    if (_dialogAudioFeature == DialogAudioFeature.useAudio)
                    {
                        _langAudioSource.clip = _dialogSequencer[2]._sequence[c]._dialogContainer._langClip;
                        _langAudioSource.Play();
                    }
                    else
                    {
                        playbackTimer = _dialogSequencer[2]._sequence[c]._dialogContainer._onScreenTimeDialog;
                    }
                    break;

                case Language.german:

                    int d = Random.Range(0, _dialogSequencer[3]._sequence.Count);

                    _subtitles.text = $"<color={"#" + _dialogSequencer[3]._sequence[d]._dialogContainer._actorColor.ConvertToHex() + ">"}" +
                        _dialogSequencer[3]._sequence[d]._dialogContainer._actorName + "</color>" +
                        _dialogSequencer[3]._sequence[d]._dialogContainer._dialogText;

                    if (_dialogAudioFeature == DialogAudioFeature.useAudio)
                    {
                        _langAudioSource.clip = _dialogSequencer[3]._sequence[d]._dialogContainer._langClip;
                        _langAudioSource.Play();
                    }
                    else
                    {
                        playbackTimer = _dialogSequencer[3]._sequence[d]._dialogContainer._onScreenTimeDialog;
                    }
                    break;

                case Language.japanese:

                    int e = Random.Range(0, _dialogSequencer[4]._sequence.Count);

                    _subtitles.text = $"<color={"#" + _dialogSequencer[4]._sequence[e]._dialogContainer._actorColor.ConvertToHex() + ">"}" +
                        _dialogSequencer[4]._sequence[e]._dialogContainer._actorName + "</color>" +
                        _dialogSequencer[4]._sequence[e]._dialogContainer._dialogText;

                    if (_dialogAudioFeature == DialogAudioFeature.useAudio)
                    {
                        _langAudioSource.clip = _dialogSequencer[4]._sequence[e]._dialogContainer._langClip;
                        _langAudioSource.Play();
                    }
                    else
                    {
                        playbackTimer = _dialogSequencer[4]._sequence[e]._dialogContainer._onScreenTimeDialog;
                    }
                    break;

                case Language.chinese:

                    int f = Random.Range(0, _dialogSequencer[5]._sequence.Count);

                    _subtitles.text = $"<color={"#" + _dialogSequencer[5]._sequence[f]._dialogContainer._actorColor.ConvertToHex() + ">"}" +
                        _dialogSequencer[5]._sequence[f]._dialogContainer._actorName + "</color>" +
                        _dialogSequencer[5]._sequence[f]._dialogContainer._dialogText;

                    if (_dialogAudioFeature == DialogAudioFeature.useAudio)
                    {
                        _langAudioSource.clip = _dialogSequencer[5]._sequence[f]._dialogContainer._langClip;
                        _langAudioSource.Play();
                    }
                    else
                    {
                        playbackTimer = _dialogSequencer[5]._sequence[f]._dialogContainer._onScreenTimeDialog;
                    }
                    break;

                case Language.korean:

                    int g = Random.Range(0, _dialogSequencer[6]._sequence.Count);

                    _subtitles.text = $"<color={"#" + _dialogSequencer[6]._sequence[g]._dialogContainer._actorColor.ConvertToHex() + ">"}" +
                        _dialogSequencer[6]._sequence[g]._dialogContainer._actorName + "</color>" +
                        _dialogSequencer[6]._sequence[g]._dialogContainer._dialogText;

                    if (_dialogAudioFeature == DialogAudioFeature.useAudio)
                    {
                        _langAudioSource.clip = _dialogSequencer[6]._sequence[g]._dialogContainer._langClip;
                        _langAudioSource.Play();
                    }
                    else
                    {
                        playbackTimer = _dialogSequencer[6]._sequence[g]._dialogContainer._onScreenTimeDialog;
                    }
                    break;

                case Language.russian:

                    int h = Random.Range(0, _dialogSequencer[7]._sequence.Count);

                    _subtitles.text = $"<color={"#" + _dialogSequencer[0]._sequence[h]._dialogContainer._actorColor.ConvertToHex() + ">"}" +
                        _dialogSequencer[7]._sequence[h]._dialogContainer._actorName + "</color>" +
                        _dialogSequencer[7]._sequence[h]._dialogContainer._dialogText;

                    if (_dialogAudioFeature == DialogAudioFeature.useAudio)
                    {
                        _langAudioSource.clip = _dialogSequencer[7]._sequence[h]._dialogContainer._langClip;
                        _langAudioSource.Play();
                    }
                    else
                    {
                        playbackTimer = _dialogSequencer[7]._sequence[h]._dialogContainer._onScreenTimeDialog;
                    }
                    break;

                default:
                    Debug.LogWarning("Language not supported.");
                    break;
            }
        }

        #endregion

        #region API

        /// <summary>
        /// This method initiates the playback of a dialog sequence based on the selected language. It retrieves the
        /// appropriate dialog text and audio clip from the dialog sequencer and plays them.
        /// </summary>
        [ContextMenu("Play Dialog")]
        public void PlayDialog()
        {
            if (Order == DialogOrder.sequenced)
            {
                InterPlay();
            }

            if(Order == DialogOrder.random)
            {
                InterRandomPlay();
            }
        }

        /// <summary>
        /// Check the status of the dialogues; if the dialogue audio is not being used, it 
        /// will only show the texts saved in the container.
        /// </summary>
        void PlayNextDialogQueue()
        {
            if (!dialogComplete)
            {
                nextToPlay = playback + 1;

                if (nextToPlay >= _dialogSequencer[currentSequence]._sequence.Count)
                {
                    Invoke(nameof(Restore), playbackTimer);
                }
                else
                {
                    playback = nextToPlay;
                    Invoke(nameof(PlayDialog), playbackTimer);
                }
            }
        }


        /// <summary>
        /// This method allows for replaying the current dialog sequence from the beginning. It restores the 
        /// system to its initial state and then starts the dialog playback.
        /// </summary>
        [ContextMenu("Replay Dialog")]
        public void RePlay()
        {
            Restore();
            dialogComplete = false;
            PlayDialog();
        }

        /// <summary>
        /// This method breaks the current dialog sequence and restores the system to its initial state,
        /// allowing for a new dialog sequence to be played.
        /// </summary>
        [ContextMenu("Break Into Dialogue")]
        public void BreakIntoDialogue()
        {
            Restore();
        }

        /// <summary>
        /// This method updates the language of the dialog system based on the selected language in the LangManager. If the
        /// LangManager's language changes, this method should be called to reflect the new language in the dialog system.
        /// </summary>
        public void UpdateLang()
        {
            if (_catchLang == Initializer.auto)
            {
                _lang = LangManager.instance.Lang;
            }
        }

        /// <summary>
        /// Set a new text on the screen, if you prefer to set and display text on a specific screen.
        /// </summary>
        /// <remarks> When you call the method, it sets a new text element TMP_Text</remarks>
        /// <param name="newText"></param>
        public void SetTextUI(TMP_Text newText)
        {
            _subtitles = newText;
        }

        /// <summary>
        /// Call this method when you need to change the audio source.
        /// </summary>
        /// <param name="newAudioSource"></param>
        public void SetAudioSource(AudioSource newAudioSource)
        {
            _langAudioSource = newAudioSource;
        }

        #endregion

        #region PROPERTIES
        
        public DialogAudioFeature Feature { get { return _dialogAudioFeature; } }
        public DialogOrder Order {  get { return _order; } set { _order = value; } }
        public bool IsPlaying { get { return isPlaying; } }
        public bool InDebug { get { return debugMode; } set { debugMode = value; } }

        #endregion
    }
}
