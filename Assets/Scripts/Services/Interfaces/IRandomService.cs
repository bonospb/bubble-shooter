namespace FreeTeam.BubbleShooter.Services
{
    public interface IRandomService
    {
        int Range(int min, int max);
        float Range(float min, float max);
    }
}
