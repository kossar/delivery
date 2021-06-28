namespace PublicApi.DTO.v1
{
    public class LangResources
    {
        public Views Views { get; set; } = new Views();
    }

    public class Views
    {
        public Shared Shared { get; set; } = new Shared();
        public Home Home { get; set; } = new Home();
        public LogIn LogIn { get; set; } = new LogIn();
    }

    public class Home
    {
        public string WelcomeHeading { get; set; } = Resources.Views.Home._Home.WelcomeHeading;
    }

    public class LogIn
    {
        public string Login { get; set; } = Resources.Views.Login._Login.Login;
        public string LogOut { get; set; } = Resources.Views.Login._Login.Logout;
        public string Register { get; set; } = Resources.Views.Login._Login.Register;
    }
    public class Shared
    {
        public LayOut LayOut { get; set; } = new LayOut();
    }

    public class LayOut
    {
        public string Languages { get; set; } = Resources.Views.Shared._Layout.Languages;
        public string TransportNeeds { get; set; } = Resources.Views.Shared._Layout.TransportNeeds;
        public string TransportOffers { get; set; } = Resources.Views.Shared._Layout.TransportOffers;
    }
}