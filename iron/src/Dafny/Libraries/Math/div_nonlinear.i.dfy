//- <NuBuild AddDafnyFlag /z3opt:NL_ARITH=true/>
//- WARNING: In general, you shouldn't need to call these directly.  Try
//- to use the ones in div.i.dfy instead.  They're more full-featured anyway.







static lemma lemma_div_of_0(d:int)
    requires d != 0;
    ensures 0/d == 0;
{ }

static lemma lemma_div_by_self(d:int)
    requires d != 0;
    ensures d/d == 1;
{ }

static lemma lemma_small_div()
    ensures forall x, d :: 0 <= x < d && d > 0 ==> x / d == 0;
{ }

static lemma lemma_mod_of_zero_is_zero(m:int)
    requires 0 < m;
    ensures 0 % m == 0;
{ }

static lemma lemma_fundamental_div_mod(x:int, d:int)
    requires d != 0;
    ensures x == d * (x/d) + (x%d);
{ }

static lemma lemma_0_mod_anything()
    ensures forall m:int :: m > 0 ==> 0 % m == 0;
{ }

static lemma lemma_mod_yourself(m:int)
    ensures m > 0 ==> m % m == 0;
{ }

static lemma lemma_small_mod(x:nat, m:nat)
    requires x<m;
    requires 0<m;
    ensures x % m == x;
{ }

static lemma lemma_mod_range(x:int, m:int)
    requires m > 0;
    ensures 0 <= x % m < m;
{ }
