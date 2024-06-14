using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mohammad.VRProject.Scripts.SliderSync
{

    public class SliderSync : MonoBehaviour
    {

        [SerializeField]
        private Slider[] sliders;

        [SerializeField]
        private TextMeshProUGUI[] textBoxes;

        private Slider thisSlider;

        // Start is called before the first frame update
        void Start()
        {
            thisSlider = GetComponent<Slider>();
            foreach (TextMeshProUGUI textBox in textBoxes)
            {
                textBox.text = thisSlider.value.ToString("0.000");
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void UpdateSliders()
        {
            foreach (Slider slider in sliders)
            {
                slider.value = thisSlider.value;
            }
            foreach(TextMeshProUGUI textBox in textBoxes)
            {
                textBox.text = thisSlider.value.ToString("0.000");
            }
        }




    }

}