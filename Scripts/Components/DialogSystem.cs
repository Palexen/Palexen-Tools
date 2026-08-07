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
using System;
using UnityEngine;
using Random = UnityEngine.Random;
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
        [SerializeField] [LanguagesDropdown("Language")] private string _lang;
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
            if (LangManager.instance.Subtitles == SubtitlesUsage.yes)
            {
                _subtitles.text = "";
            }

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
                    if (LangManager.instance.Subtitles == SubtitlesUsage.yes)
                    {
                        _subtitles.text = "";
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

            if (LangManager.instance.Subtitles == SubtitlesUsage.yes)
            {
                _subtitles.text = "";
            }

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

            ExecuteDialogPlayback(playback);

            currentSequence = LangManager.instance.LangIndex;
        }

        void InterRandomPlay()
        {
            int randomIndex = Random.Range(0, _dialogSequencer[LangManager.instance.LangIndex]._sequence.Count);

            ExecuteDialogPlayback(randomIndex);
        }

        private void ExecuteDialogPlayback(int sequenceIndex)
        {
            int langIndex = LangManager.instance.LangIndex; //
            var currentContainer = _dialogSequencer[langIndex]._sequence[sequenceIndex]._dialogContainer;

            if (LangManager.instance.Subtitles == SubtitlesUsage.yes)
            {
                string actorHexColor = currentContainer._actorColor.ConvertToHex();
                _subtitles.text = $"<color=#{actorHexColor}>{currentContainer._actorName}</color>{currentContainer._dialogText}";
            }

            if (_dialogAudioFeature == DialogAudioFeature.useAudio)
            {
                _langAudioSource.clip = currentContainer._langClip;
                _langAudioSource.Play();
            }
            else
            {
                playbackTimer = currentContainer._onScreenTimeDialog;

                if (Order == DialogOrder.sequenced) 
                {
                    PlayNextDialogQueue();
                }
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
                _lang = LangManager.instance.LangName;
            }
        }

        /// <summary>
        /// Set a new text on the screen, if you prefer to set and display text on a specific screen.
        /// </summary>
        /// <remarks> When you call the method, it sets a new text element TMP_Text</remarks>
        /// <param name="newText"></param>
        [Obsolete("Use SubtitlesText Property instead")]
        public void SetTextUI(TMP_Text newText)
        {
            _subtitles = newText;
        }

        /// <summary>
        /// Call this method when you need to change the audio source.
        /// </summary>
        /// <param name="newAudioSource"></param>
        [Obsolete("Use Speaker Property instead")]
        public void SetAudioSource(AudioSource newAudioSource)
        {
            _langAudioSource = newAudioSource;
        }

        #endregion

        #region PROPERTIES
        
        public DialogAudioFeature Feature { get { return _dialogAudioFeature; } }
        public DialogOrder Order {  get { return _order; } set { _order = value; } }
        public bool IsPlaying { get { return isPlaying; } }
        public bool IsFinished { get { return dialogComplete; } }
        public bool InDebug { get { return debugMode; } set { debugMode = value; } }
        public TMP_Text SubtitlesText { get { return _subtitles; } set { _subtitles = value; } }
        public AudioSource Speaker { get { return _langAudioSource; } set { _langAudioSource = value; } }

        #endregion
    }
}
