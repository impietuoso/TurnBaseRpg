using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Drafts.Editor
{
    public static class ParticleSystemSetTextureSheetSprites
    {
        private static ParticleSystem _target;

        [MenuItem("Amigo/Particle/Set Texture Sheet Sprites")]
        static void SetTextureSheetSprites()
        {
            if (Selection.activeObject is GameObject go && go.TryGetComponent(out ParticleSystem ps))
            {
                _target = ps;
                Debug.Log("ParticleSystem target set");
                return;
            }

            if (!_target) throw new Exception("Choose a ParticleSystem target first");

            var sprites = Selection.objects.OfType<Sprite>().ToArray();
            if (!sprites.Any()) return;

            while (_target.textureSheetAnimation.spriteCount > 0)
                _target.textureSheetAnimation.RemoveSprite(0);

            foreach (var sprite in sprites)
                _target.textureSheetAnimation.AddSprite(sprite);

            Debug.Log("Sprites set");
            _target = null;
        }
    }
}