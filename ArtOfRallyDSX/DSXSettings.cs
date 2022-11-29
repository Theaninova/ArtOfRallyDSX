using ArtOfRallyDSX.DSX;
using UnityEngine;
using UnityModManagerNet;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable ConvertToConstant.Global

namespace ArtOfRallyDSX
{
    public class DsxSettings : UnityModManager.ModSettings, IDrawable
    {
        [Header("Acceleration")] [Draw]
        public Trigger AccelerationTrigger = Trigger.Right;

        [Draw(DrawType.Slider, Min = 0f, Max = 8f)]
        public float RpmEase = 5f;
        
        [Draw(DrawType.Slider, Min = 0f, Max = 20f)]
        public float SlipTolerance = 5f;
        
        [Draw(DrawType.Slider, Min = 0, Max = 40)]
        public int SlipFrequency = 32;

        [Draw(DrawType.Slider, Min = -2f, Max = 2f)]
        public float SlipPulse = 2f;

        [Header("Brake Trigger")] [Draw] public Trigger BrakeTrigger = Trigger.Left;
        
        [Draw(DrawType.Slider, Min = 0, Max = 8)]
        public int BrakeForce = 6;

        [Draw(DrawType.Slider, Min = 0f, Max = 200f)]
        public float BrakeSpeedEase = 100f;
        
        [Draw] public bool DrawDebugUI = false;

        public void OnChange()
        {
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}