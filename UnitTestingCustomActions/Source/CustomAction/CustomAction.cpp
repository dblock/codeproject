#include "stdafx.h"

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
    return TRUE;
}

// a custom action that sets Property1=Value1
UINT __declspec(dllexport) __stdcall SetProperty1CustomAction(MSIHANDLE h)
{
    return ::MsiSetPropertyW(h, L"Property1", L"Value1");
}


