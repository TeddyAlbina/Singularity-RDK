//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//

// NOTE: This file contains declarations of various lemmas proved by
// the implementations in VerifiedBitVectorsBuiltinImpl.bpl:
//   - Do not modify this file without verifying that the implementations in
//     that file still prove the declarations in this file!
//   - Do not add any declarations to this file without adding an
//     implementation in that file!  (All lemmas must be proved!)

function $Aligned(b:bv32) returns(bool)
{
  $and(b, 3bv32) == 0bv32
}

function $bbvec4(a:[int]int, off:int, aBase:int, bb:[int]int, i0:int, i1:int, i2:int, g1:int, g2:int) returns(bool)
{
  (forall i:int::{TV(i)} TV(i) && word(i - i0) && i1 <= i && i < i2 && $Aligned(B(i - i0)) ==>
       between(g1, g2, g1 + 4 * I($shr(B(i - i0), 7bv32)))
    && (a[aBase + (i - i0)] == off <==>
          0bv32 == $and(B(bb[g1 + 4 * I($shr(B(i - i0), 7bv32))]),
                        $shl(1bv32, $and($shr(B(i - i0), 2bv32), 31bv32)))))
}

function $bb2vec4(a:[int]int, aBase:int, bb:[int]int, i0:int, i1:int, i2:int, g1:int, g2:int) returns(bool)
{
  (forall i:int::{TV(i)} TV(i) && word(i - i0) && i1 <= i && i < i2 && $Aligned(B(i - i0)) ==>
       between(g1, g2, g1 + 4 * I($shr(B(i - i0), 6bv32)))
    && (B(a[aBase + (i - i0)]) == $and(
          $shr(B(bb[g1 + 4 * I($shr(B(i - i0), 6bv32))]), $and($shr(B(i - i0), 1bv32), 31bv32)),
          3bv32
          )))
}

procedure _aligned($x:bv32);
  ensures  $Aligned($mul(4bv32, $x));

procedure _zeroAligned();
  ensures  $Aligned(0bv32);

procedure _andAligned($x:bv32);
  ensures  $and($x, 3bv32) == 0bv32 <==> $Aligned($x);

procedure _addAligned($x:bv32, $y:bv32);
  ensures  $Aligned($x) ==>
             ($Aligned($y) <==> $Aligned($add($x, $y)));

procedure _subAligned($x:bv32, $y:bv32);
  ensures  $Aligned($x) ==>
             ($Aligned($y) <==> $Aligned($sub($x, $y)));

procedure _notAligned($b:bv32);
  requires $Aligned($b);
  ensures  !$Aligned($add($b, 1bv32));
  ensures  !$Aligned($add($b, 2bv32));
  ensures  !$Aligned($add($b, 3bv32));
  ensures  $le($b, 4294967292bv32);

procedure _is4kAligned($x:bv32);
  ensures  $and($sub($x, $and($x, 4095bv32)), 4095bv32) == 0bv32;
  ensures  $le(0bv32, $and($x, 4095bv32)) && $le($and($x, 4095bv32), 4095bv32);

procedure _add4kAligned($x:bv32);
  requires $and($x, 4095bv32) == 0bv32;
  ensures  $and($add($x, 4096bv32), 4095bv32) == 0bv32;
  ensures  $Aligned($x);

procedure _is2m4kAligned($x:bv32);
  ensures  $and($sub($add($x, 2097152bv32), $and($x, 2097151bv32)), 4095bv32) == 0bv32;
  ensures  $le(0bv32, $and($x, 2097151bv32)) && $le($and($x, 2097151bv32), 2097151bv32);

procedure _initialize($unitSize:bv32);
  requires $le($unitSize, 16777215bv32);
  ensures  $shr(0bv32, 7bv32) == 0bv32;
  ensures  $shr($mul(128bv32, $unitSize), 7bv32) == $unitSize;
  ensures  $shr($mul(256bv32, $unitSize), 7bv32) == $add($unitSize, $unitSize);

