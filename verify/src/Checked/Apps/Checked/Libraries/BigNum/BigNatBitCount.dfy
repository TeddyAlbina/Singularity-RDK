include "BigNatX86Spec.dfy"
include "BigNatCore.dfy"
include "BigNatCompare.dfy"	// just for the mul synonyms. clean up.
include "../Math/power2.dfy"

static lemma lemma_power2_5_is_32()
	ensures power2(5) == 32;
{
	lemma_power2_1_is_2();
	reveal_power2();
	assert power2(2)==4;
	assert power2(3)==8;
	assert power2(4)==16;
	assert power2(5)==32;
}

static lemma lemma_mul32power226_is_power31()
	ensures INTERNAL_mul(32,power2(26)) == power2(31);
{
	calc {
		INTERNAL_mul(32,power2(26));
			{ lemma_power2_5_is_32(); }
		INTERNAL_mul(power2(5),power2(26));
			{ lemma_exponentiation(5,26); }
		power2(5+26);
		power2(31);
	}
}

predicate ZeroBitRepresentation(A:BigNat, c:nat)
	requires WellformedBigNat(A);
{
	c==0 <==> zero(A)
}

predicate BitCount(A:BigNat, c:nat)
	requires WellformedBigNat(A);
{
	ZeroBitRepresentation(A,c)
	&& ((c>0) ==> (power2(c-1) <= I(A)))
	&& (I(A) < power2(c))
}

// It's a little insane to track bit counts in a BigNat. I doubt we're going to
// see bignums bigger than 2^32 bits long! Hence we add this ModestBigNat constraint
// to talk about bignums smaller than that.
// As with many things, it appears to be easier to track the bits precisely
// than to try to reason approximately (e.g. about word counts) all the way
// through, since then we have to account for possible slop at every step.

function KindaBigNat() : nat
{
	// Getting tight right up against 2^32 turned out to be tedious.
	// Here's an easy-to-reason-about number that's still pretty plump.
	power2(power2(31))
}

predicate ModestBigNatValue(A:BigNat)
{
	WellformedBigNat(A)
	&& I(A) < KindaBigNat()
}

predicate ModestBigNatBits(A:BigNat,ac:nat)
{
	WellformedBigNat(A)
	&& BitCount(A,ac)
	&& ac<=power2(31)
}

predicate ModestBigNatWords(A:BigNat)
{
	WellformedBigNat(A)
	&& |A.words| <= power2(26)
}

ghost method WordConstructOneBits(s:nat) returns (a:nat)
	requires s<=32;
	ensures power2(s)-1==a;
	ensures s==0 <==> (a==0);
	ensures Word32(a);
{
	if (s==0)
	{
		a := 0;
		lemma_power2_0_is_1();
	}
	else if (s==1)
	{
		a := 1;
		lemma_power2_1_is_2();
	}
	else
	{
		var sub_a:nat := WordConstructOneBits(s-1);
		a := 2*sub_a + 1;
		reveal_power2();
	}

	assert power2(s)-1==a;

	if (s>0)
	{
		assert power2(s)-1 < power2(s);
		lemma_power2_increases(s,32);
		assert power2(s) <= power2(32);
		assert a < power2(32);
	}
}

