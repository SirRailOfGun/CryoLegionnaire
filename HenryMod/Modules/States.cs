﻿using CryoLegionnaire.SkillStates;
using CryoLegionnaire.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace CryoLegionnaire.Modules
{
    public static class States
    {
        internal static void RegisterStates()
        {
            Modules.Content.AddEntityState(typeof(BaseMeleeAttack));
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(Icethrower));
            Modules.Content.AddEntityState(typeof(ChillOut));

            Modules.Content.AddEntityState(typeof(Roll));

            Modules.Content.AddEntityState(typeof(ThrowBomb));
        }
    }
}