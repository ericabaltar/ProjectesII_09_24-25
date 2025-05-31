using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueGetter : MonoBehaviour
{
    public Slider slider;
    public int Value { get { return (int)slider.value; } }
}