ghost method MakePower2Minus1(thirtytwos:nat,ones:nat) returns (S:BigNat)
	requires 0<ones<=32;
	ensures WellformedBigNat(S);
	ensures 0<=INTERNAL_mul(32,thirtytwos);
	ensures I(S) == power2(INTERNAL_mul(32,thirtytwos) + ones)-1;
	ensures |S.words| == thirtytwos + 1;
	ensures S.words[|S.words|-1] == power2(ones)-1;
	ensures forall i :: 0<=i<|S.words|-1 ==> S.words[i] == Width() - 1;
{
	lemma_mul_nonnegative(32,thirtytwos);

	if (thirtytwos == 0)
	{
		var ones_word:nat := WordConstructOneBits(ones);
		assert ones_word > 0;
		S := BigNat_ctor([ones_word]);
		calc ==>
		{
			true;
				{ selectively_reveal_I(S); }
			I(S) == INTERNAL_mul(I(BigNat_ctor(S.words[1..])),Width())+S.words[0];
				{ assert |S.words[1..]| == 0; }
			I(S) == INTERNAL_mul(I(BigNat_ctor([])),Width())+ones_word;
				{
					assert zero(BigNat_ctor([]));
					lemma_mul_annihilate(Width());
				}
			I(S) == ones_word;
			I(S) == power2(ones)-1;
				{ lemma_mul_annihilate(32); }
			I(S) == power2(INTERNAL_mul(32,thirtytwos)+ones)-1;
		}
	}
	else
	{
		var sub_S:BigNat := MakePower2Minus1(thirtytwos-1, ones);
		// I(sub_S) == power2(INTERNAL_mul(32,thirtytwos-1) + ones)-1;
		var low_word:nat := WordConstructOneBits(32);
		S := BigNat_ctor([low_word] + sub_S.words);
		calc {
			I(S);
				{ selectively_reveal_I(S); }
			INTERNAL_mul(I(sub_S), Width()) + low_word;
				// MakePower2Minus1(sub_S) ensures
			INTERNAL_mul(power2(INTERNAL_mul(32,thirtytwos-1) + ones)-1,Width()) + low_word;
				// Width ensures
			INTERNAL_mul(power2(INTERNAL_mul(32,thirtytwos-1) + ones)-1, power2(32)) + low_word;
				{ lemma_mul_commutative(power2(INTERNAL_mul(32,thirtytwos-1) + ones)-1, power2(32)); }
			INTERNAL_mul(power2(32), power2(INTERNAL_mul(32,thirtytwos-1) + ones)-1) + low_word;
				{ lemma_mul_distributive(power2(32),power2(INTERNAL_mul(32,thirtytwos-1) + ones),1); }
			INTERNAL_mul(power2(32), power2(INTERNAL_mul(32,thirtytwos-1) + ones)) - INTERNAL_mul(power2(32), 1) + low_word;
				{ lemma_exponentiation(32, INTERNAL_mul(32,thirtytwos-1) + ones); }
			power2(32+INTERNAL_mul(32,thirtytwos-1) + ones) - INTERNAL_mul(power2(32), 1) + low_word;
				{ lemma_mul_identity(32); lemma_mul_distributive(32,1,thirtytwos-1); }
			power2(INTERNAL_mul(32,thirtytwos) + ones) - INTERNAL_mul(power2(32), 1) + low_word;
				// WordConstructOneBits ensures
			power2(INTERNAL_mul(32,thirtytwos) + ones) - INTERNAL_mul(power2(32), 1) + power2(32) - 1;
					{ lemma_mul_identity(power2(32)); }
			power2(INTERNAL_mul(32,thirtytwos) + ones) - power2(32) + power2(32) - 1;
			power2(INTERNAL_mul(32,thirtytwos) + ones) - 1;
		}
	}
}

lemma lemma_bit_boundaries(A:BigNat, ac:nat, v:nat)
	requires WellformedBigNat(A);
	requires I(A) < power2(v);
	requires BitCount(A,ac);
	ensures ac <= v;
{
	if (v<ac)
	{
		assert v+1 <= ac;
		calc ==>
		{
			power2(ac-1) <= I(A);
				{ lemma_power2_increases(v,ac-1); }
			power2(v) <= I(A);
			false;
		}
	}
	assert ac<=v;
}

lemma lemma_modesty_bit_value_equivalence(A:BigNat,ac:nat)
	requires WellformedBigNat(A);
	requires BitCount(A,ac);
	ensures ModestBigNatBits(A,ac) <==> ModestBigNatValue(A);
{
	if (ModestBigNatBits(A,ac))
	{
		calc ==> {
				// ModestBigNatBits ensures
			ac <= power2(31);
				{ lemma_power2_increases(ac, power2(31)); }
			power2(ac) <= power2(power2(31));
				// BitCount(A,ac) ensures I(A) < power2(ac);
			I(A) < power2(power2(31));
			I(A) < KindaBigNat();
			ModestBigNatValue(A);
		}
	}
	assert ModestBigNatBits(A,ac) ==> ModestBigNatValue(A);

	if (ModestBigNatValue(A))
	{
		calc ==> {
			// ModestBigNatValue(A) ensures
			I(A) < KindaBigNat();
			I(A) < power2(power2(31));
				{ lemma_bit_boundaries(A, ac, power2(31)); }
			ac <= power2(31);
			ModestBigNatBits(A,ac);
		}
	}
	assert ModestBigNatValue(A) ==> ModestBigNatBits(A,ac);
}

