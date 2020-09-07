//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//

// The Nucleus defines and establishes NucleusInv, which is abstract to managed code.
function NucleusInv($S:int, $StackState:[int]StackState, $toAbs:[int]int, $AbsMem:[int][int]int,
  CurrentStack:int, $gcSlice:[int]int, $color:[int]int, StackTop:int, $fs:[int]int, $fn:[int]int, CachePtr:int, CacheSize:int,
  ColorBase:int, HeapLo:int, HeapHi:int, ReserveMin:int,
  $Mem:[int]int, $sMem:[int]int, $dMem:[int]int, $pciMem:[int]int, $tMems:[int][int]int, $fMems:[int][int]int, $gcMem:[int]int,
  SLo:int, DLo:int, PciLo:int, TLo:int, FLo:int, GcLo:int, GcHi:int,
  $FrameCounts:[int]int, $FrameAddrs:[int][int]int, $FrameLayouts:[int][int]$FrameLayout,
  $FrameSlices:[int][int]int, $FrameAbss:[int][int][int]int, $FrameOffsets:[int][int]int,
  $IoMmuEnabled:bool, $PciConfigState:[int]int, DmaAddr:int
  ) returns(bool);

