namespace Services
{
    public interface ILoadingInitialization
    {
        int Priority { get; }
        void Init();
    }
}