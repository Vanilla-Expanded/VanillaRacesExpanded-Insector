using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaRacesExpandedInsector
{
    public class Need_Pollution : Need
    {

        public float lastPollutionAcquired;

        public int lastPollutionAcquiredTick;

      
      

        public override bool ShowOnNeedList
        {
            get
            {
               
                if (this.pawn.HasActiveGene(InternalDefOf.VRE_PollutionDependency))
                {
                    return true;
                }
                else return false;


            }
        }



        public override void NeedInterval()
        {
            if (!this.IsFrozen)
            {
               
                this.CurLevel -= this.PollutionFallPerTick * 150f;
               

            }
        }

        private float PollutionFallPerTick
        {
            get
            {
                if (pawn.Map != null)
                {
                    if (pawn.Position.IsPolluted(pawn.Map))
                    {
                        return -this.def.fallPerDay / 60000f;
                    }
                    else
                    {
                        return this.def.fallPerDay / 60000f;
                    }
                }
                else return 0;
                
            }
        }

        public Need_Pollution(Pawn pawn) : base(pawn)
        {

        }

      






    }
}
