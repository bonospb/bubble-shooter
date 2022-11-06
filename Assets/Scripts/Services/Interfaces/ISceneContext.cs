using FreeTeam.BubbleShooter.Views;
using UnityEngine;

namespace FreeTeam.BubbleShooter.Services
{
    internal interface ISceneContext
    {
        Camera Camera { get; }
        SpriteRenderer Background { get; }
        Transform BubbleViewContainer { get; }
        LineRenderer TrajectoryRenderer { get; }
        PredicationView PredicationView { get; }
        ParticleSystem DestroyParticles { get; }
        EdgeCollider2D EdgeCollider { get; }
    }
}
