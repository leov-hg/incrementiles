using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
using UnityEngine.Networking;

namespace FM {
    public class Prefs {
        public static string SESSION_COUNT = "FM_RB_SessionCount";
        public static string EVENT_COUNT = "FM_RB_EventCount";
        public static string RATED = "FM_RB_Rated";
        public static string POSTPONED = "FM_RB_Postponed";
        public static string POSTPONE_TIME = "FM_RB_PostponeTime";
        public static string OPTED_OUT = "FM_RB_OptedOut";
        public static string APP_VERSION = "FM_RB_AppVersion";
    }
    public enum RateBoxes { RateBoxUI01, RateBoxUI02, RateBoxUI03 }
    public class RateBox : MonoBehaviour {
        public static RateBox instance;

        [Header("Display Options")]
        [Tooltip("Show ratebox after every given session")]
        public bool showOnAlternateSessions;
        public int alternateSessionCount = 1;
        [Tooltip("Delay after every launch in minutes. (Will work with display on launch checked)")]
        public float delayAfterLaunch;

        [Header("Display Locations")]
        public bool displayOnLaunch;
        [Tooltip("Display ratebox after a custom event count is complete")]
        public bool displayOnCustomEvent;
        public bool refreshCountEverySession;
        public int customEventCount;
        public bool requireInternetConnection;

        [Header("Popup Options")]
        [Tooltip("New ios in game rating popup")]
        public bool iosInGameReview;
        [Tooltip("If in game rating popup is active, do you want to show RateBox before that?")]
        public bool showRateBoxWithInGameReviewPopup = true;
        [Tooltip("Rating above a certain number will be published and you can opt to get email if rating is below that number")]
        public int minRatingToPublish;
        [Tooltip("Email rating which is less than the given number")]
        public bool emailBadRating;
        public string toEmail;
        public string subject;
        public string text;
        public bool includeGivenRating;

        [Header("Popup Text")]
        public string title;
        public string message;
        public string rateButton;
        public string postponeButton;
        [Tooltip("Postpone button timer")]
        public float postponeCooldownInMinutes;
        public string optOutButton;

        [Header("Rate After Update")]
        public bool showAfterUpdate;
        public bool showAfterUpdateOnIos;
        public bool showAfterUpdateOnAndroid;

        [Header("Store Pages")]
        public string appStoreAppId;

        [Header("RateBox UI Style")]
        public RateBoxes rateBoxStyles;

        bool rightSession;
        int eventCount;

