using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

namespace flow
{
    public class AdvertisingManager : MonoBehaviour, IUnityAdsInitializationListener
    {
        [SerializeField]
        private InterstitialAd intersticialAd;

        [SerializeField]
        private RewardedAdsButton rewardedAd;

        [SerializeField]
        private BannerAd bannerAd;


        private static AdvertisingManager instance;

        public static AdvertisingManager GetInstance() { return instance; }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            InitializeAds();
        }

        [SerializeField] string _androidGameId;
        [SerializeField] string _iOSGameId;
        [SerializeField] bool _testMode = true;
        private string _gameId;

        public void InitializeAds()
        {
            _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOSGameId
                : _androidGameId;

            Advertisement.Initialize(_gameId, _testMode, this);
            Debug.Log("Unity Ads initialization called.");
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");

            //Llamamos a los distintos tipos de ADS que tenemos
            bannerAd.LoadBanner();
            intersticialAd.LoadAd();
            rewardedAd.LoadAd();

            ShowBannerAd();
        }

        public void ShowBannerAd()
        {
            bannerAd.ShowAd();
        }

        public void HideBannerAd()
        {
            bannerAd.HideAd();
        }

        public void ShowIntersticialAd()
        {
            //Probabilidad de que salga un anuncio de este tipo
            int rand = Random.Range(0, 5);

            if (rand == 1)
            {
                //Quitamos el banner para que no se superponga
                HideBannerAd();
                //Mostramos el anuncio intersticial
                intersticialAd.ShowAd();
            }
            
        }

        //Este se muestra solo usando el botón

        //public void ShowRewardedAd()
        //{
        //    //Quitamos el banner para que no se superponga
        //    HideBannerAd();
        //    //Mostramos el anuncio intersticial
        //    rewardedAd.ShowAd();
        //}

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }



    }
}


