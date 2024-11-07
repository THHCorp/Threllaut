namespace Threllaut;

public partial class App : Application
{
    public App() => this.InitializeComponent();

    protected override Window CreateWindow(IActivationState? activationState) =>
        new(new MainPage());
}
