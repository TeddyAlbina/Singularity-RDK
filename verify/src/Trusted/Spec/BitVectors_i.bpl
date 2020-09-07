//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//

// Bit vector definitions, hiding native bit-vector support

function  $add(x:bv32, y:bv32) returns(bv32);
function  $sub(x:bv32, y:bv32) returns(bv32);
function  $mul(x:bv32, y:bv32) returns(bv32);
function  $and(x:bv32, y:bv32) returns(bv32);
function  $or (x:bv32, y:bv32) returns(bv32);
function  $shr(x:bv32, y:bv32) returns(bv32);
function  $shl(x:bv32, y:bv32) returns(bv32);
function  $not(x:bv32)         returns(bv32);
function  $le (x:bv32, y:bv32) returns(bool);

function{:expand false} TBV(b:bv32) returns(bool) { true }

// meaning undefined if !word(i)
function B(i:int) returns(bv32);
function I(b:bv32) returns(int);

