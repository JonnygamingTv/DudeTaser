using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DudeTaser
{
    public class Tased
    {
        [XmlAttribute] public ushort ID;
        [XmlAttribute] public float TaserTime;
        [XmlAttribute] public float MovementMultiplier;
        [XmlAttribute] public string TaseBypass;
        [XmlAttribute] public SDG.Unturned.EPlayerStance Stance;
        [XmlAttribute] public SDG.Unturned.EPlayerGesture Gesture;
        public Tased() { }
        public Tased(ushort a, float b, float m, string c, SDG.Unturned.EPlayerStance d, SDG.Unturned.EPlayerGesture e) { ID = a; TaserTime=b; MovementMultiplier = m; TaseBypass = c; Stance=d; Gesture = e; }
    }
}