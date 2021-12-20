using VContainer.Unity;

public class GamePresenter : IStartable
{
    private readonly HelloWorldService helloWorldService;
    private readonly HelloScreen helloScreen;

    public GamePresenter(HelloWorldService helloWorldService, HelloScreen helloScreen)
    {
        this.helloWorldService = helloWorldService;
        this.helloScreen = helloScreen;
    }

    void IStartable.Start()
    {
        helloScreen.HelloButton.onClick.AddListener(() => helloWorldService.Hello());
    }
}
