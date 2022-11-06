using System;

namespace FreeTeam.BubbleShooter.Services
{
    public sealed class RandomService : IRandomService
    {
        #region Private
        private readonly Random _rng;
        #endregion

        public RandomService(int? seed)
        {
            _rng = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        #region Public methods
        public int Range(int min, int max) =>
            _rng.Next(min, max);

        public float Range(float min, float max) =>
            min + (max - min) * (float)_rng.NextDouble();
        #endregion
    }
}
