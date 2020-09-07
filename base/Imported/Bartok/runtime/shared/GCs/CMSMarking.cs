//
// Copyright (c) Microsoft Corporation.   All rights reserved.
//

/*******************************************************************/
/*                           WARNING                               */
/* This file should be identical in the Bartok and Singularity     */
/* depots. Master copy resides in Bartok Depot. Changes should be  */
/* made to Bartok Depot and propagated to Singularity Depot.       */
/*******************************************************************/


namespace System.GCs {

    using Microsoft.Bartok.Runtime;
    using System.Threading;
    using System.Runtime.CompilerServices;

    //[NoBarriers]
    internal unsafe class CMSMarking
    {

        internal enum MarkingPhase {
            Dummy,              // Not used!
            Idle,               // We are not doing anything at this point
            Requested,          // Someone has asked for a GC to be started
            Preparing,          // No marking, but will be soon
            ComputingRoots,     // Marking the roots
            Tracing,            // Tracing gray objects
            StopTheWorld,       // Stop all mutators to do a STW trace
        }

        internal static int currentMarkingPhase;

        internal static UIntPtr markedColor;
        internal static UIntPtr unmarkedColor;

        internal static MarkingPhase CurrentMarkingPhase {
            [NoStackLinkCheck]
            get { return (MarkingPhase) currentMarkingPhase; }
        }

        // We use this negated variable because it is initialized by
        // the compiler to be false.  This allows the runtime system
        // to use the fast code paths early in the bootstrap process.
        private static bool referenceCheckIsSlow;

        internal static bool referenceCheckIsFast
        {
            [Inline]
            get { return !referenceCheckIsSlow; }
            [Inline]
            set { referenceCheckIsSlow = !value; }
        }

        internal static bool ReferenceCheckIsFast(int mask)
        {
            if ((mask & BarrierMask.PathSpec.UseMask) != 0) {
                return (mask & BarrierMask.PathSpec.AllowFast) !=0;
            } else {
                return referenceCheckIsFast;
            }
        }

        [NoInline]
        [CalledRarely]
        [NoStackLinkCheckTrans]
        internal static void ReferenceCheckSlow(UIntPtr *addr, Object value)
        {
#if !SINGULARITY || CONCURRENT_MS_COLLECTOR
            UIntPtr oldValue = *addr;
            MarkIfNecessary(oldValue);
            if (CurrentMarkingPhase == MarkingPhase.ComputingRoots) {
                MarkIfNecessary(Magic.addressOf(value));
            }
#endif // CONCURRENT_MS_COLLECTOR
        }

        /// <summary>
        /// In the sliding views phase, where some threads may have
        /// scanned their roots and others have not, we need to ensure
        /// that both old and new values will be marked and scanned.
        /// In the tracing phase we only need to ensure that the old
        /// values are traced and marked, as the old values may be the
        /// only references to a part of the snapshot reachable object
        /// graph from the untraced part of the object graph.
        /// </summary>
        /// <param name="addr">The memory location being modified</param>
        /// <param name="value">The reference value to be written into
        /// the "addr" location</param>
        /// <param name="mask">The barrier mode mask generated by the
        /// compiler</param>
        [Inline]
        [NoBarriers]
        [NoStackLinkCheckTrans]
        internal static void ReferenceCheck(UIntPtr *addr, Object value,
                                            int mask)
        {
#if !SINGULARITY || CONCURRENT_MS_COLLECTOR
            if (!ReferenceCheckIsFast(mask)) {
                ReferenceCheckSlow(addr, value);
            }
#endif // CONCURRENT_MS_COLLECTOR
        }

        [NoInline]
        [CalledRarely]
        [NoStackLinkCheckTrans]
        internal static void ReferenceCheckSlow(ref Object reference, Object value)
        {
#if !SINGULARITY || CONCURRENT_MS_COLLECTOR
            UIntPtr oldValue = Magic.addressOf(reference);
            MarkIfNecessary(oldValue);
            if (CurrentMarkingPhase == MarkingPhase.ComputingRoots) {
                MarkIfNecessary(Magic.addressOf(value));
            }
#endif // CONCURRENT_MS_COLLECTOR
        }

        /// <summary>
        /// In the sliding views phase, where some threads may have
        /// scanned their roots and others have not, we need to ensure
        /// that both old and new values will be marked and scanned.
        /// In the tracing phase we only need to ensure that the old
        /// values are traced and marked, as the old values may be the
        /// only references to a part of the snapshot reachable object
        /// graph from the untraced part of the object graph.
        /// </summary>
        /// <param name="reference">The memory location being modified</param>
        /// <param name="value">The reference value to be written into
        /// the "addr" location</param>
        /// <param name="mask">The barrier mode mask generated by the
        /// compiler</param>
        [Inline]
        [NoBarriers]
        [NoStackLinkCheckTrans]
        internal static void ReferenceCheck(ref Object reference, Object value,
                                            int mask)
        {
#if !SINGULARITY || CONCURRENT_MS_COLLECTOR
            if (!ReferenceCheckIsFast(mask)) {
                ReferenceCheckSlow(ref reference, value);
            }
#endif // CONCURRENT_MS_COLLECTOR
        }

        /// <summary>
        /// Ensures that a reference value is going to be marked and
        /// scanned.
        /// </summary>
        /// <param name="value">The reference value that may need to
        /// be marked</param>
        [NoBarriers]
        [NoStackLinkCheckTrans]
        internal static bool MarkIfNecessary(UIntPtr value)
        {
#if !SINGULARITY || CONCURRENT_MS_COLLECTOR
            if (value == 0) {
                return false;
            }
            UIntPtr marked = markedColor;
            if (PageTable.IsGcPage(PageTable.Page(value)) &&
                ThreadHeaderQueue.GcMark(Magic.fromAddress(value)) != marked) {
                VTable.Assert(PageTable.IsMyPage(PageTable.Page(value)));
                Thread thread = Thread.CurrentThread;
                UIntPtr unmarked = unmarkedColor;
                ThreadHeaderQueue.Push(thread, value, marked, unmarked);
                return true;
            }
#endif // CONCURRENT_MS_COLLECTOR
            return false;
        }

        [Inline]
        [NoBarriers]
        [NoStackLinkCheckTrans]
        [DisableNullChecks]
        internal static void MarkIfNecessaryInline(UIntPtr value,
                                                   Thread thread)
        {
#if !SINGULARITY || CONCURRENT_MS_COLLECTOR
            UIntPtr marked = markedColor;
            if (ThreadHeaderQueue.GcMark(Magic.fromAddress(value)) != marked) {
                VTable.Assert(PageTable.IsMyPage(PageTable.Page(value)));
                UIntPtr unmarked = unmarkedColor;
                ThreadHeaderQueue.Push(thread, value, marked, unmarked);
            }
#endif // CONCURRENT_MS_COLLECTOR
        }

        /// <summary>
        /// Ensures that an object is going to be marked and scanned.
        /// </summary>

        // Change these to ///-style comments when the TODO is removed:
        //
        // <param name="value">The object that may need to be marked</param>
        // <param name="t">!TODO!</param>
        [Inline]
        internal static void MarkObject(UIntPtr value, Thread t) {
#if !SINGULARITY || CONCURRENT_MS_COLLECTOR
            MarkIfNecessaryInline(value, t);
#endif // CONCURRENT_MS_COLLECTOR
        }

    }

}