using System.Linq;
using System.Media;
using System.Reflection;

namespace DashboardHardwareChecker.Helpers
{
    public static class PlaySound
    {
        public static void PlaySoundFromResource(SoundType soundType = SoundType.Success)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var audio = Assembly.GetExecutingAssembly()
                .GetManifestResourceNames()
                .Where(x => x.EndsWith(".wav"))
                .ToList();

            switch (soundType)
            {
                case SoundType.Success:
                {
                    using var stream =
                        assembly.GetManifestResourceStream("DashboardHardwareChecker.Resources.top_gun.wav");
                    {
                        var player = new SoundPlayer(stream);
                        player.Play();
                    }
                }
                    break;

                //case SoundType.Error:
                //{
                //    using var stream =
                //        assembly.GetManifestResourceStream("ClearDashboard.Wpf.Application.Resources.DashboardFailure.wav");
                //    {
                //        var player = new SoundPlayer(stream);
                //        player.Play();
                //    }
                //}
                //    break;
            }
        }
    }

    public enum SoundType
    {
        Success,
        Error
    }
}
