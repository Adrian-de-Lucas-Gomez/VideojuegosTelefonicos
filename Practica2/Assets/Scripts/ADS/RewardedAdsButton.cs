using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

namespace flow
{
    public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] Button hintButton;

        [SerializeField] string _androidAdUnitId = "Rewarded_Android";
        [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
        string _adUnitId;

        //private bool addedListener = false;

        //Test
        public int pistas = 0;

        void Awake()
        {
            _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOSAdUnitId
                : _androidAdUnitId;

            hintButton.interactable = false;
        }

        // Load content to the Ad Unit:
        public void LoadAd()
        {
            // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
            Debug.Log("Loading Ad: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }

        // If the ad successfully loads, add a listener to the button and enable it:
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            Debug.Log("Ad Loaded: " + adUnitId);

            hintButton.interactable = true;

            hintButton.onClick.AddListener(ShowAd);
        }

        // Implement a method to execute when the user clicks the button.
        public void ShowAd()
        {
            Debug.Log("Ad shown with callBack");
            Advertisement.Show(_adUnitId, this);

            hintButton.interactable = false;
        }

        // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (placementId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                Debug.Log("Unity Ads Rewarded Ad Completed");
                // Grant a reward.
                //GameManager.GetInstance().OnHintAdded();

                pistas++;
                // Load another ad:
                Advertisement.Load(_adUnitId, this);
            }
        }

        //Este si que se llama en el editor
        public void OnUnityAdsShowStart(string adUnitId)
        {
            //Para probar en el editor
            //GameManager.GetInstance().OnHintAdded();

            // Load another ad:
            //Advertisement.Load(_adUnitId, this);
        }

        
        // Implement Load and Show Listener error callbacks:
        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
        }
        public void OnUnityAdsShowClick(string adUnitId) { }
    }
}