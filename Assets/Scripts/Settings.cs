public static class Settings
{
    public static bool GameIsPaused { get; set; }
    public static bool ShowArrowHint { get; set; }
    public static bool HardModeActivated { get; set; }
    public static bool ShowBalanceFactor { get; set; }
    public static float Volume { get; set; }

    public static bool isTutorial { get; set; }
    public static bool isSandbox { get; set; }

    //To run only once
    public static bool DidItRun = false;
}