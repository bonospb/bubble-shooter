using DG.Tweening;
using FreeTeam.BubbleShooter.Common;
using FreeTeam.BubbleShooter.Configuration;
using FreeTeam.BubbleShooter.ECS.Components;
using FreeTeam.BubbleShooter.Services;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Assets.Scripts.ECS.Systems
{
    public sealed class PredictionViewUpdateSystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsFilterInject<Inc<Prediction, Position>> predictionFilter = default;
        private readonly EcsFilterInject<Inc<Bubble, Next>> bubbleFilter = default;

        private readonly EcsPoolInject<Bubble> bubblePool = default;
        private readonly EcsPoolInject<Position> positionPool = default;

        private readonly EcsCustomInject<ISceneContext> sceneContext = default;
        private readonly EcsCustomInject<ILevelConfig> levelConfig = default;
        #endregion

        #region Implementation
        public void Run(IEcsSystems systems)
        {
            if (predictionFilter.Value.IsEmpty() || bubbleFilter.Value.IsEmpty())
            {
                sceneContext.Value.PredicationView.Renderer.enabled = false;
                return;
            }

            sceneContext.Value.PredicationView.Renderer.enabled = true;

            foreach (var entity in bubbleFilter.Value)
            {
                var color = GetColorForNumber(bubblePool.Value.Get(entity).Value);
                color.a = sceneContext.Value.PredicationView.Renderer.color.a;
                sceneContext.Value.PredicationView.Renderer.color = color;
            }

            foreach (var entity in predictionFilter.Value)
            {
                var newPosition = Hex.ToWorldPosition(positionPool.Value.Get(entity).Value);
                var predictionTransform = sceneContext.Value.PredicationView.transform;
                if (newPosition != (Vector2)predictionTransform.position)
                {
                    predictionTransform.position = newPosition;
                    predictionTransform.DOKill();
                    predictionTransform.localScale = Vector3.one * .5f;
                    predictionTransform.DOScale(Vector3.one, .25f);
                }
            }
        }
        #endregion

        #region Private methods
        private Color GetColorForNumber(int number)
        {
            foreach (var bubbleData in levelConfig.Value.BubbleData)
            {
                if (bubbleData.Number == number)
                    return bubbleData.Color;
            }

            return default;
        }
        #endregion
    }
}
