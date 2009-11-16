// CalculateLogicImpl.h : Declaration of the CCalculateLogicImpl

#pragma once
#include "resource.h"       // main symbols
#include "Calculate.h"


// CCalculateLogicImpl

class ATL_NO_VTABLE CCalculateLogicImpl : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CCalculateLogicImpl, &CLSID_CalculateLogic>,
	public IDispatchImpl<ICalculateLogic, &IID_ICalculateLogic, &LIBID_CalculateLib, /*wMajor =*/ 1, /*wMinor =*/ 0>
{
public:
	CCalculateLogicImpl()
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_CALCULATELOGICIMPL)


BEGIN_COM_MAP(CCalculateLogicImpl)
	COM_INTERFACE_ENTRY(ICalculateLogic)
	COM_INTERFACE_ENTRY(IDispatch)
END_COM_MAP()


	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}
	
	void FinalRelease() 
	{
	}

public:

	STDMETHOD(CostPerHead)(DOUBLE lTotalAmount, LONG lNumOfPeople, DOUBLE* lCostPerHead);
	STDMETHOD(SomeExtraMethod)();
};

OBJECT_ENTRY_AUTO(__uuidof(CalculateLogic), CCalculateLogicImpl)
