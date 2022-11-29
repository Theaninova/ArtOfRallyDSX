using ArtOfRallyDSX.DSX;
using HarmonyLib;
using UnityEngine;

namespace ArtOfRallyDSX
{
    [HarmonyPatch(typeof(CarController), "FixedUpdate")]
    public class DSXCarController
    {
        public static bool Within(float a, float b, float c)
        {
            return a >= b - c && a <= b + c;
        }

        public static void Postfix(CarController __instance, Drivetrain ___drivetrain, CarDynamics ___cardynamics)
        {
            var rpmProgress = 1 - (___drivetrain.rpm - ___drivetrain.minRPM) /
                (___drivetrain.maxRPM - ___drivetrain.minRPM);
            var reachedMaxPower = ___drivetrain.rpm >= ___drivetrain.maxPowerRPM;
            DebugUI.Ffb = ___drivetrain.gear;
            var slipLerp = Mathf.Clamp((___drivetrain.wheelTireVelo - ___drivetrain.velo) / Main.Settings.SlipTolerance,
                0f, 1f);
            Main.DualSenseConnection.Send(new[]
            {
                new Instruction
                {
                    type = InstructionType.TriggerUpdate,
                    parameters = new object[]
                    {
                        0, Main.Settings.AccelerationTrigger, TriggerMode.Machine,
                        1,
                        9,
                        0,
                        Mathf.Lerp(Mathf.Clamp(Mathf.RoundToInt(rpmProgress * Main.Settings.RpmEase), 3f, 8), 0f,
                            slipLerp),
                        Mathf.Clamp(Mathf.RoundToInt(slipLerp * Main.Settings.SlipFrequency), 0, 255),
                        Mathf.Clamp(slipLerp * Main.Settings.SlipPulse, 0f, 2f),
                    }
                },
                new Instruction
                {
                    type = InstructionType.TriggerUpdate,
                    parameters = new object[]
                    {
                        0, Main.Settings.BrakeTrigger, TriggerMode.Resistance,
                        0,
                        Within(___drivetrain.wheelTireVelo, 0f, 1f) || __instance.ABSTriggered
                            ? 0
                            : Mathf.Clamp(
                                Mathf.RoundToInt((1 - ___drivetrain.wheelTireVelo / Main.Settings.BrakeSpeedEase) *
                                                 Main.Settings.BrakeForce),
                                0, 8),
                    }
                },
                new Instruction
                {
                    type = InstructionType.RGBUpdate,
                    parameters = new object[]
                    {
                        0,
                        Mathf.Clamp(Mathf.RoundToInt((1 - rpmProgress) * 255), 0, 255),
                        Mathf.Clamp(Mathf.RoundToInt(rpmProgress * 255), 0, 255),
                        0,
                    }
                },
                new Instruction
                {
                    type = InstructionType.PlayerLED,
                    parameters = new object[]
                    {
                        0,
                        ___drivetrain.gear == 1,
                        ___drivetrain.gear == 2,
                        ___drivetrain.gear == 3,
                        ___drivetrain.gear == 4,
                        ___drivetrain.gear == 5,
                    }
                },
                new Instruction
                {
                    type = InstructionType.TriggerThreshold,
                    parameters = new object[]
                    {
                        0, Trigger.Right, 0
                    }
                },
            });
        }
    }
}