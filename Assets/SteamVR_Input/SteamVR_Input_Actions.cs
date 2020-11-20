//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Valve.VR
{
    using System;
    using UnityEngine;
    
    
    public partial class SteamVR_Actions
    {
        
        private static SteamVR_Action_Pose p_default_Pose;
        
        private static SteamVR_Action_Vector2 p_default_Pitch;
        
        private static SteamVR_Action_Boolean p_default_RightTrigger;
        
        private static SteamVR_Action_Vector2 p_default_Modulation;
        
        private static SteamVR_Action_Boolean p_default_TrackPadClick;
        
        private static SteamVR_Action_Boolean p_default_LeftTrigger;
        
        private static SteamVR_Action_Vibration p_default_Haptic;
        
        public static SteamVR_Action_Pose default_Pose
        {
            get
            {
                return SteamVR_Actions.p_default_Pose.GetCopy<SteamVR_Action_Pose>();
            }
        }
        
        public static SteamVR_Action_Vector2 default_Pitch
        {
            get
            {
                return SteamVR_Actions.p_default_Pitch.GetCopy<SteamVR_Action_Vector2>();
            }
        }
        
        public static SteamVR_Action_Boolean default_RightTrigger
        {
            get
            {
                return SteamVR_Actions.p_default_RightTrigger.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Vector2 default_Modulation
        {
            get
            {
                return SteamVR_Actions.p_default_Modulation.GetCopy<SteamVR_Action_Vector2>();
            }
        }
        
        public static SteamVR_Action_Boolean default_TrackPadClick
        {
            get
            {
                return SteamVR_Actions.p_default_TrackPadClick.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean default_LeftTrigger
        {
            get
            {
                return SteamVR_Actions.p_default_LeftTrigger.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Vibration default_Haptic
        {
            get
            {
                return SteamVR_Actions.p_default_Haptic.GetCopy<SteamVR_Action_Vibration>();
            }
        }
        
        private static void InitializeActionArrays()
        {
            Valve.VR.SteamVR_Input.actions = new Valve.VR.SteamVR_Action[] {
                    SteamVR_Actions.default_Pose,
                    SteamVR_Actions.default_Pitch,
                    SteamVR_Actions.default_RightTrigger,
                    SteamVR_Actions.default_Modulation,
                    SteamVR_Actions.default_TrackPadClick,
                    SteamVR_Actions.default_LeftTrigger,
                    SteamVR_Actions.default_Haptic};
            Valve.VR.SteamVR_Input.actionsIn = new Valve.VR.ISteamVR_Action_In[] {
                    SteamVR_Actions.default_Pose,
                    SteamVR_Actions.default_Pitch,
                    SteamVR_Actions.default_RightTrigger,
                    SteamVR_Actions.default_Modulation,
                    SteamVR_Actions.default_TrackPadClick,
                    SteamVR_Actions.default_LeftTrigger};
            Valve.VR.SteamVR_Input.actionsOut = new Valve.VR.ISteamVR_Action_Out[] {
                    SteamVR_Actions.default_Haptic};
            Valve.VR.SteamVR_Input.actionsVibration = new Valve.VR.SteamVR_Action_Vibration[] {
                    SteamVR_Actions.default_Haptic};
            Valve.VR.SteamVR_Input.actionsPose = new Valve.VR.SteamVR_Action_Pose[] {
                    SteamVR_Actions.default_Pose};
            Valve.VR.SteamVR_Input.actionsBoolean = new Valve.VR.SteamVR_Action_Boolean[] {
                    SteamVR_Actions.default_RightTrigger,
                    SteamVR_Actions.default_TrackPadClick,
                    SteamVR_Actions.default_LeftTrigger};
            Valve.VR.SteamVR_Input.actionsSingle = new Valve.VR.SteamVR_Action_Single[0];
            Valve.VR.SteamVR_Input.actionsVector2 = new Valve.VR.SteamVR_Action_Vector2[] {
                    SteamVR_Actions.default_Pitch,
                    SteamVR_Actions.default_Modulation};
            Valve.VR.SteamVR_Input.actionsVector3 = new Valve.VR.SteamVR_Action_Vector3[0];
            Valve.VR.SteamVR_Input.actionsSkeleton = new Valve.VR.SteamVR_Action_Skeleton[0];
            Valve.VR.SteamVR_Input.actionsNonPoseNonSkeletonIn = new Valve.VR.ISteamVR_Action_In[] {
                    SteamVR_Actions.default_Pitch,
                    SteamVR_Actions.default_RightTrigger,
                    SteamVR_Actions.default_Modulation,
                    SteamVR_Actions.default_TrackPadClick,
                    SteamVR_Actions.default_LeftTrigger};
        }
        
        private static void PreInitActions()
        {
            SteamVR_Actions.p_default_Pose = ((SteamVR_Action_Pose)(SteamVR_Action.Create<SteamVR_Action_Pose>("/actions/default/in/Pose")));
            SteamVR_Actions.p_default_Pitch = ((SteamVR_Action_Vector2)(SteamVR_Action.Create<SteamVR_Action_Vector2>("/actions/default/in/Pitch")));
            SteamVR_Actions.p_default_RightTrigger = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/RightTrigger")));
            SteamVR_Actions.p_default_Modulation = ((SteamVR_Action_Vector2)(SteamVR_Action.Create<SteamVR_Action_Vector2>("/actions/default/in/Modulation")));
            SteamVR_Actions.p_default_TrackPadClick = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/TrackPadClick")));
            SteamVR_Actions.p_default_LeftTrigger = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/LeftTrigger")));
            SteamVR_Actions.p_default_Haptic = ((SteamVR_Action_Vibration)(SteamVR_Action.Create<SteamVR_Action_Vibration>("/actions/default/out/Haptic")));
        }
    }
}
