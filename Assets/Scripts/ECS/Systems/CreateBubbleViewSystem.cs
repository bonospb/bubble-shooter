using DG.Tweening;
using FreeTeam.BubbleShooter.Common;
using FreeTeam.BubbleShooter.Configuration;
using FreeTeam.BubbleShooter.ECS.Components;
using FreeTeam.BubbleShooter.Services;
using FreeTeam.BubbleShooter.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace FreeTeam.BubbleShooter.ECS.Systems
{
    public sealed class CreateBubbleViewSystem : IEcsRunSystem
    {
        #region Inject
        private readonly EcsFilterInject<Inc<Bubble>, Exc<UnityObject<BubbleView>>> bubblesFilter = default;

        private readonly EcsPoolInject<Bubble> bubblePool = default;
        private readonly EcsPoolInject<Position> positionPool = default;
        private readonly EcsPoolInject<WorldPosition> worldPositionPool = default;
        private readonly EcsPoolInject<UnityObject<BubbleView>> bubbleViewPool = default;

        private readonly EcsCustomInject<ILevelConfig> levelConfig = default;
        private readonly EcsCustomInject<ISceneContext> sceneContext = default;
        #endregion

        #region Implementation
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in bubblesFilter.Value)
            {
                var value = bubblePool.Value.Get(entity).Value;

                var isOnGrid = false;
                Vector3 worldPosition;
                Vector2Int pos = default;

                if (positionPool.Value.Has(entity))
                {
                    pos = positionPool.Value.Get(entity).Value;
                    worldPosition = Hex.ToWorldPosition(pos);
                    isOnGrid = true;
                }
                else if (worldPositionPool.Value.Has(entity))
                {
                    worldPosition = worldPositionPool.Value.Get(entity).Value;
                    worldPositionPool.Value.Del(entity);
                }
                else
                {
                    continue;
                }

                var view = Object.Instantiate(levelConfig.Value.BubbleView, worldPosition, Quaternion.identity, sceneContext.Value.BubbleViewContainer);
                view.Renderer.color = GetColorForValue(value);
                view.SetText($"{pos.x}/{pos.y}");

                var duration = isOnGrid ? Random.Range(0.4f, 0.7f) : 0.25f;

                view.transform.localScale = Vector3.zero;
                view.transform.DOScale(Vector3.one, duration);

                bubbleViewPool.Value.Add(entity).Value = view;
            }
        }
        #endregion

        #region Private
        private Color GetColorForValue(int value)
        {
            foreach (var data in levelConfig.Value.BubbleData)
            {
                if (data.Number == value)
                    return data.Color;
            }

            return Color.white;
        }
        #endregion
    }
}
