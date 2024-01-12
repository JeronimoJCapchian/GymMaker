using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Interactiva.UI.Utilities {
    public class SponsorSystem : MonoBehaviour
    {
        [SerializeField] GameObject sponsorPrefab;
        [SerializeField] GameObject donorPrefab;


        [SerializeField] Sprite[] logos;
        [SerializeField] string[] urls;

        [SerializeField] string[] donors;

        
        private void Start()
        {
            /*
            for (int i = 0; i < logos.Length; i++)
            {
                GameObject sponsor = Instantiate(sponsorPrefab);
                sponsor.transform.SetParent(transform);
                sponsor.transform.Find("Logo").GetComponent<Image>().sprite = logos[i];
                if (i < urls.Length && !string.IsNullOrEmpty(urls[i]))
                {
                    sponsor.GetComponent<URLOpener>().href = urls[i];
                    sponsor.transform.Find("URLIcon").gameObject.SetActive(true);
                } else
                {
                    sponsor.transform.Find("URLIcon").gameObject.SetActive(false);
                }
            }
            for (int i = 0; i < donors.Length; i++)
            {
                GameObject donor = Instantiate(donorPrefab);
                donor.transform.SetParent(transform);
                donor.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = donors[i];
            }*/
            Destroy(this);
        }

        private void OnValidate()
        {
            //Start();
        }

    } 
}
