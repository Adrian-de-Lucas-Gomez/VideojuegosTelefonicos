using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

namespace flow
{
    public class BannerAd : MonoBehaviour
    {
        [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

        [SerializeField] string _androidAdUnitId = "Banner_Android";
        [SerializeField] string _iOsAdUnitId = "Banner_iOS";
        string _adUnitId;

        private bool firstLoad = false;

        void Start()
        {
            _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOsAdUnitId
                : _androidAdUnitId;

            // Set the banner position:
            Advertisement.Banner.SetPosition(_bannerPosition);
        }

        public void Update()
        {
            if (Advertisement.IsReady() && !firstLoad)
            {
                LoadBanner();
                firstLoad = true;
            }
        }

        // Implement a method to call when the Load Banner button is clicked:
        public void LoadBanner()
        {
            // Set up options to notify the SDK of load events:
            BannerLoadOptions options = new BannerLoadOptions
            {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerError
            };

            // Load the Ad Unit with banner content:
            Advertisement.Banner.Load(_adUnitId, options);
        }

        // Implement code to execute when the loadCallback event triggers:
        void OnBannerLoaded()
        {
            Debug.Log("Banner loaded");

            //Test
            ShowBannerAd();
        }

        // Implement code to execute when the load errorCallback event triggers:
        void OnBannerError(string message)
        {
            Debug.Log($"Banner Error: {message}");
            // Optionally execute additional code, such as attempting to load another ad.
        }

        // Implement a method to call when the Show Banner button is clicked:
        void ShowBannerAd()
        {
            // Set up options to notify the SDK of show events:
            BannerOptions options = new BannerOptions
            {
                clickCallback = OnBannerClicked,
                hideCallback = OnBannerHidden,
                showCallback = OnBannerShown
            };

            // Show the loaded Banner Ad Unit:
            Advertisement.Banner.Show(_adUnitId, options);
        }

        // Implement a method to call when the Hide Banner button is clicked:
        void HideBannerAd()
        {
            // Hide the banner:
            Advertisement.Banner.Hide();
        }

        void OnBannerClicked() { }
        void OnBannerShown() { }
        void OnBannerHidden() { }
    }
}