procedure _bb4Zero($a:[int]int, $off:int, $aBase:int, $bb:[int]int, $i0:int, $i1:int, $i2:int, $g1:int, $g2:int, $idx:int);
  requires (forall i:int::{TV(i)} TV(i) && $i1 <= i && i < $i2 + 128 ==> $a[$aBase + (i - $i0)] == $off);
  requires $bbvec4($a, $off, $aBase, $bb, $i0, $i1, $i2, $g1, $g2);
  requires $Aligned(B($idx)) && $Aligned(B($g1));
  requires B($i2 - $i0) == $mul(32bv32, $sub(B($idx), B($g1)));
  requires $i1 == $i0;
  requires $le($shr(B($i2 - $i0), 7bv32), 33554431bv32) && $mul(128bv32, $shr(B($i2 - $i0), 7bv32)) == B($i2 - $i0) ==>
    $idx - $g1 == 4 * I($shr(B($i2 - $i0), 7bv32));
  requires (forall i:int::{TV(i)} TV(i) && $i2 <= i && i < $i2 + 128 ==>
    $le(B($i2 - $i0), B(i - $i0)) && $le(B(i - $i0), $add(B($i2 - $i0), 127bv32)));
  requires between($g1, $g2, $idx);
  requires B(0) == 0bv32;
  ensures  $bbvec4($a, $off, $aBase, $bb[$idx := 0], $i0, $i1, $i2 + 128, $g1, $g2);

procedure _bb4GetBit($i0:int, $k:int);
  ensures  $le($and($shr(B($k - $i0), 2bv32), 31bv32), 31bv32);

procedure _bb4SetBit($a:[int]int, $on:int, $off:int, $aBase:int, $bb:[int]int, $i0:int, $i1:int, $i2:int, $k:int, $idx:int, $bbb:int, $ret:[int]int, $g1:int, $g2:int);
  requires $bbvec4($a, $off, $aBase, $bb, $i0, $i1, $i2, $g1, $g2);
  requires TV($k) && word($k - $i0) && $i1 <= $k && $k < $i2 && $Aligned(B($k - $i0));
  requires $on != $off;
  requires $idx == $g1 + 4 * I($shr(B($k - $i0), 7bv32));
  requires B($bbb) == $or(B($bb[$idx]), $shl(1bv32, $and($shr(B($k - $i0), 2bv32), 31bv32)));
  requires $ret == $bb[$idx := $bbb];
  ensures  $bbvec4($a[$aBase + ($k - $i0) := $on], $off, $aBase, $ret, $i0, $i1, $i2, $g1, $g2);
  ensures  between($g1, $g2, $idx);
  ensures  $le($and($shr(B($k - $i0), 2bv32), 31bv32), 31bv32);

procedure _bb4Zero2($a:[int]int, $aBase:int, $bb:[int]int, $i0:int, $i1:int, $i2:int, $g1:int, $g2:int, $idx:int);
  requires (forall i:int::{TV(i)} TV(i) && $i1 <= i && i < $i2 + 64 ==> $a[$aBase + (i - $i0)] == 0);
  requires $bb2vec4($a, $aBase, $bb, $i0, $i1, $i2, $g1, $g2);
  requires $Aligned(B($idx)) && $Aligned(B($g1));
  requires B($i2 - $i0) == $mul(16bv32, $sub(B($idx), B($g1)));
  requires $i1 == $i0;
  requires $le($shr(B($i2 - $i0), 6bv32), 67108863bv32) && $mul(64bv32, $shr(B($i2 - $i0), 6bv32)) == B($i2 - $i0) ==>
    $idx - $g1 == 4 * I($shr(B($i2 - $i0), 6bv32));
  requires (forall i:int::{TV(i)} TV(i) && $i2 <= i && i < $i2 + 64 ==>
    $le(B($i2 - $i0), B(i - $i0)) && $le(B(i - $i0), $add(B($i2 - $i0), 63bv32)));
  requires between($g1, $g2, $idx);
  requires B(0) == 0bv32;
  ensures  $bb2vec4($a, $aBase, $bb[$idx := 0], $i0, $i1, $i2 + 64, $g1, $g2);

