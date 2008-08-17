#include "stdafx.h"


int _tmain(int argc, _TCHAR* argv[])
{
    CoInitialize(NULL);

    MSIHANDLE hdb = NULL;
    // create the database
    LPCTSTR filename = L"Test.msi";
    MsiOpenDatabase(filename, MSIDBOPEN_CREATEDIRECT, & hdb);

    // set summary information, this makes an MSI runnable
    MSIHANDLE hsummary = NULL;
    MsiGetSummaryInformation(hdb, NULL, 7, & hsummary);
    MsiSummaryInfoSetPropertyA(hsummary, PID_REVNUMBER, VT_LPSTR, 0, NULL, "{00000000-0000-0000-0000-000000000000}");
    MsiSummaryInfoSetPropertyA(hsummary, PID_SUBJECT, VT_LPSTR, 0, NULL, "Test MSI");
    MsiSummaryInfoSetPropertyA(hsummary, PID_TITLE, VT_LPSTR, 0, NULL, "Test MSI");
    MsiSummaryInfoSetPropertyA(hsummary, PID_AUTHOR, VT_LPSTR, 0, NULL, "dB.");
    MsiSummaryInfoSetPropertyA(hsummary, PID_TEMPLATE, VT_LPSTR, 0, NULL, ";1033");
    MsiSummaryInfoSetProperty(hsummary, PID_PAGECOUNT, VT_I4, 100, NULL, NULL);
    MsiSummaryInfoSetProperty(hsummary, PID_WORDCOUNT, VT_I4, 100, NULL, NULL);
    // persiste the summary in the stream
    MsiSummaryInfoPersist(hsummary);
    MsiCloseHandle(hsummary);
    // commit changes to disk
    MsiDatabaseCommit(hdb);
    // close the database
    // MsiCloseHandle(hdb);

    // reopen as an MSI package, this function accepts opened handles in form of #handle
    wchar_t handle[12] = { 0 };
    _snwprintf(handle, ARRAYSIZE(handle), L"#%d", (UINT) hdb);
    MSIHANDLE hproduct = NULL;
    MsiOpenPackage(handle, & hproduct);

    // load CustomAction.dll
    HMODULE hca = LoadLibrary(L"CustomAction.dll");
    // find the address of SetProperty1CustomAction
    typedef int (__stdcall * LPCUSTOMACTION) (MSIHANDLE h);
    LPCUSTOMACTION lpca = (LPCUSTOMACTION) GetProcAddress(hca, "SetProperty1CustomAction");
    // call the custom action
    lpca(hproduct);

    wchar_t buffer[255] = { 0 };
    DWORD size = ARRAYSIZE(buffer);
    MsiGetProperty(hproduct, L"Property1", buffer, & size);
    if (wcscmp(buffer, L"Value1") == 0)
    {
        wprintf(L"success!");
    }

    FreeLibrary(hca);
    MsiCloseHandle(hproduct);
    CoUninitialize();
	return 0;
}

