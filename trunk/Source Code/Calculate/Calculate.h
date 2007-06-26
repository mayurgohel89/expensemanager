

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 6.00.0366 */
/* at Thu Jun 07 11:31:32 2007
 */
/* Compiler settings for .\Calculate.idl:
    Oicf, W1, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __Calculate_h__
#define __Calculate_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __ICalculateLogic_FWD_DEFINED__
#define __ICalculateLogic_FWD_DEFINED__
typedef interface ICalculateLogic ICalculateLogic;
#endif 	/* __ICalculateLogic_FWD_DEFINED__ */


#ifndef __CalculateLogic_FWD_DEFINED__
#define __CalculateLogic_FWD_DEFINED__

#ifdef __cplusplus
typedef class CalculateLogic CalculateLogic;
#else
typedef struct CalculateLogic CalculateLogic;
#endif /* __cplusplus */

#endif 	/* __CalculateLogic_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 

void * __RPC_USER MIDL_user_allocate(size_t);
void __RPC_USER MIDL_user_free( void * ); 

#ifndef __ICalculateLogic_INTERFACE_DEFINED__
#define __ICalculateLogic_INTERFACE_DEFINED__

/* interface ICalculateLogic */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ICalculateLogic;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("8777B92C-3979-4F2D-9811-86F384BCFB4E")
    ICalculateLogic : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE CostPerHead( 
            /* [in] */ DOUBLE lTotalAmount,
            /* [in] */ LONG lNumOfPeople,
            /* [retval][out] */ DOUBLE *lCostPerHead) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SomeExtraMethod( void) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ICalculateLogicVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ICalculateLogic * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ICalculateLogic * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ICalculateLogic * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ICalculateLogic * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ICalculateLogic * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ICalculateLogic * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ICalculateLogic * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *CostPerHead )( 
            ICalculateLogic * This,
            /* [in] */ DOUBLE lTotalAmount,
            /* [in] */ LONG lNumOfPeople,
            /* [retval][out] */ DOUBLE *lCostPerHead);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SomeExtraMethod )( 
            ICalculateLogic * This);
        
        END_INTERFACE
    } ICalculateLogicVtbl;

    interface ICalculateLogic
    {
        CONST_VTBL struct ICalculateLogicVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ICalculateLogic_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define ICalculateLogic_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define ICalculateLogic_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define ICalculateLogic_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define ICalculateLogic_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define ICalculateLogic_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define ICalculateLogic_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)


#define ICalculateLogic_CostPerHead(This,lTotalAmount,lNumOfPeople,lCostPerHead)	\
    (This)->lpVtbl -> CostPerHead(This,lTotalAmount,lNumOfPeople,lCostPerHead)

#define ICalculateLogic_SomeExtraMethod(This)	\
    (This)->lpVtbl -> SomeExtraMethod(This)

#endif /* COBJMACROS */


#endif 	/* C style interface */



/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ICalculateLogic_CostPerHead_Proxy( 
    ICalculateLogic * This,
    /* [in] */ DOUBLE lTotalAmount,
    /* [in] */ LONG lNumOfPeople,
    /* [retval][out] */ DOUBLE *lCostPerHead);


void __RPC_STUB ICalculateLogic_CostPerHead_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ICalculateLogic_SomeExtraMethod_Proxy( 
    ICalculateLogic * This);


void __RPC_STUB ICalculateLogic_SomeExtraMethod_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __ICalculateLogic_INTERFACE_DEFINED__ */



#ifndef __CalculateLib_LIBRARY_DEFINED__
#define __CalculateLib_LIBRARY_DEFINED__

/* library CalculateLib */
/* [helpstring][version][uuid] */ 


EXTERN_C const IID LIBID_CalculateLib;

EXTERN_C const CLSID CLSID_CalculateLogic;

#ifdef __cplusplus

class DECLSPEC_UUID("2DF5DC41-2326-46E5-A586-AD916534AB54")
CalculateLogic;
#endif
#endif /* __CalculateLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


