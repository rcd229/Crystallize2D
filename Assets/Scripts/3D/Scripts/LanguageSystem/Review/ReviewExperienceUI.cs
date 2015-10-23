using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic;

public class ReviewExperienceUI : MonoBehaviour {

    public Text levelText;
    public Text experienceText;
    public Image barImage;

    float experience = 0;

    void Start() {
        PlayerData.Instance.Proficiency.RecalculateExperience();
        experience = PlayerData.Instance.Proficiency.ReviewExperience;
    }

    void Update() {
        experience = Mathf.MoveTowards(experience, PlayerData.Instance.Proficiency.ReviewExperience, 10f * Time.deltaTime);
        var level = ProficiencyPlayerData.GetReviewLevel((int)experience);
        var currentLevelExp = ProficiencyPlayerData.GetReviewLevelExperience((int)experience);
        var nextLevelExp = ProficiencyPlayerData.GetNextLevelExperienceFromExperience((int)experience);
        levelText.text = string.Format("{0}", level);
        experienceText.text = string.Format("{0}/{1}", currentLevelExp, nextLevelExp);
        barImage.fillAmount = ((experience - Mathf.Floor(experience)) + (float)currentLevelExp) / nextLevelExp;
    }

}
