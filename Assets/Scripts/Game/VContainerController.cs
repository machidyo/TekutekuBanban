using Niantic.ARDK.Extensions.Meshing;
using VContainer;
using VContainer.Unity;

public class VContainerController : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<ScreenController>();
        builder.RegisterEntryPoint<GameController>();

        builder.RegisterComponentInHierarchy<ScreenViewer>();
        builder.RegisterComponentInHierarchy<ARMeshManager>();
        builder.RegisterComponentInHierarchy<NetworkManager>();
        builder.RegisterComponentInHierarchy<Operation>();
        builder.RegisterComponentInHierarchy<Question>();
    }
}
