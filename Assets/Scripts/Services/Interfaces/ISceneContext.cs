using FreeTeam.BubbleShooter.Views;
using UnityEngine;

namespace FreeTeam.BubbleShooter.Services
{
    internal interface ISceneContext
    {
        Camera Camera { get; }
        SpriteRenderer Background { get; }
        Transform BubbleViewRoot { get; }
        LineRenderer TrajectoryRenderer { get; }
        PredicationView PredicationView { get; }
        ParticleSystem DestroyParticles { get; }
    }
}
