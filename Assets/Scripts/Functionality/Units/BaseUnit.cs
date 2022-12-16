using UnityEngine;

// This class has basic functionality for dealing with units
public class BaseUnit
{

    // Calculate HP based on profile
    public static int calculateHP(UnitProfile profile) {
        int newHP = 2 * profile.baseHP * profile.lvl / 100;
		newHP +=  10 + profile.lvl;
        newHP *= profile.modHP; // Multiply by modifier
        return newHP;
    }

    // Give experience to a unit, returns true if they leveled up
    public static bool receiveExperience(UnitProfile profile, int exp) {
        var leveledUp = false;
        var expToNextLevel = getExpNeededToLevel(profile.lvl);
        profile.currentExperience += exp;
        if (profile.currentExperience >= expToNextLevel) {
            profile.currentExperience -= expToNextLevel;
            profile.lvl++; // This could be refactored actually
            leveledUp = true;
            expToNextLevel = getExpNeededToLevel(profile.lvl);
        }

        //Update experience percent
        profile.experiencePercent = (float)profile.currentExperience / (float)expToNextLevel;
        return leveledUp;
    }

    public static int getExpNeededToLevel(int level) {
        int expNeeded = (int)Mathf.Pow(level, 3); // Med Fast for everyone!
        return expNeeded;
    }

    // Calculate experience based on profile
    public static int calculateExperience(UnitProfile profile, int totalHitMe, int highestLevelHitMe) {
        if (profile.baseExperience == 0) return 0;

        // Bonus if the unit cannot be captured
        float isWildBonus = profile.canCaptureMe ? 1.0f : 1.5f;
        int level = profile.lvl;
        int baseExp = profile.baseExperience;
        // See https://bulbapedia.bulbagarden.net/wiki/Experience for calculation
        int calc1 = (int)(isWildBonus * baseExp * level);
        int calc2 = calc1 / (7 * totalHitMe);
        int calc3 = calc2 + 1;

        int totalExp = calc3;
        return totalExp;
    }
}
