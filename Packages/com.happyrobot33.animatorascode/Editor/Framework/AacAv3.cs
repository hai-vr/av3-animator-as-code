// ReSharper disable once CheckNamespace
namespace AnimatorAsCode.Framework
{
    /// <summary>
    /// AacAv3 is a class that provides access to specifics of the VRChat SDK3 avatar system.
    /// </summary>
    public partial class AacAv3
    {
        private readonly AacBackingAnimator _backingAnimator;

        internal AacAv3(AacBackingAnimator backingAnimator)
        {
            _backingAnimator = backingAnimator;
        }

        // ReSharper disable InconsistentNaming
        public AacFlBoolParameter IsLocal => _backingAnimator.BoolParameter("IsLocal");
        public AacFlEnumIntParameter<Av3Viseme> Viseme =>
            _backingAnimator.EnumParameter<Av3Viseme>("Viseme");
        public AacFlEnumIntParameter<Av3Gesture> GestureLeft =>
            _backingAnimator.EnumParameter<Av3Gesture>("GestureLeft");
        public AacFlEnumIntParameter<Av3Gesture> GestureRight =>
            _backingAnimator.EnumParameter<Av3Gesture>("GestureRight");
        public AacFlFloatParameter GestureLeftWeight =>
            _backingAnimator.FloatParameter("GestureLeftWeight");
        public AacFlFloatParameter GestureRightWeight =>
            _backingAnimator.FloatParameter("GestureRightWeight");
        public AacFlFloatParameter AngularY => _backingAnimator.FloatParameter("AngularY");
        public AacFlFloatParameter VelocityX => _backingAnimator.FloatParameter("VelocityX");
        public AacFlFloatParameter VelocityY => _backingAnimator.FloatParameter("VelocityY");
        public AacFlFloatParameter VelocityZ => _backingAnimator.FloatParameter("VelocityZ");
        public AacFlFloatParameter Upright => _backingAnimator.FloatParameter("Upright");
        public AacFlBoolParameter Grounded => _backingAnimator.BoolParameter("Grounded");
        public AacFlBoolParameter Seated => _backingAnimator.BoolParameter("Seated");
        public AacFlBoolParameter AFK => _backingAnimator.BoolParameter("AFK");
        public AacFlIntParameter TrackingType => _backingAnimator.IntParameter("TrackingType");
        public AacFlIntParameter VRMode => _backingAnimator.IntParameter("VRMode");
        public AacFlBoolParameter MuteSelf => _backingAnimator.BoolParameter("MuteSelf");
        public AacFlBoolParameter InStation => _backingAnimator.BoolParameter("InStation");
        public AacFlFloatParameter Voice => _backingAnimator.FloatParameter("Voice");

        // ReSharper restore InconsistentNaming

        public IAacFlCondition ItIsRemote() => IsLocal.IsFalse();

        public IAacFlCondition ItIsLocal() => IsLocal.IsTrue();

        public enum Av3Gesture
        {
            // Specify all the values explicitly because they should be dictated by VRChat, not enumeration order.
            Neutral = 0,
            Fist = 1,
            HandOpen = 2,
            Fingerpoint = 3,
            Victory = 4,
            RockNRoll = 5,
            HandGun = 6,
            ThumbsUp = 7
        }

        public enum Av3Viseme
        {
            // Specify all the values explicitly because they should be dictated by VRChat, not enumeration order.
            // ReSharper disable InconsistentNaming
            sil = 0,
            pp = 1,
            ff = 2,
            th = 3,
            dd = 4,
            kk = 5,
            ch = 6,
            ss = 7,
            nn = 8,
            rr = 9,
            aa = 10,
            e = 11,
            ih = 12,
            oh = 13,
            ou = 14
            // ReSharper restore InconsistentNaming
        }
    }
}
