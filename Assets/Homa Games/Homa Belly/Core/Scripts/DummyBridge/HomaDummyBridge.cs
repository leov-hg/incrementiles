﻿#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomaGames.HomaBelly.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace HomaGames.HomaBelly
{
    /// <summary>
    /// Homa Bridge is the main connector between the public facade (HomaBelly)
    /// and all the inner behaviour of the Homa Belly library. All features
    /// and callbacks will be centralized within this class.
    /// </summary>
    public class HomaDummyBridge : IHomaBellyBridge
    {
        private const string RES_PATH = "Assets/Homa Games/Homa Belly/Core/Prefabs/DummyAds/";
        #region Private properties
        private bool BannerLoaded = false;
        private bool RewardedVideoLoaded = false;
        private GameObject DummyBanner;
        private Events m_events = new Events();
        private bool initialized = false;
        public bool IsInitialized
        {
            get
            {
                return initialized;
            }
        }
        #endregion

        public void Initialize()
        {
            HomaGamesLog.Debug("[Homa Belly] Initializing Homa Belly Dummy for Unity Editor");
            RemoteConfiguration.FetchRemoteConfiguration().ContinueWith((remoteConfiguration) =>
            {
                HomaGamesLog.Debug("[Homa Belly] Initializing Homa Belly after Remote Configuration fetch");
                InitializeRemoteConfigurationDependantComponents(remoteConfiguration.Result);

            }, TaskScheduler.FromCurrentSynchronizationContext());

            InitializeRemoteConfigurationIndependentComponents();
        }

        /// <summary>
        /// Initializes all those components that can be initialized
        /// before the Remote Configuration data is fetched
        /// </summary>
        private void InitializeRemoteConfigurationIndependentComponents()
        {
            // Apply Homa Games GDPR values
            GDPRUtils.ApplyHomaGamesGDPRValues();
            LoadRewardedVideoAd();

            // Notify initialized
            initialized = true;
            m_events.OnInitialized();
        }

        /// <summary>
        /// Initializes all those components that require from Remote Configuration
        /// data in order to initialize
        /// </summary>
        private void InitializeRemoteConfigurationDependantComponents(RemoteConfiguration.RemoteConfigurationSetup remoteConfigurationSetup)
        {
            CrossPromotionManager.Initialize(remoteConfigurationSetup);
        }

        public void SetDebug(bool enabled)
        {

        }

        public void ValidateIntegration()
        {

        }

        public void OnApplicationPause(bool pause)
        {

        }

        #region IHomaBellyBridge

        public void ShowRewardedVideoAd(string placementId = null)
        {
            if (!RewardedVideoLoaded)
            {
                HomaGamesLog.Warning("[Homa Belly] Rewarded Video Ad not yet loaded.");
            }
            else
            {
                m_events.OnRewardedVideoAdStartedEvent(placementId);
                ShowDummyRewardedAd(placementId);
            }
        }

        public bool IsRewardedVideoAdAvailable(string placementId = null)
        {
            return RewardedVideoLoaded;
        }

        private void LoadRewardedVideoAd(string placementId = null)
        {
            ExecuteWithDelay(1f, () =>
            {
                RewardedVideoLoaded = true;
                m_events.OnRewardedVideoAvailabilityChangedEvent(true,placementId);
            });
        }

        private void ShowDummyRewardedAd(string placementId)
        {
            GameObject rewardedPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(RES_PATH + "Rewarded.prefab");
            GameObject dummyRewardedAd = UnityEngine.Object.Instantiate(rewardedPrefab, Vector3.zero, Quaternion.identity);
            bool grantedReward = false;
            DummyRewardBehaviour dummy = dummyRewardedAd.GetComponent<DummyRewardBehaviour>();
            dummy.HomaRewardedCloseButton.onClick.AddListener(() =>
            {
                if (grantedReward)
                {
                    m_events.OnRewardedVideoAdRewardedEvent(new VideoAdReward(dummy.NameInput.text, int.Parse(dummy.QuantityInput.text)));
                }
                m_events.OnRewardedVideoAdClosedEvent();
                LoadRewardedVideoAd(placementId);
                UnityEngine.Object.Destroy(dummyRewardedAd);
            });
            dummy.HomaRewardButton.onClick.AddListener(() =>
            {
                grantedReward = true;
                dummy.HomaRewardStatus.text = "Reward granted. Will send reward callback on ad close.";
            });
            m_events.OnRewardedVideoAvailabilityChangedEvent(false,placementId);
            RewardedVideoLoaded = false;
        }

        // Banners
        public void LoadBanner(BannerSize size, BannerPosition position, string placementId = null, Color bannerBackgroundColor = default)
        {
            if (!BannerLoaded)
            {
                // Only support BottomCenter and TopCenter for now
                string bannerPrefabName = position == BannerPosition.BOTTOM ? "BannerBottom" : "BannerTop";
                GameObject bannerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(RES_PATH + bannerPrefabName + ".prefab");
                GameObject dummyBanner = UnityEngine.Object.Instantiate(bannerPrefab, Vector3.zero, Quaternion.identity);
                dummyBanner.SetActive(false); // Hidden by default

                DummyBanner = dummyBanner;
                BannerLoaded = true;
                ExecuteWithDelay(0.1f, () => m_events.OnBannerAdLoadedEvent(placementId));
            }
        }

        public void ShowBanner(string placementId = null)
        {
            if (!BannerLoaded)
            {
                HomaGamesLog.Warning("[Homa Belly] Banner was not created, can not show it");
            }
            else
            {
                if (DummyBanner)
                {
                    DummyBanner.SetActive(true);
                }
            }
        }

        public void HideBanner(string placementId = null)
        {
            if (DummyBanner)
            {
                DummyBanner.SetActive(false);
            }
        }

        public void DestroyBanner(string placementId = null)
        {
            if (DummyBanner)
            {
                UnityEngine.Object.Destroy(DummyBanner);
            }
        }

        public void ShowInsterstitial(string placementId = null)
        {
            m_events.OnInterstitialAdOpenedEvent();
            GameObject interstitialPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(RES_PATH + "/Interstitial.prefab");
            GameObject dummyInterstitial = UnityEngine.Object.Instantiate(interstitialPrefab, Vector3.zero, Quaternion.identity);
            DummyInterstitialBehaviour dummy = dummyInterstitial.GetComponent<DummyInterstitialBehaviour>();

            dummy.HomaInterstitialCloseButton.onClick.AddListener(() =>
            {
                m_events.OnInterstitialAdClosedEvent();
                UnityEngine.Object.Destroy(dummyInterstitial);
            });
            m_events.OnInterstitialAdShowSucceededEvent(placementId);
        }

        public bool IsInterstitialAvailable(string placementId = null)
        {
            return true;
        }

        public void SetUserIsAboveRequiredAge(bool consent)
        {

        }

        public void SetTermsAndConditionsAcceptance(bool consent)
        {

        }

        public void SetAnalyticsTrackingConsentGranted(bool consent)
        {

        }

        public void SetTailoredAdsConsentGranted(bool consent)
        {

        }

        public void TrackInAppPurchaseEvent(string productId, string currencyCode, double unitPrice, string transactionId = null, string payload = null)
        {
            DummyEvent("InAppPurchase", "productId=" + productId, "currencyCode=" + currencyCode, "unitPrice=" + unitPrice, "transactionId=" + transactionId, "payload=" + payload);
        }

        public void TrackResourceEvent(ResourceFlowType flowType, string currency, float amount, string itemType, string itemId)
        {
            DummyEvent("ResourceEvent", "ResourceFlowType=" + flowType.ToString(), "currency=" + currency, "amount=" + amount, "itemType=" + itemType, "itemId=" + itemId);
        }

        public void TrackProgressionEvent(ProgressionStatus progressionStatus, string progression, int score = 0)
        {
            DummyEvent("Progression", "ProgressionStatus=" + progressionStatus.ToString(), "progression=" + progression, "score=" + score);
        }

        public void TrackErrorEvent(ErrorSeverity severity, string message)
        {
            DummyEvent("Error", "ErrorSeverity=" + severity.ToString(), "message=" + message);
        }

        public void TrackDesignEvent(string eventName, float eventValue = 0f)
        {
            DummyEvent("Design", "eventName=" + eventName, "eventValue=" + eventValue);
        }

        public void TrackAdEvent(AdAction adAction, AdType adType, string adNetwork, string adPlacementId)
        {
            DummyEvent("Ad", "AdAction=" + adAction.ToString(), "AdType=" + adType.ToString(), "adNetwork=" + adNetwork, "adPlacementId=" + adPlacementId);
        }

        public void TrackAdRevenue(AdRevenueData adRevenueData)
        {
            DummyEvent("AdRevenue", "AdRevenueData=" + adRevenueData.ToString());
        }

        public void TrackAttributionEvent(string eventName, Dictionary<string, object> arguments = null)
        {
            DummyEvent(eventName, "Arguments=" + Json.Serialize(arguments));
        }

        private void DummyEvent(string eventName, params string[] p)
        {
            var str = "[Homa Belly] Tracking " + eventName + " Event : ";
            foreach (string param in p)
                str += " " + param;
            HomaGamesLog.Debug(str);
        }

        private void ExecuteWithDelay(float seconds, Action action)
        {
            Task.Delay((int)(seconds * 1000)).ContinueWith((result) =>
              {
                  action();
              }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion
    }
}
#endif