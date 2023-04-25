using R2API;
using System;

namespace CryoLegionnaire.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            #region Henry
            //string desc = "Henry is a skilled fighter who makes use of a wide arsenal of weaponry to take down his foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            //desc = desc + "< ! > Sword is a good all-rounder while Boxing Gloves are better for laying a beatdown on more powerful foes." + Environment.NewLine + Environment.NewLine;
            //desc = desc + "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine;
            //desc = desc + "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine;
            //desc = desc + "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

            //string outro = "..and so he left, searching for a new identity.";
            //string outroFailure = "..and so he vanished, forever a blank slate.";

            //LanguageAPI.Add(prefix + "NAME", "Henry");
            //LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            //LanguageAPI.Add(prefix + "SUBTITLE", "The Chosen One");
            //LanguageAPI.Add(prefix + "LORE", "sample lore");
            //LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            //LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            //#region Skins
            //LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            //LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            //#endregion

            //#region Passive
            //LanguageAPI.Add(prefix + "PASSIVE_NAME", "Henry passive");
            //LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            //#endregion

            //#region Primary
            //LanguageAPI.Add(prefix + "PRIMARY_SLASH_NAME", "Sword");
            //LanguageAPI.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Helpers.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * StaticValues.swordDamageCoefficient}% damage</style>.");
            //#endregion

            //#region Secondary
            //LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Handgun");
            //LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Helpers.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style>.");
            //#endregion

            //#region Utility
            //LanguageAPI.Add(prefix + "UTILITY_ROLL_NAME", "Roll");
            //LanguageAPI.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
            //#endregion

            //#region Special
            //LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Bomb");
            //LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * StaticValues.bombDamageCoefficient}% damage</style>.");
            //#endregion

            //#region Achievements
            //LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Henry: Mastery");
            //LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Henry, beat the game or obliterate on Monsoon.");
            //LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Henry: Mastery");
            //#endregion
            #endregion

            #region Cryo Legionnaire
            string prefix = CryoLegionnaire.DEVELOPER_PREFIX + "_CRYO_BODY_";

            string desc = "Henry is a skilled fighter who makes use of a wide arsenal of weaponry to take down his foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Sword is a good all-rounder while Boxing Gloves are better for laying a beatdown on more powerful foes." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so they left, disappointed by the chilly reception.";
            string outroFailure = "..and so they vanished, frozen in memory.";

            LanguageAPI.Add(prefix + "NAME", "Cryo Legionnaire");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "The Cold Shoulder");
            LanguageAPI.Add(prefix + "LORE", "sample lore");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Keywords
            LanguageAPI.Add("KEYWORD_CHILL", "[ Chill ]\nApplies a stack of chill on hit, slowing enemies by 5% per stack, and freezing at 20 stacks.\nBosses are slowed half as much");
            LanguageAPI.Add("KEYWORD_CHILL3", "[ Chill ]\nApplies three stacks of chill on hit, slowing enemies by 5% per stack, and freezing at 20 stacks.\nBosses are slowed half as much");
            LanguageAPI.Add("KEYWORD_CHILL5", "[ Chill ]\nApplies five stacks of chill on hit, slowing enemies by 5% per stack, and freezing at 20 stacks.\nBosses are slowed half as much");
            LanguageAPI.Add("KEYWORD_EXECUTE", $"[ Execute ]\nDeal an addtional <style=cIsDamage>{100f * Modules.StaticValues.executeDamageCoefficient}% damage</style> hit in a small area.");
            #endregion

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Chill");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Cryo Legionnaire's attacks apply chill, which slows enemies by 5% per stack, and has half strength against bosses. Chill will freeze foes at 20 stacks.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_BEAM_NAME", "Thermal Inversion Cannon");
            LanguageAPI.Add(prefix + "PRIMARY_BEAM_DESCRIPTION", Helpers.chillPrefix + $"Fire a laser for <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_SLASH_NAME", "Cryo Cannon");
            LanguageAPI.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Helpers.chillPrefix + $"Sweep your cryocannon forwards for <style=cIsDamage>{100f * StaticValues.swordDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Chill Out!");
            LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Helpers.chillPrefix + $"Erupt in a burst of extreme cold, Chilling enemies five times in a wide area. Nearby enemies are hit again for <style=cIsDamage>{100f * StaticValues.burstDamageCoefficient}% damage</style> and chilled thrice more.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_CHARGE_NAME", "Icebreaker");
            LanguageAPI.Add(prefix + "UTILITY_CHARGE_DESCRIPTION", $"Crash through foes, dealing <style=cIsDamage>{100f * StaticValues.bashDamageCoefficient}% damage</style> and <style=cIsDamage>executing</style> frozen foes");

            LanguageAPI.Add(prefix + "UTILITY_JUMP_NAME", "Avalanche");
            LanguageAPI.Add(prefix + "UTILITY_JUMP_DESCRIPTION", Helpers.heavyPrefix + "Launch yourself high into the air and become immune to the fall. Damage foes near the landing zone.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Cold Front");
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", Helpers.chillPrefix + $"Throw a bomb for <style=cIsDamage>{100f * StaticValues.bombDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Cryo Legionnaire: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Cryo Legionnaire, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Cryo Legionnaire: Mastery");
            #endregion
            #endregion
        }
    }
}