lemma lemma_modesty_word_value_equivalence(A:BigNat)
	requires WellformedBigNat(A);
	ensures ModestBigNatWords(A) <==> ModestBigNatValue(A);
{
	var kinda_bignum:BigNat := MakePower2Minus1(power2(26)-1,32);
	calc {
		I(kinda_bignum);
		power2(INTERNAL_mul(32,power2(26)-1) + 32)-1;
			{ lemma_mul_identity(32); }
		power2(INTERNAL_mul(32,power2(26)-1) + INTERNAL_mul(32,1))-1;
			{ lemma_mul_distributive(32,power2(26)-1,1); }
		power2(INTERNAL_mul(32,power2(26)))-1;
			{ lemma_power2_5_is_32(); }
		power2(INTERNAL_mul(power2(5),power2(26)))-1;
			{ lemma_exponentiation(5,26); }
		power2(power2(5+26))-1;
		KindaBigNat()-1;
	}

	if (ModestBigNatWords(A))
	{
		if (|A.words| < |kinda_bignum.words|)
		{
			lemma_cmp_inequal_length(A,kinda_bignum);
			assert I(A) <= I(kinda_bignum);
		}
		else
		{
			lemma_le_equal_length(A,kinda_bignum);
			assert I(A) <= I(kinda_bignum);
		}
		assert I(A) < KindaBigNat();
		assert ModestBigNatValue(A);
	}
	assert ModestBigNatWords(A) ==> ModestBigNatValue(A);

	if (ModestBigNatValue(A))
	{
		assert I(A) < KindaBigNat();
		assert I(A) <= I(kinda_bignum);
		if (|A.words| > |kinda_bignum.words|)
		{
			lemma_cmp_inequal_length(kinda_bignum, A);
			assert I(kinda_bignum) < I(A);
			assert false;
		}
		assert |A.words| <= power2(26);
		assert ModestBigNatWords(A);
	}
	assert ModestBigNatValue(A) ==> ModestBigNatWords(A);
}

lemma lemma_zero_bits(A:BigNat, ac:nat)
	requires WellformedBigNat(A);
	requires BitCount(A,ac);
	ensures zero(A) <==> (ac==0);
{
	assert ZeroBitRepresentation(A,ac);
}

function method MakeSmallLiteralBigNat_def(x:nat) : BigNat
	requires x < Width();
	ensures WellformedBigNat(MakeSmallLiteralBigNat_def(x));
{
	if (x==0) then
		BigNat_ctor([])
	else
		BigNat_ctor([x])
}

lemma lemma_MakeSmallLiteralBigNat(x:nat)
	requires x < Width();
	ensures I(MakeSmallLiteralBigNat_def(x)) == x;
{
	var R:BigNat := MakeSmallLiteralBigNat_def(x);
	assert WellformedBigNat(R);
	if (x==0)
	{
		assert zero(R);
		assert I(R) == 0;
	}
	else
	{
		assert R.words == [x];
		calc {
			I(R);
				{ reveal_I(); }
			INTERNAL_mul(I(BigNat_ctor(R.words[1..])),Width())+R.words[0];
				{ assert |R.words[1..]| == 0; }
			INTERNAL_mul(I(BigNat_ctor([])),Width())+R.words[0];
				{
					reveal_I();
					lemma_mul_basics_forall();
				}
			R.words[0];
			x;
		}
		assert I(R) == x;
	}
}

function method MakeSmallLiteralBigNat(x:nat) : BigNat
	requires x < Width();
	ensures WellformedBigNat(MakeSmallLiteralBigNat(x));
	ensures I(MakeSmallLiteralBigNat(x))==x;
{
	lemma_MakeSmallLiteralBigNat(x);
	MakeSmallLiteralBigNat_def(x)
}

//////////////////////////////////////////////////////////////////////////////
// FrumpyBigNat: For when you need room to multiply two numbers.

function Frump() : nat
{
	power2(power2(30))
}

function FrumpyBigNat(N:BigNat) : bool
{
	WellformedBigNat(N)
	&& I(N) < Frump()
}

lemma lemma_frumpy_is_modest(X:BigNat)
	requires FrumpyBigNat(X);
	ensures ModestBigNatWords(X);
{
	lemma_power2_strictly_increases(30,31);
	lemma_power2_strictly_increases(power2(30),power2(31));
	lemma_modesty_word_value_equivalence(X);
}

