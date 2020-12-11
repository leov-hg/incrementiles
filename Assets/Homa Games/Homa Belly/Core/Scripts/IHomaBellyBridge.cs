﻿using System.Collections.Generic;

namespace HomaGames.HomaBelly
{
    /// <summary>
    /// Interface exposed with Homa Belly to interact
    /// with any of its products: mediations, attributions or analytics.
    /// </summary>
    public interface IHomaBellyBridge
    {
        #region Base
        /// <summary>
        /// Sets the debug flag
        /// </summary>
        /// <param name="enabled">True to see debug information, false othwerise</param>
        void SetDebug(bool enabled);

        /// <summary>
        /// Validate the SDKs integration
        /// </summary>
        void ValidateIntegration();

        /// <summary>
        /// Determines if Homa Belly is already initialized
        /// </summary>
        /// <returns></returns>
        bool IsInitialized
        {
            get;
        }

        #endregion

        #region Ads

        /// <summary>
        /// Requests to show a rewarded video ad
        /// </summary>
        /// <param name="placementId">(optional) The ad placement</param>
        void ShowRewardedVideoAd(string placementId = null);

        /// <summary>
        /// Determines if a rewarded video ad is available
        /// </summary>
        /// <returns></returns>
        bool IsRewardedVideoAdAvailable(string placementId = null);

        /// <summary>
        /// Loads a banner with the given size and position
        /// </summary>
        /// <param name="size">The banner's size</param>
        /// <param name="position">The banner's position</param>
        /// <param name="placementId">(optional) The ad placement</param>
        /// <param name="bannerBackgroundColor">(optional) The banner background color</param>
        void LoadBanner(BannerSize size, BannerPosition position, string placementId = null, UnityEngine.Color bannerBackgroundColor = default);

        /// <summary>
        /// Show the latest loaded banner
        /// </summary>
        void ShowBanner(string placementId = null);

        /// <summary>
        /// Hides the latest banner shown
        /// </summary>
        void HideBanner(string placementId = null);

        /// <summary>
        /// Destroys the latest loaded banner
        /// </summary>
        void DestroyBanner(string placementId = null);

        /// <summary>
        /// Shows the latest interstitial loaded ad
        /// </summary>
        /// <param name="placementId">(optional) The ad placement</param>
        void ShowInsterstitial(string placementId = null);

        /// <summary>
        /// Determines if the interstitial ad is available
        /// </summary>
        /// <returns></returns>
        bool IsInterstitialAvailable(string placementId = null);

        #endregion

        #region GDPR/CCPA
        /// <summary>
        /// Specifies if the user asserted being above the required age
        /// </summary>
        /// <param name="consent">true if user accepted, false otherwise</param>
        void SetUserIsAboveRequiredAge(bool consent);

        /// <summary>
        /// Specifies if the user accepted privacy policy and terms and conditions
        /// </summary>
        /// <param name="consent">true if user accepted, false otherwise</param>
        void SetTermsAndConditionsAcceptance(bool consent);

        /// <summary>
        /// Specifies if the user granted consent for analytics tracking
        /// </summary>
        /// <param name="consent">true if user accepted, false otherwise</param>
        void SetAnalyticsTrackingConsentGranted(bool consent);

        /// <summary>
        /// Specifies if the user granted consent for showing tailored ads
        /// </summary>
        /// <param name="consent">true if user accepted, false otherwise</param>
        void SetTailoredAdsConsentGranted(bool consent);

        #endregion

        #region Analytics

        /// <summary>
        /// Tracks an In App Purchase event. Purchase can be verified if
        /// `transactionId` and `payload` are informed for the corresponding platforms
        /// </summary>
        /// <param name="productId">The product id puchased</param>
        /// <param name="currencyCode">The currency code of the purchase</param>
        /// <param name="unitPrice">The unit price</param>
        /// <param name="transactionId">(Optional) The transaction id for the IAP validation</param>
        /// <param name="payload">(Optional - Only Android) Payload for Android IAP validation</param>
        void TrackInAppPurchaseEvent(string productId, string currencyCode, double unitPrice, string transactionId = null, string payload = null);

        /// <summary>
        /// Track a resource flow (source/sink) event
        /// </summary>
        /// <param name="flowType">`Source` when user obtains some resource, or `Sink` when user spens some resource</param>
        /// <param name="currency">Resource name</param>
        /// <param name="amount">Resource amount</param>
        /// <param name="itemType">Resource type</param>
        /// <param name="itemId">Resource id</param>
        void TrackResourceEvent(ResourceFlowType flowType, string currency, float amount, string itemType, string itemId);

        /// <summary>
        /// Tracks a progression event
        /// </summary>
        /// <param name="progressionStatus">Start, Complete or Fail</param>
        /// <param name="progression">Progress description</param>
        /// <param name="score">(Optional) Score</param>
        void TrackProgressionEvent(ProgressionStatus progressionStatus, string progression, int score = 0);

        /// <summary>
        /// Tracks an error event
        /// </summary>
        /// <param name="severity">Debug, Info, Warning, Error or Critical</param>
        /// <param name="message">The error message</param>
        void TrackErrorEvent(ErrorSeverity severity, string message);

        /// <summary>
        /// Tracks a design event
        /// </summary>
        /// <param name="eventName">The design event name</param>
        /// <param name="eventValue">(Optional) Any event value</param>
        void TrackDesignEvent(string eventName, float eventValue = 0f);

        /// <summary>
        /// Tracks an Ad event
        /// </summary>
        /// <param name="adAction">Clicked, FailedShow, RewardReceived, Request or Loaded</param>
        /// <param name="adType">Ad type</param>
        /// <param name="adNetwork">Ad network</param>
        /// <param name="adPlacementId">Ad placement id</param>
        void TrackAdEvent(AdAction adAction, AdType adType, string adNetwork, string adPlacementId);

        /// <summary>
        /// Tracks an Ad Revenue event
        /// </summary>
        /// <param name="adRevenueData">Object holding all ad revenue data to be sent</param>
        void TrackAdRevenue(AdRevenueData adRevenueData);

        /// <summary>
        /// Tracks an event on the attribution platform
        /// </summary>
        /// <param name="eventName">The event name</param>
        /// <param name="arguments">(Optional) Additional arguments. Dictionary values must have one of these types: string, int, long, float, double, null, ArrayList, Dictionary<String,object></param>
        void TrackAttributionEvent(string eventName, Dictionary<string, object> arguments = null);

        #endregion
    }
}