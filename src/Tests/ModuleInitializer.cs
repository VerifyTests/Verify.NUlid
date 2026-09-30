public static class ModuleInitializer
{
    #region Initialize

    [ModuleInitializer]
    public static void Init() =>
        VerifyNUlid.Initialize();

    #endregion

    [ModuleInitializer]
    public static void InitOther()
    {
        VerifierSettings.InitializePlugins();
    }
}