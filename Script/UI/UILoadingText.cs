using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace ArcherGame
{
    public class UILoadingText : MonoBehaviour
    {
        TextMeshProUGUI textLoadingVal;
        void Start()
        {
            textLoadingVal = GetComponentInChildren<TextMeshProUGUI>();
            textLoadingVal.text = "Loading... 0%";
        }


        void Update()
        {
            UpdateTextLoadingVal();

        }

        public void UpdateTextLoadingVal()
        {
            Transform topBar = transform.Find("TopBar");

            Image topbarImage = topBar.GetComponent<Image>();
            if (topbarImage != null)
            {
                float val = topbarImage.fillAmount;
                textLoadingVal.text = $"Loading... {val * 100:F2}%";
            }


        }
    }
}