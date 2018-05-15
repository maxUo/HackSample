namespace UnblockHackNET
{
    public enum Pages
    {
        Main,
        Menu,
        Settings,
        My,
        Auth,
		CustomVision,
        Carto
    }

    public enum NavigationMode
    {
        Normal,
        Modal,
        RootPage,
        Custom
    }

    public enum PageState
    {
        Clean,
        Loading,
        Normal,
        NoData,
        Error,
        NoInternet
    }
}
