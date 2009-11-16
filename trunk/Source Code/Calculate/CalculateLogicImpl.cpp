// CalculateLogicImpl.cpp : Implementation of CCalculateLogicImpl
#pragma once
#include "stdafx.h"
#include "CalculateLogicImpl.h"

STDMETHODIMP CCalculateLogicImpl::CostPerHead(DOUBLE dTotalAmount, LONG lNumOfPeople, DOUBLE* dCostPerHead)
{
	try
	{
		double dNumOfPeople = static_cast<double>(lNumOfPeople);
		if( lNumOfPeople < 1)
		{
			throw;
		}
		else 
		{
			*dCostPerHead = ( dTotalAmount / dNumOfPeople );
		}
	}
	catch( ... )
	{
	  OutputDebugString( _T("Exception in method CCalculateLogicImpl::CostPerHead()")); 
	}
	return S_OK;
}




STDMETHODIMP CCalculateLogicImpl::SomeExtraMethod()
{
	return S_OK;
}