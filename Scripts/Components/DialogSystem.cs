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
using UnityEngine.UI;
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
        public Language _lang;
        public Initializer _catchLang;

        [MyHeader("Audio Feature")]
        public DialogAudioFeature _dialogAudioFeature = DialogAudioFeature.useAudio;
        [FieldColor(FieldPropertyColor.orange, ShowObjectMessage.warningMessage)] public AudioSource _langAudioSource;

        [MyHeader("Finish")]
        public ObjectManagerInteractionMode _afterComplete = ObjectManagerInteractionMode.destroy;

        [MyHeader("Subtitles UI")]
        [FieldColor(FieldPropertyColor.pink, ShowObjectMessage.errorMessage)] public TMP_Text _subtitles;
        public List<DialogSequencer> _dialogSequencer;

        [Header("Debug")]
        public bool debugMode;
        public bool isPlaying = false;
        public int playback;
        public int currentSequence;
        public bool dialogComplete;
        public float playbackTimer;
        public int nextToPlay;

        #endregion

        #region UNITY METHODS

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _subtitles.text = "";

            UpdateLang();
        }

        // Update is called once per frame
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

        #endregion

        #region API

        /// <summary>
        /// This method initiates the playback of a dialog sequence based on the selected language. It retrieves the
        /// appropriate dialog text and audio clip from the dialog sequencer and plays them.
        /// </summary>
        [ContextMenu("Play Dialog")]
        public void PlayDialog()
        {
            isPlaying = true;

            switch(_lang)
            {
                case Language.english:
                    _subtitles.text = _dialogSequencer[0]._sequence[playback]._dialogContainer._dialogText;

                    if(_dialogAudioFeature == DialogAudioFeature.useAudio)
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
                    _subtitles.text = _dialogSequencer[1]._sequence[playback]._dialogContainer._dialogText;

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
                    _subtitles.text = _dialogSequencer[2]._sequence[playback]._dialogContainer._dialogText;

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
                    _subtitles.text = _dialogSequencer[3]._sequence[playback]._dialogContainer._dialogText;

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
                    _subtitles.text = _dialogSequencer[4]._sequence[playback]._dialogContainer._dialogText;

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
                    _subtitles.text = _dialogSequencer[5]._sequence[playback]._dialogContainer._dialogText;

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
                    _subtitles.text = _dialogSequencer[6]._sequence[playback]._dialogContainer._dialogText;

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
                    _subtitles.text = _dialogSequencer[7]._sequence[playback]._dialogContainer._dialogText;

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

        public void UpdateLang()
        {
            if (_catchLang == Initializer.auto)
            {
                _lang = LangManager.instance.GetLang();
            }
        }

        #endregion
    }
}