        void Awake() {
            if (instance != null) {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
            rightSession = false;

            if (PlayerPrefs.GetString(Prefs.APP_VERSION, "-1") == "-1") {
                PlayerPrefs.SetString(Prefs.APP_VERSION, Application.version);
            }

            if (PlayerPrefs.GetInt(Prefs.RATED, 0) == 1) {
                string lastVersion = PlayerPrefs.GetString(Prefs.APP_VERSION, "-1");
                string currentVersion = Application.version;

                if (lastVersion == currentVersion) {
                    Destroy(gameObject);
                } else {
                    PlayerPrefs.SetString(Prefs.APP_VERSION, currentVersion);

#if UNITY_ANDROID
                    if (!showAfterUpdateOnAndroid) {
                        Destroy(gameObject);
                        return;
                    } else {
                        ResetPrefs();
                    }
#elif UNITY_IOS
                    if (!showAfterUpdateOnIos) {
                        Destroy(gameObject);
                        return;
                    } else {
                        ResetPrefs();
                    }
#endif
                }
            }
            if (PlayerPrefs.GetInt(Prefs.OPTED_OUT, 0) == 1) {
                Destroy(gameObject);
                return;
            }
        }

        void Start() {
            eventCount = refreshCountEverySession ? 0 : PlayerPrefs.GetInt(Prefs.EVENT_COUNT, 0);
            int sessionCount = PlayerPrefs.GetInt(Prefs.SESSION_COUNT, 0) + 1;
            PlayerPrefs.SetInt(Prefs.SESSION_COUNT, sessionCount);

            System.Action CheckConditions = () => {
                if (rightSession && displayOnLaunch) {
                    long currentTime = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerSecond;
                    if (PlayerPrefs.GetInt(Prefs.POSTPONED, 0) == 0) {
                        StartCoroutine(Delay(delayAfterLaunch * 60, () => {
                            Show();
                        }));
                    } else {
                        if (currentTime - long.Parse(PlayerPrefs.GetString(Prefs.POSTPONE_TIME, "0")) >= postponeCooldownInMinutes * 60) {
                            PlayerPrefs.SetInt(Prefs.POSTPONED, 0);
                            StartCoroutine(Delay(delayAfterLaunch * 60, () => {
                                Show();
                            }));
                        }
                    }
                }
            };

            if (showOnAlternateSessions) {
                if (sessionCount % alternateSessionCount == 0) {
                    if (requireInternetConnection) {
                        IsNetworkAvailable((bool available) => {
                            if (!available) {
                                sessionCount--;
                                PlayerPrefs.SetInt(Prefs.SESSION_COUNT, sessionCount);
                            } else {
                                rightSession = true;
                                CheckConditions();
                            }
                        });
                    } else {
                        rightSession = true;
                        CheckConditions();
                    }
                }
            } else {
                rightSession = true;
                CheckConditions();
            }
        }

        public static void IncreaseEventCount() {
            if (instance == null) {
                return;
            }
            instance.eventCount++;
            PlayerPrefs.SetInt(Prefs.EVENT_COUNT, instance.eventCount);
            if (instance.eventCount % instance.customEventCount == 0) {
                if (instance.rightSession) {
                    if (instance.requireInternetConnection) {
                        long currentTime = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerSecond;
                        if (currentTime - long.Parse(PlayerPrefs.GetString(Prefs.POSTPONE_TIME, "0")) >= instance.postponeCooldownInMinutes * 60) {
                            instance.IsNetworkAvailable((bool available) => {
                                if (available) {
                                    Show();
                                } else {
                                    instance.eventCount--;
                                    PlayerPrefs.SetInt(Prefs.EVENT_COUNT, instance.eventCount);
                                }
                            });
                        }
                    } else {
                        Show();
                    }
                }
            }
        }

        public static void Show() {
            if (instance == null) {
                return;
            }
#if UNITY_IOS
            if (!instance.iosInGameReview) {
                instance.SpawnRateBox();
            } else if (instance.iosInGameReview) {
                if (instance.showRateBoxWithInGameReviewPopup) {
                    instance.SpawnRateBox();
                } else {
                    iOSReviewRequest.Request();
                }
            }
#elif UNITY_ANDROID
            instance.SpawnRateBox();
#endif
        }

        void SpawnRateBox() {
            Transform canvasT = Instantiate(Resources.Load("Canvas")as GameObject).transform;
            canvasT.GetComponent<Canvas>().sortingOrder = 100;
            EventSystem es = GameObject.FindObjectOfType<EventSystem>();
            if (es == null) {
                Instantiate(Resources.Load("EventSystem")as GameObject);
            }

            RectTransform rateBox = Instantiate((Resources.Load(rateBoxStyles.ToString())as GameObject)).GetComponent<RectTransform>();
            RateBoxUI rbUi = rateBox.GetComponent<RateBoxUI>();
            rbUi.SetUIText(title, message, optOutButton, postponeButton, rateButton);

            rateBox.SetParent(canvasT);
            rateBox.anchoredPosition = Vector2.zero;

            rateBox.offsetMin = Vector2.zero;
            rateBox.offsetMax = Vector2.zero;

            rateBox.GetChild(0).localScale = rateBox.localScale;
            if (rateBox.GetChild(0).localScale.x > 1) {
                rateBox.GetChild(0).localScale = new Vector2(1, 1);
            }
            rateBox.localScale = new Vector2(1, 1);
        }

        public static void Rate(int rating) {
            if (instance == null) {
                return;
            }
            if (rating >= instance.minRatingToPublish) {
                PlayerPrefs.SetInt(Prefs.RATED, 1);
#if UNITY_ANDROID
                Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
#elif UNITY_IOS
                if (instance.iosInGameReview) {
                    string deviceVersion = Device.systemVersion;
                    if (deviceVersion.Length > 4) {
                        deviceVersion = deviceVersion.Substring(0, 4);
                    }
                    if (deviceVersion[deviceVersion.Length - 1] == '.') {
                        deviceVersion = deviceVersion.Substring(0, deviceVersion.Length - 1);
                    }
                    if (float.Parse(deviceVersion) >= 10.3f) {
                        iOSReviewRequest.Request();
                    } else {
                        Application.OpenURL("https://itunes.apple.com/app/id" + instance.appStoreAppId);
                    }
                } else {
                    Application.OpenURL("https://itunes.apple.com/app/id" + instance.appStoreAppId);
                }
#endif
            } else {
                Application.OpenURL("mailto:" + instance.toEmail + "?subject=" + UnityWebRequest.EscapeURL(instance.subject + (instance.includeGivenRating ? (" [" + rating + " Stars]") : "")).Replace("+", "%20") + "&body=" + UnityWebRequest.EscapeURL(instance.text).Replace("+", "%20"));
            }
        }

        public static void Later() {
            if (instance == null) {
                return;
            }
            long currentTime = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerSecond;
            PlayerPrefs.SetInt(Prefs.POSTPONED, 1);
            PlayerPrefs.SetString(Prefs.POSTPONE_TIME, "" + currentTime);
        }

        public static void OptOut() {
            if (instance == null) {
                return;
            }
            PlayerPrefs.SetInt(Prefs.OPTED_OUT, 1);
            Destroy(instance.gameObject);
        }

        void IsNetworkAvailable(System.Action<bool> action) {
            StartCoroutine(checkInternetConnection(action));
        }

        IEnumerator checkInternetConnection(System.Action<bool> action) {
            UnityWebRequest www = new UnityWebRequest("https://google.com");
            yield return www.SendWebRequest();
            if (www.error != null) {
                action(false);
            } else {
                action(true);
            }
        }

        IEnumerator Delay(float delay, System.Action _OnComplete) {
            yield return new WaitForSeconds(delay);
            if (_OnComplete != null)_OnComplete();
        }

        void ResetPrefs() {
            PlayerPrefs.SetInt(Prefs.SESSION_COUNT, 0);
            PlayerPrefs.SetInt(Prefs.EVENT_COUNT, 0);
            PlayerPrefs.SetInt(Prefs.RATED, 0);
            PlayerPrefs.SetInt(Prefs.POSTPONED, 0);
            PlayerPrefs.SetString(Prefs.POSTPONE_TIME, "0");
        }

        public void HardResetPrefs() {
            ResetPrefs();
            PlayerPrefs.SetInt(Prefs.OPTED_OUT, 0);
            PlayerPrefs.SetString(Prefs.APP_VERSION, "-1");
        }

    }
}