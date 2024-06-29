using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;
public class DayNightCycle : MonoBehaviour
{
    
    public Light2D sunlight;
    public TMP_Text currentTimeText;
    //24 hours
    //Day lasts 16, night lasts 8?
    //Day = 24 minutes, night lasts 12?
    [Header ("Day/Night Cycle Settings")]
    public float dayLengthInMinutes;
    public float nightLengthInMinutes;

    private float dayLengthInSeconds;
    private float nightLengthInSeconds;

    private float totalDayTimeInMinutes;
    private float totalDayTimeInSeconds;
    private bool isDayTime;
    private float currentTime;
    private string dayState;
    private string timeSuffix;
    public void  Start() {
        sunlight = GetComponent<Light2D>();
        sunlight.intensity = 0.8f;

        dayLengthInSeconds = dayLengthInMinutes * 60;
        nightLengthInSeconds = nightLengthInMinutes * 60;
        totalDayTimeInMinutes = dayLengthInMinutes + nightLengthInMinutes;
        totalDayTimeInSeconds = dayLengthInSeconds + nightLengthInSeconds;
        isDayTime = true;
        //currentTime = totalDayTimeInMinutesInSeconds / 4; //This should set time to "6am"
        currentTime = 0f;
        dayState = "day";
        timeSuffix = "AM";
    }
    
    void Update() {
        currentTime += Time.deltaTime;

        if(currentTime > dayLengthInSeconds && isDayTime) {
            currentTime = 0f;
            isDayTime = false;
            dayState = "night";
            
        } 
        else if (currentTime > nightLengthInSeconds  && !isDayTime) {
            currentTime = 0f;
            isDayTime = true;
            dayState = "day";
        }
        
        UpdateLighting();
    }

    void UpdateLighting() {
        float timePercent = 0f;
        float sunlightIntensity = 0f;
        if(isDayTime) {
            timePercent = currentTime / dayLengthInSeconds;
            sunlightIntensity = Mathf.Sin(timePercent * Mathf.PI);
            if (sunlightIntensity < .05f) {
                sunlightIntensity = .05f;
            }
            sunlight.intensity = sunlightIntensity;
        } else {
            sunlight.intensity = 0.05f;
        }
            
    }


    



}