lemma lemma_frumpy_product_is_modest(X:BigNat,Y:BigNat,XY:BigNat)
	requires FrumpyBigNat(X);
	requires FrumpyBigNat(Y);
	requires WellformedBigNat(XY);
	requires I(X)*I(Y) == I(XY);
	ensures ModestBigNatWords(XY);
{
	calc {
		I(XY);
		I(X) * I(Y);
		<=	{ lemma_mul_inequality(I(X), Frump(), I(Y)); }
		Frump() * I(Y);
			{ lemma_mul_is_commutative_forall(); }
		I(Y) * Frump();
		<	{ lemma_mul_strict_inequality(I(Y), Frump(), Frump()); }
		Frump() * Frump();
		power2(power2(30)) * power2(power2(30));
			{ lemma_power2_adds(power2(30), power2(30)); }
		power2(power2(30) + power2(30));
		power2(2 * power2(30));
			{ reveal_power2(); }
		power2(power2(31));
	}
	lemma_modesty_word_value_equivalence(XY);
}

lemma lemma_frumpy_squared_is_modest(X:BigNat,Xsquared:BigNat)
	requires FrumpyBigNat(X);
	requires WellformedBigNat(Xsquared);
	requires I(X)*I(X) == I(Xsquared);
	ensures ModestBigNatWords(Xsquared);
{
	lemma_frumpy_product_is_modest(X,X,Xsquared);
}

static lemma lemma_power2_strictly_increases_converse_imply(e1: int, e2: int)
    ensures 0 <= e1 && 0 < e2 && power2(e1) < power2(e2) ==> e1 < e2;
{
    if (0 <= e1 && 0 < e2 && power2(e1) < power2(e2))
    {
        lemma_power2_strictly_increases_converse(e1,e2);
    }
}

method WordCountBits(a:nat) returns (s:nat)
	requires Word32(a);
	ensures s>0 ==> power2(s-1) <= a;
	ensures s==0 <==> (a==0);
	ensures a < power2(s);
	ensures s<=32;
{
	// TODO there's an intel instruction to do this in one step.
	// TODO could implement this in log(32) time instead of linear.
	s := 0;
	var ps := 1;

	lemma_power2_0_is_1();
	while (ps<=a)
		decreases Width()-ps;
		invariant ps == power2(s);
		invariant s>0 ==> power2(s-1) <= a;
		invariant a==0 ==> s==0;
		invariant s==0 ==> a==0 || ps<=a;
		invariant s>=0;
	{
		s := s + 1;
//		ps := 2*ps; // TODO: implement *
		ps := ps+ps;
		reveal_power2();
	}
//	if (s>1)
	{
		assert s>1 ==> power2(s-1) <= a;
		assert a < Width();
		assert a < power2(32);
		assert s>1 ==> power2(s-1) < power2(32);
		lemma_power2_strictly_increases_converse_imply(s-1,32);
		assert s-1 < 32;
		assert s <= 32;
	}
	assert s <= 32;
}


