using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace FM{
[CustomEditor(typeof(RateBox))]
    public class RateboxEditor : Editor {

        RateBox rb;

        void OnEnable(){
            rb = (RateBox) target;
        }

        public override void OnInspectorGUI(){
            EditorGUILayout.Space();

            rb.showOnAlternateSessions = EditorGUILayout.Toggle("Show On Alternate Sessions", rb.showOnAlternateSessions);
            if(rb.showOnAlternateSessions){
                EditorGUI.indentLevel++;
                rb.alternateSessionCount = EditorGUILayout.IntField("Alternate Session Count", rb.alternateSessionCount);
                if(rb.alternateSessionCount < 1){
                    rb.alternateSessionCount = 1;
                }
                EditorGUI.indentLevel--;
            }

            rb.displayOnLaunch = EditorGUILayout.Toggle("Display On Launch", rb.displayOnLaunch);
            if(rb.displayOnLaunch){
                EditorGUI.indentLevel++;
                rb.delayAfterLaunch = EditorGUILayout.FloatField("Delay After Launch In Minutes", rb.delayAfterLaunch);
                if(rb.delayAfterLaunch < 0){
                    rb.delayAfterLaunch = 0;
                }
                EditorGUI.indentLevel--;
            }

            rb.displayOnCustomEvent = EditorGUILayout.Toggle("Display On Custom Event", rb.displayOnCustomEvent);
            if(rb.displayOnCustomEvent){
                EditorGUI.indentLevel++;
                rb.refreshCountEverySession = EditorGUILayout.Toggle("Refresh Count Every Session", rb.refreshCountEverySession);
                rb.customEventCount = EditorGUILayout.IntField("Custom Event Count", rb.customEventCount);
                EditorGUI.indentLevel--;
            }

            rb.requireInternetConnection = EditorGUILayout.Toggle("Require Internet Connection", rb.requireInternetConnection);

            rb.minRatingToPublish = EditorGUILayout.IntField("Minimum Rating to Publish", rb.minRatingToPublish);
            if(rb.minRatingToPublish > 0){
                EditorGUI.indentLevel++;
                rb.emailBadRating = EditorGUILayout.Toggle("Email low rating", rb.emailBadRating);
                if(rb.emailBadRating){
                    EditorGUI.indentLevel++;
                    rb.toEmail = EditorGUILayout.TextField("To Email", rb.toEmail);
                    rb.subject = EditorGUILayout.TextField("Subject", rb.subject);
                    EditorGUILayout.LabelField("Text");
                    rb.text = EditorGUILayout.TextArea(rb.text, GUILayout.Height(50));
                    rb.includeGivenRating = EditorGUILayout.Toggle("Include Given Rating", rb.includeGivenRating);
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }
            if(rb.minRatingToPublish > 5){
                rb.minRatingToPublish = 5;
            }else if(rb.minRatingToPublish < 0){
                rb.minRatingToPublish = 0;
            }

            rb.iosInGameReview = EditorGUILayout.Toggle("Show iOS In Game Review Popup", rb.iosInGameReview);
            if(rb.iosInGameReview){
                EditorGUI.indentLevel++;
                rb.showRateBoxWithInGameReviewPopup = EditorGUILayout.Toggle("Show 'RateBox' before 'In Game Review Popup'", rb.showRateBoxWithInGameReviewPopup);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Popup Display Text", EditorStyles.boldLabel);
            rb.title = EditorGUILayout.TextField("Title", rb.title);
            rb.message = EditorGUILayout.TextField("Message", rb.message);
            rb.rateButton = EditorGUILayout.TextField("Rate Button", rb.rateButton);
            rb.postponeButton = EditorGUILayout.TextField("Postpone Button", rb.postponeButton);
            if(rb.postponeButton != ""){
                EditorGUI.indentLevel++;
                rb.postponeCooldownInMinutes = EditorGUILayout.FloatField("Postpone Cooldown In Minutes", rb.postponeCooldownInMinutes);
                EditorGUI.indentLevel--;
            }
            rb.optOutButton = EditorGUILayout.TextField("Opt Out Button", rb.optOutButton);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Update", EditorStyles.boldLabel);
            rb.showAfterUpdate = EditorGUILayout.Foldout(rb.showAfterUpdate, "Show 'RateBox' After App Update");
            if(rb.showAfterUpdate){
                EditorGUI.indentLevel++;
                rb.showAfterUpdateOnIos = EditorGUILayout.Toggle("iOS", rb.showAfterUpdateOnIos);
                rb.showAfterUpdateOnAndroid = EditorGUILayout.Toggle("Android", rb.showAfterUpdateOnAndroid);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Store Pages", EditorStyles.boldLabel);
            rb.appStoreAppId = EditorGUILayout.TextField("App Store App Id", rb.appStoreAppId);

            EditorGUILayout.Space();
            rb.rateBoxStyles = (RateBoxes) EditorGUILayout.EnumPopup("Rate Box Styles", rb.rateBoxStyles);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
            if(GUILayout.Button("Reset PlayerPrefs!")){
                rb.HardResetPrefs();
            }

            if (GUI.changed)
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            EditorUtility.SetDirty(rb);
            serializedObject.ApplyModifiedProperties();
        }

    }
}
