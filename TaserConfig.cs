using Rocket.API;

namespace DudeTaser
{
    public class TaserConfig : IRocketPluginConfiguration
    {
        public ushort TaserId;
        public float TasedTime;
        public float MovementMultiplier;
        public string NoTasePerm;
        public SDG.Unturned.EPlayerStance TasedStance;
        public SDG.Unturned.EPlayerGesture TasedGesture;
        public System.Collections.Generic.List<Tased> TasedL { get; set; }

        public void LoadDefaults()
        {
            TaserId = 63026;
            TasedTime = 5f;
            MovementMultiplier = 0.1f;
            NoTasePerm = "taserbypass";
            TasedStance = SDG.Unturned.EPlayerStance.PRONE;
            TasedGesture = SDG.Unturned.EPlayerGesture.SURRENDER_START;
            TasedL = new System.Collections.Generic.List<Tased>() { new Tased(TaserId,TasedTime,MovementMultiplier,NoTasePerm,TasedStance,TasedGesture) };
        }
    }
}