method BigNatCountBits(A:BigNat) returns (c:nat)
	requires WellformedBigNat(A);
	requires ModestBigNatWords(A);
	ensures Word32(c);
	ensures BitCount(A, c);
{
	if (zero(A))
	{
		c := 0;
		lemma_power2_0_is_1();
	}
	else
	{
		var last_word:nat := A.words[|A.words|-1];
		var last_word_bits:nat := WordCountBits(last_word);

		calc ==> {
				// ModestBigNatWords(A) ensures
			|A.words| <= power2(26);
				{ lemma_power2_increases(26,32); }
			|A.words| <= power2(32);
			|A.words| <= Width();
			|A.words|-1 < Width();
			Word32(|A.words|-1);
		}

		lemma_2to32();
		assert Word32(32);

		var l,h := Product32(32, |A.words|-1);
		if (h>0)
		{
			h := h; // HACK: turn ghost if into real if until dafnycc can handle ghost if in real methods
			calc ==>
			{
				true;
					{ lemma_mul_properties(); }
				Width() <= INTERNAL_mul(h,Width());
				Width() <= l+INTERNAL_mul(h,Width());
					// Product32 ensures
				Width() <= INTERNAL_mul(32,|A.words|-1);
					{ lemma_mul_distributive(32,|A.words|,1); }
				Width() <= INTERNAL_mul(32,|A.words|)-INTERNAL_mul(32,1);
					{ lemma_mul_strictly_positive_forall(); }
				Width() < INTERNAL_mul(32,|A.words|);
					// Conflicts with ModestBigNat(A);
			}
			calc ==>
			{
				|A.words| <= power2(26);
					{ lemma_mul_left_inequality(32,|A.words|,power2(26)); }
				INTERNAL_mul(32,|A.words|) <= INTERNAL_mul(32,power2(26));
					{ lemma_mul32power226_is_power31(); }
				INTERNAL_mul(32,|A.words|) <= power2(31);
					{ lemma_power2_strictly_increases(31,32); }
				INTERNAL_mul(32,|A.words|) < Width();
			}
			assert false;
		}
		assert h==0;
		lemma_mul_annihilate(Width());
		assert l == INTERNAL_mul(32, |A.words|-1);

		c := l + last_word_bits;
		assert c > 0;
		assert zero(A) <==> c==0;

		// TODO: dafnycc local variable scoping
		ghost var thirtytwos:nat;
		ghost var ones:nat;
		ghost var lo_proxy:BigNat;

		// Prove lower bound. Requires special case at 1, where we can't ask for a
		// MakePower2Minus1, since that's 2^0-1 == 0, which has no defined 'ones' part.
		// So special-casing here keeps the main case cleaner.
		if (|A.words|==1 && last_word_bits==1)
		{
			last_word_bits := last_word_bits; // HACK: turn ghost if into real if until dafnycc can handle ghost if in real methods
			calc ==> {
				c == INTERNAL_mul(32,|A.words|-1) + last_word_bits;
					{ lemma_mul_annihilate(32); }
				c == 1;
			}
			lemma_power2_0_is_1();
			lemma_power2_1_is_2();
			assert power2(c-1) == power2(0) == 1 <= last_word < 2 == power2(c);
			assert last_word == 1;
			assert A.words[0] == last_word;
			assert A.words == [last_word];
			calc {
				I(A);
				I(BigNat_ctor([1]));
					{ selectively_reveal_I(BigNat_ctor([1])); }
				INTERNAL_mul(I(BigNat_ctor(A.words[1..])),Width())+1;
					{ selectively_reveal_I(BigNat_ctor([])); }
					{ reveal_I(); }
				INTERNAL_mul(0,Width())+1;
					{ lemma_mul_basics_forall(); }
				1;
			}
			assert I(A) == 1;

			assert I(A) < power2(c);
		}
		else if (last_word_bits==1)
		{
			last_word_bits := last_word_bits; // HACK: turn ghost if into real if until dafnycc can handle ghost if in real methods
			assert 2<=|A.words|;
//			ghost var thirtytwos:nat := |A.words|-2;
//			ghost var ones:nat := 32;
//			ghost var lo_proxy:BigNat := MakePower2Minus1(thirtytwos,ones);
			thirtytwos := |A.words|-2;
			ones := 32;
			lo_proxy := MakePower2Minus1(thirtytwos,ones);

			calc {
				INTERNAL_mul(32,thirtytwos)+ones;
					{ lemma_mul_identity(32); }
				INTERNAL_mul(32,thirtytwos)+INTERNAL_mul(32,1);
					{ lemma_mul_distributive(32,thirtytwos,1); }
				INTERNAL_mul(32,thirtytwos+1);
				INTERNAL_mul(32,|A.words|-1);
				c - 1;
			}
			assert power2(c-1)-1 == I(lo_proxy);
			lemma_cmp_inequal_length(lo_proxy, A);
			assert power2(c-1) <= I(A);
		}
		else
		{
//			ghost var thirtytwos:nat := |A.words|-1;
//			ghost var ones:nat := last_word_bits-1;
//			ghost var lo_proxy:BigNat := MakePower2Minus1(thirtytwos,ones);
			thirtytwos := |A.words|-1;
			ones := last_word_bits-1;
			lo_proxy := MakePower2Minus1(thirtytwos,ones);

			assert c == INTERNAL_mul(32,thirtytwos)+ones+1;
			assert power2(c-1)-1 == I(lo_proxy);
			lemma_lt_equal_length(lo_proxy, A);
			assert power2(c-1) <= I(A);
		}

		ghost var hi_proxy:BigNat := MakePower2Minus1(|A.words|-1,last_word_bits);

		calc ==>
		{
			true;
				// MakePower2Minus1 ensures
			I(hi_proxy) == power2(INTERNAL_mul(32,|A.words|-1) + last_word_bits)-1;
			I(hi_proxy) == power2(c)-1;
			  {
			assert |A.words| == |A.words|-1 + 1 == |hi_proxy.words|;
			assert A.words[|A.words|-1] < power2(c) <==> A.words[|A.words|-1] <= power2(c)-1 <==> A.words[|A.words|-1] <= hi_proxy.words[|hi_proxy.words|-1];
			/* [ckh] current workaround for non-ghost calc doesn't handle nested calcs
			  	calc {
					|A.words|;
					|A.words|-1 + 1;
					|hi_proxy.words|;
				}
				calc ==> {
					A.words[|A.words|-1] < power2(c);
					A.words[|A.words|-1] <= power2(c)-1;
					A.words[|A.words|-1] <= hi_proxy.words[|hi_proxy.words|-1];
				}
*/
				lemma_le_equal_length(A, hi_proxy);
			  }
			I(A) <= power2(c)-1;
			I(A) < power2(c);
		}

		calc ==>
		{
			c == INTERNAL_mul(32, |A.words|-1) + last_word_bits;
			c <= INTERNAL_mul(32, |A.words|-1) + 32;
				{ lemma_mul_identity(32); }
			c <= INTERNAL_mul(32, |A.words|-1) + INTERNAL_mul(32,1);
				{ lemma_mul_distributive(32,|A.words|-1,1); }
			c <= INTERNAL_mul(32, |A.words|);
				// ModestBigNatWords(A) ensures |A.words|<=power2(26)
				{ lemma_mul_left_inequality(32,|A.words|,power2(26)); }
			c <= INTERNAL_mul(32, power2(26));
				{ lemma_mul32power226_is_power31(); }
			c <= power2(31);
				{ lemma_power2_strictly_increases(31,32); }
			c < Width();
			Word32(c);
		}
	}
}

