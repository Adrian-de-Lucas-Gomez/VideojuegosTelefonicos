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

        private static bool activeADS;

        public static AdvertisingManager GetInstance() { return instance; }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                DecideADS();
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            InitializeAds();
        }

        private void DecideADS()
        {
            if (!activeADS) return;

            GameManager.actualScene actScene = GameManager.GetInstance().GetActualScene();

            switch (actScene)
            {
                case GameManager.actualScene.MainMenu:
                    instance.HideBannerAd();
                    break;

                case GameManager.actualScene.SelectLevel:
                    instance.ShowBannerAd();
                    break;

                case GameManager.actualScene.PlayScene:
                    instance.ShowBannerAd();
                    break;
            }
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

        public static void DeactivateADS()
        {
            activeADS = false;
            Debug.Log("Anuncios desactivados");
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
            activeADS = true;

            //Llamamos a los distintos tipos de ADS que tenemos para que carguen el anuncio
            bannerAd.LoadBanner();
            intersticialAd.LoadAd();
            rewardedAd.LoadAd();

            ReloadADS();
        }

        public void ReloadADS()
        {
            if (!activeADS) return;

            if (GameManager.GetInstance().GetActualScene() != GameManager.actualScene.MainMenu) ShowBannerAd();

            else HideBannerAd();
            
        }

        public void ShowBannerAd()
        {
            if (!activeADS) return;
            bannerAd.ShowAd();
        }

        public void HideBannerAd()
        {
            if (!activeADS) return;
            bannerAd.HideAd();
        }

        public void ShowIntersticialAd()
        {
            if (!activeADS) return;
            //Probabilidad de que salga un anuncio de este tipo
            int rand = Random.Range(0, 5);

            if (rand == 1)
            {
                //Quitamos el banner para que no se superponga
                HideBannerAd();
                //Mostramos el anuncio intersticial
                intersticialAd.ShowAd();
            }
            else
            {
                ReloadADS();
            }
            
        }

        //Este se muestra solo usando el botón

        public void ShowRewardedAd()
        {
            //Quitamos el banner para que no se superponga
            HideBannerAd();
            //Mostramos el anuncio intersticial
            rewardedAd.ShowAd();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }



    }
}


