using System;
using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.Framework
{
    public partial class AacVrcAssetLibrary
    {
        public AvatarMask LeftHandAvatarMask()
        {
            return AssetDatabase.LoadAssetAtPath<AvatarMask>(
                "Assets/VRCSDK/Examples3/Animation/Masks/vrc_Hand Left.mask"
            );
        }

        public AvatarMask RightHandAvatarMask()
        {
            return AssetDatabase.LoadAssetAtPath<AvatarMask>(
                "Assets/VRCSDK/Examples3/Animation/Masks/vrc_Hand Right.mask"
            );
        }

        public AnimationClip ProxyForGesture(AacAv3.Av3Gesture gesture, bool masculine)
        {
            return AssetDatabase.LoadAssetAtPath<AnimationClip>(
                "Assets/VRCSDK/Examples3/Animation/ProxyAnim/"
                    + ResolveProxyFilename(gesture, masculine)
            );
        }

        private static string ResolveProxyFilename(AacAv3.Av3Gesture gesture, bool masculine)
        {
            switch (gesture)
            {
                case AacAv3.Av3Gesture.Neutral:
                    return masculine ? "proxy_hands_idle.anim" : "proxy_hands_idle2.anim";
                case AacAv3.Av3Gesture.Fist:
                    return "proxy_hands_fist.anim";
                case AacAv3.Av3Gesture.HandOpen:
                    return "proxy_hands_open.anim";
                case AacAv3.Av3Gesture.Fingerpoint:
                    return "proxy_hands_point.anim";
                case AacAv3.Av3Gesture.Victory:
                    return "proxy_hands_peace.anim";
                case AacAv3.Av3Gesture.RockNRoll:
                    return "proxy_hands_rock.anim";
                case AacAv3.Av3Gesture.HandGun:
                    return "proxy_hands_gun.anim";
                case AacAv3.Av3Gesture.ThumbsUp:
                    return "proxy_hands_thumbs_up.anim";
                default:
                    throw new ArgumentOutOfRangeException(nameof(gesture), gesture, null);
            }
        }
    }
}
