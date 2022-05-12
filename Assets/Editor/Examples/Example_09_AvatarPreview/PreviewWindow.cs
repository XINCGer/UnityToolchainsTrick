using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace ToolKits
{
    public class PreviewWindow : EditorWindow
    {
        private static PreviewWindow _window;
        private static readonly Vector2 MIN_SIZE = new Vector2(600, 400);

        private static readonly Rect PREVIEW_RECT = new Rect(0, 50, 500, 300);

        private const string PREVIEW_ANIMCONTROLLER_PATH =
            "Assets/GameAssets/Arts/AnimatorControllers/PreviewController.controller";

        private const string AVATAR_PATH = "Assets/GameAssets/Arts/Prefabs/Warrior.prefab";
        private const string CLIP_PATH = "Assets/GameAssets/Arts/Models/AnimationClips/Female/1HIdle.anim";

        private AvatarPreview _avatarPreview;
        private AnimationClip _animationClip;
        private Animator _animator;
        private AnimatorController _previewAnimator;
        private GameObject PreviewInstance;
        private AnimatorState _animatorState;

        [MenuItem("Tools/PreviewWindow", priority = 9)]
        private static void PopUp()
        {
            _window = GetWindow<PreviewWindow>("预览窗口");
            _window.minSize = MIN_SIZE;
            _window.Init();
            _window.Show();
        }

        private void Init()
        {
        }

        private void OnGUI()
        {
            if (GUILayout.Button("加载预览"))
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AVATAR_PATH);
                PreviewInstance = EditorHelper.InstantiateGoByPrefab(prefab, null);
                PreviewInstance.hideFlags = HideFlags.HideAndDontSave;
                PreviewInstance.tag = Constants.PREVIRE_TAG;
                _previewAnimator = AssetDatabase.LoadAssetAtPath<AnimatorController>(PREVIEW_ANIMCONTROLLER_PATH);
                _animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(CLIP_PATH);
                var states = _previewAnimator.layers[0].stateMachine.states;
                foreach (var item in states)
                {
                    if (item.state.name == "Preview")
                    {
                        _animatorState = item.state;
                        break;
                    }
                }

                InitController();

                _avatarPreview = new AvatarPreview(_animator, _animationClip);
                _avatarPreview.OnAvatarChangeFunc = SetPreviewAvatar;
                _avatarPreview.fps = Mathf.RoundToInt(_animationClip.frameRate);
                _avatarPreview.ShowIKOnFeetButton = (_animationClip as Motion).isHumanMotion;
                _avatarPreview.ResetPreviewFocus();
                _avatarPreview.timeControl.stopTime = _animationClip.length;

                // force an update on timeControl if AvatarPreviewer is closed when creating/editing animation curves
                // prevent from having a nomralizedTime == -inf
                if (_avatarPreview.timeControl.currentTime == Mathf.NegativeInfinity)
                    _avatarPreview.timeControl.Update();
            }

            if (null != _avatarPreview)
            {
                if (Event.current.type == EventType.Repaint)
                {
                    _avatarPreview.timeControl.loop = true;
                    _avatarPreview.timeControl.Update();
                    AnimationClipSettings previewInfo = AnimationUtility.GetAnimationClipSettings(_animationClip);
                    float normalizedTime = previewInfo.stopTime - previewInfo.startTime != 0
                        ? (_avatarPreview.timeControl.currentTime - previewInfo.startTime) /
                          (previewInfo.stopTime - previewInfo.startTime)
                        : 0.0f;
                    _avatarPreview.Animator.Play(0, 0, normalizedTime);
                    _avatarPreview.Animator.Update(_avatarPreview.timeControl.deltaTime);
                }
                _avatarPreview.DoAvatarPreview(PREVIEW_RECT, Constants.preBackgroundSolid);
            }

            Repaint();
        }

        private void SetPreviewAvatar()
        {
            DestroyController();
            InitController();
        }

        private void InitController()
        {
            _animator = PreviewInstance.GetComponent<Animator>();
            _animatorState.motion = _animationClip;
            _animator.runtimeAnimatorController = _previewAnimator;
        }

        private void DestroyController()
        {
        }

        private void OnDestroy()
        {
            if (null != _avatarPreview)
            {
                _avatarPreview.OnDestroy();
                _avatarPreview = null;
            }

            GameObject.DestroyImmediate(PreviewInstance);
            PreviewInstance = null;
            _animationClip = null;
            _animator = null;
            _previewAnimator = null;
            _animatorState = null;
        }

        private void OnDisable()
        {
            if (null != _avatarPreview)
            {
                _avatarPreview.OnDisable();
            }
        }
    }
}