procedure _bb4Get2Bit($i0:int, $k:int);
  ensures  $le($and($shr(B($k - $i0), 1bv32), 31bv32), 31bv32);

procedure _bb4Set2Bit($a:[int]int, $val:int, $aBase:int, $bb:[int]int, $i0:int, $i1:int, $i2:int, $k:int, $idx:int, $bbb:int, $_bbb:int, $ret:[int]int, $g1:int, $g2:int);
  requires $bb2vec4($a, $aBase, $bb, $i0, $i1, $i2, $g1, $g2);
  requires TV($k) && word($k - $i0) && $i1 <= $k && $k < $i2 && $Aligned(B($k - $i0));
  requires $idx == $g1 + 4 * I($shr(B($k - $i0), 6bv32));
  requires $le(B($val), 3bv32);
  requires B($bbb) == $and(B($bb[$idx]), $not($shl(3bv32, $and($shr(B($k - $i0), 1bv32), 31bv32))));
  requires B($_bbb) == $or(B($bbb), $shl(B($val), $and($shr(B($k - $i0), 1bv32), 31bv32)));
  requires $ret == $bb[$idx := $_bbb];
  ensures  $bb2vec4($a[$aBase + ($k - $i0) := $val], $aBase, $ret, $i0, $i1, $i2, $g1, $g2);
  ensures  between($g1, $g2, $idx);
  ensures  $le($and($shr(B($k - $i0), 1bv32), 31bv32), 31bv32);

procedure _const();
  ensures $sub(1bv32, 1bv32) == 0bv32;
  ensures $add(1bv32, 1bv32) == 2bv32;
  ensures $add(2bv32, 1bv32) == 3bv32;
  ensures $add(2bv32, 2bv32) == 4bv32;
  ensures $add(4bv32, 1bv32) == 5bv32;
  ensures $add(5bv32, 1bv32) == 6bv32;
  ensures $add(5bv32, 2bv32) == 7bv32;
  ensures $mul(4bv32, 4bv32) == 16bv32;
  ensures $add(16bv32, 16bv32) == 32bv32;
  ensures $sub(32bv32, 1bv32) == 31bv32;
  ensures $add(32bv32, 32bv32) == 64bv32;
  ensures $sub(64bv32, 1bv32) == 63bv32;
  ensures $mul(32bv32, 4bv32) == 128bv32;
  ensures $sub(128bv32, 1bv32) == 127bv32;
  ensures $mul(16bv32, 16bv32) == 256bv32;
  ensures $add(256bv32, 256bv32) == 512bv32;
  ensures $mul(64bv32, 64bv32) == 4096bv32;
  ensures $sub(4096bv32, 1bv32) == 4095bv32;
  ensures $mul(256bv32, 256bv32) == 65536bv32;
  ensures $sub(65536bv32, 1bv32) == 65535bv32;
  ensures $mul(65536bv32, 32bv32) == 2097152bv32;
  ensures $sub(2097152bv32, 1bv32) == 2097151bv32;
  ensures $mul(65536bv32, 256bv32) == 16777216bv32;
  ensures $sub(16777216bv32, 1bv32) == 16777215bv32;
  ensures $mul(65536bv32, 512bv32) == 33554432bv32;
  ensures $sub(33554432bv32, 1bv32) == 33554431bv32;
  ensures $add(33554432bv32, 33554432bv32) == 67108864bv32;
  ensures $sub(67108864bv32, 1bv32) == 67108863bv32;
  ensures $mul(65536bv32, 65535bv32) == 4294901760bv32;
  ensures $add(4294901760bv32, 65535bv32) == 4294967295bv32;
  ensures $sub(4294967295bv32, 3bv32) == 4294967292bv32;

