public static class EventConst {
    public const string UPDATE_WORLD_POS_AIM = "UpdateWorldPositionAim_";
    public const string UPDATE_UI_POS_AIM = "UpdateUIPositionAim_";
    public const string UPDATE_AIM_TARGET_IN_SIGHT = "UpdateAimTargetInSight_";

    public const string UPDATE_PLAYER_STATS = "UpdatePlayerStats_";
    public const string UPDATE_PLAYER_HEALTH = "UpdatePlayerHealth_";
    public const string UPDATE_PLAYER_FUEL = "UpdatePlayerFuel_";

 
    public static string GetUpdateWorldPosAim(int id)
    {
        return UPDATE_WORLD_POS_AIM + id;
    }
    public static string GetUpdateUIPosAim(int id)
    {
        return UPDATE_UI_POS_AIM + id;
    }

    public static string GetUpdateAimTargetInSight(int id)
    {
        return UPDATE_AIM_TARGET_IN_SIGHT + id;
    }

    public static string GetUpdatePlayerStats(int id)
    {
        return UPDATE_PLAYER_STATS + id;
    }

    public static string GetUpdatePlayerHealth(int id)
    {
        return UPDATE_PLAYER_HEALTH + id;
    }

    public static string GetUpdatePlayerFuel(int id)
    {
        return UPDATE_PLAYER_FUEL + id;
    }

}
