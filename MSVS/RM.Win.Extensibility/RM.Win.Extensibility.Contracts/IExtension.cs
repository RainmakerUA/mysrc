
namespace RM.Win.Extensibility.Contracts
{
    public interface IExtension<in TApp>
    {
	    void OnInitializing(TApp app);

	    void OnInitialized();

	    void OnExecute();

	    void OnUnloading();
    }
}
