/***
*assert.h - define the assert macro
*
*       Copyright (c) Microsoft Corporation. All rights reserved.
*
*Purpose:
*       Defines the assert(exp) macro.
*       [ANSI/System V]
*
*       [Public]
*
****/

#include <crtdefs.h>

#undef  assert

#ifdef  NDEBUG

#define assert(_Expression)     ((void)0)

#else

#ifdef  __cplusplus
extern "C" {
#endif

_CRTIMP void __cdecl _wassert(__in_z const wchar_t * _Message, __in_z const wchar_t *_File, __in unsigned _Line);

#ifdef  __cplusplus
}
#endif

#define assert(_Expression) (void)( (!!(_Expression)) || (_wassert(_CRT_WIDE(#_Expression), _CRT_WIDE(__FILE__), __LINE__), 0) )

#endif  /* NDEBUG */
