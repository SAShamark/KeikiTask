using UI.Screens;

namespace Services.Scenes
{
    public interface ISceneLoader
    {
        public SceneType SceneType { get; }

        public void LoadScene(SceneType sceneType, ScreenTypes nextScreen = 0, bool isLoading = true);
    }
}