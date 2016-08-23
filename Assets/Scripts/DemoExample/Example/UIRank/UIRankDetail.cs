﻿using UnityEngine;
using System.Collections;
using System;

namespace TinyFrameWork
{
    /// <summary>
    /// Rank system sub detail window
    /// </summary>
    public class UIRankDetail : UIBaseWindow, IWindowAnimation
    {
        private UILabel lbDetail;
        private UISprite spHeadIcon;
        private GameObject objAnimation;
        private GameObject btnClose;

        private GameObject btnGoToLevel;

        // animation content
        private TweenPosition twPosition;

        public override void InitWindowOnAwake()
        {
            this.windowID = WindowID.WindowID_Rank_Detail;
            InitWindowData();
            base.InitWindowOnAwake();

            lbDetail = GameUtility.FindDeepChild(this.gameObject, "Move/Label").GetComponent<UILabel>();
            spHeadIcon = GameUtility.FindDeepChild<UISprite>(this.gameObject, "Move/Sprite");
            objAnimation = GameUtility.FindDeepChild(this.gameObject, "Move").gameObject;
            btnClose = GameUtility.FindDeepChild(this.gameObject, "Move/Close").gameObject;
            btnGoToLevel = GameUtility.FindDeepChild(this.gameObject, "Move/BtnGotoLevel").gameObject;
            UIEventListener.Get(btnClose).onClick = OnBtnClose;

            // Go to the Level Window
            UIEventListener.Get(btnGoToLevel).onClick = delegate
            {
                UICenterMasterManager.GetInstance().ShowWindow(WindowID.WindowID_Level);
            };
        }

        public override void ResetWindow()
        {
            ResetAnimation();
        }

        protected override void InitWindowData()
        {
            base.InitWindowData();
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.NeedAdded;
        }
        public override void ShowWindow(BaseWindowContextData contextData)
        {
            IsLock = true;
            EnterAnimation(delegate
            {
                IsLock = false;
                Debuger.Log("UIRankDetail play enter Animation is over.");
            });
        }

        public override void HideWindow(Action onComplete)
        {
            QuitAnimation(delegate
            {
                Debuger.Log("UIRankDetail play quit animation is over.");
                if (onComplete != null)
                    onComplete();
            });
        }

        public void EnterAnimation(EventDelegate.Callback onComplete)
        {
            if(twPosition== null)
                twPosition = objAnimation.GetComponent<TweenPosition>();
            twPosition.PlayForward();
            EventDelegate.Set(twPosition.onFinished, onComplete);
        }

        public void QuitAnimation(EventDelegate.Callback onComplete)
        {
            if (twPosition == null)
                twPosition = objAnimation.GetComponent<TweenPosition>();
            twPosition.PlayReverse();
            EventDelegate.Set(twPosition.onFinished, onComplete);
        }

        public void ResetAnimation()
        {
            if (twPosition != null)
                twPosition.ResetToBeginningExtension(0.0f);
        }

        private void OnBtnClose(GameObject gb)
        {
           UIRankManager.GetInstance().HideWindow(windowID);
        }

        public void UpdateDetailData(string playerName, string iconName)
        {
            lbDetail.text = playerName;
            spHeadIcon.spriteName = iconName;
        }

    }
}
