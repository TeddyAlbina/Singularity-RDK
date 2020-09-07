//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//

// Bit vector definitions, exposing native bit-vector support

function {:bvbuiltin "bvadd"}  $add(x:bv32, y:bv32) returns(bv32);
function {:bvbuiltin "bvsub"}  $sub(x:bv32, y:bv32) returns(bv32);
function {:bvbuiltin "bvmul"}  $mul(x:bv32, y:bv32) returns(bv32);
function {:bvbuiltin "bvand"}  $and(x:bv32, y:bv32) returns(bv32);
function {:bvbuiltin "bvor"}   $or (x:bv32, y:bv32) returns(bv32);
function {:bvbuiltin "bvlshr"} $shr(x:bv32, y:bv32) returns(bv32);
function {:bvbuiltin "bvshl"}  $shl(x:bv32, y:bv32) returns(bv32);
function {:bvbuiltin "bvnot"}  $not(x:bv32)         returns(bv32);
function {:bvbuiltin "bvule"}  $le (x:bv32, y:bv32) returns(bool);

function{:expand false} TBV(b:bv32) returns(bool) { true }

// meaning undefined if !word(i)
function B(i:int) returns(bv32);
function I(b:bv32) returns(int);

