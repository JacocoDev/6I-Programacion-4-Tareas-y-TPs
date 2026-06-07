using UnityEngine;

public static class CombatSystem
{
    public static void ResolveTurn(ActionType boxer1Action, ActionType boxer1PreviousAction, ActionType boxer2Action, ActionType boxer2PreviousAction, Boxer boxer1, Boxer boxer2, int punchDamage, out bool boxer1TookDamage, out bool boxer2TookDamage)
    {
        boxer1TookDamage = false;
        boxer2TookDamage = false;

        if (boxer1Action == ActionType.Punch)
        {
            int damage = GetAttackDamage(boxer1Action, boxer1PreviousAction, punchDamage);
            int finalDamage = GetDamageAgainstDefense(damage, boxer2Action);

            if (finalDamage > 0)
            {
                boxer2.TakeDamage(finalDamage);
                boxer2TookDamage = true;
            }
        }

        if (boxer2Action == ActionType.Punch)
        {
            int damage = GetAttackDamage(boxer2Action, boxer2PreviousAction, punchDamage);
            int finalDamage = GetDamageAgainstDefense(damage, boxer1Action);

            if (finalDamage > 0)
            {
                boxer1.TakeDamage(finalDamage);
                boxer1TookDamage = true;
            }
        }
    }

    private static int GetAttackDamage(ActionType action, ActionType previousAction, int punchDamage)
    {
        if (action != ActionType.Punch)
            return 0;

        if (previousAction == ActionType.Dodge)
            return punchDamage * 2;

        return punchDamage;
    }

    private static int GetDamageAgainstDefense(int attackDamage, ActionType defenderAction)
    {
        if (defenderAction == ActionType.Dodge)
            return 0;

        if (defenderAction == ActionType.Block)
            return Mathf.CeilToInt(attackDamage / 2f);

        return attackDamage;
    }
}