lemma lemma_unroll_power2(exp:nat)
    ensures power2(0) == 1;
    ensures power2(exp + 1) == 2 * power2(exp);
{
    reveal_power2();
}

method MakePower2Word32(exp:nat) returns (w:nat)
	requires exp<32;
	ensures w == power2(exp);
	ensures Word32(w);
{
    lemma_2to32();
    reveal_power2();
    var n:int := 0;
    w := 1;
//    lemma_unroll_power2(0);
    while (n < exp)
      invariant 0 <= n <= exp;
      invariant w == power2(n);
      invariant Word32(w);
    {
//        lemma_unroll_power2(n);
//        assert Word32(w + w);
        w := w + w;
        n := n + 1;
    }
/*
	// [ckh] Alas, this will cause a large amount of heap allocation on every call
	w := [
	1,
	2,
	4,
	8,
	16,
	32,
	64,
	128,
	256,
	512,
	1024,
	2048,
	4096,
	8192,
	16384,
	32768,
	65536,
	131072,
	262144,
	524288,
	1048576,
	2097152,
	4194304,
	8388608,
	16777216,
	33554432,
	67108864,
	134217728,
	268435456,
	536870912,
	1073741824,
	2147483648 ][exp];

	lemma_2to32();

	reveal_power2();
	assert 1==power2(0);
	assert 2==power2(1);
	assert 4==power2(2);
	assert 8==power2(3);
	assert 16==power2(4);
	assert 32==power2(5);
	assert 64==power2(6);
	assert 128==power2(7);
	assert 256==power2(8);
	assert 512==power2(9);
	assert 1024==power2(10);
	assert 2048==power2(11);
	assert 4096==power2(12);
	assert 8192==power2(13);
	assert 16384==power2(14);
	assert 32768==power2(15);
	assert 65536==power2(16);
	assert 131072==power2(17);
	assert 262144==power2(18);
	assert 524288==power2(19);
	assert 1048576==power2(20);
	assert 2097152==power2(21);
	assert 4194304==power2(22);
	assert 8388608==power2(23);
	assert 16777216==power2(24);
	assert 33554432==power2(25);
	assert 67108864==power2(26);
	assert 134217728==power2(27);
	assert 268435456==power2(28);
	assert 536870912==power2(29);
	assert 1073741824==power2(30);
	assert 2147483648==power2(31);
*/
}

method IsModestBigNat (A:BigNat) returns (modest:bool)
    requires WellformedBigNat(A);
    ensures modest == ModestBigNatValue(A);
{
    modest := (|A.words| <= 0x4000000);
    lemma_2to32();
    reveal_power2();
    assert 0x4000000 == power2(26);
    assert modest == ModestBigNatWords(A);
    lemma_modesty_word_value_equivalence(A);
}
