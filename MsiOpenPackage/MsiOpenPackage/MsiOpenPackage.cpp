// MsiOpenPackage.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

struct MsiDatabaseSummaryEntry
{
    DWORD dwPropertyId; // see http://msdn.microsoft.com/en-us/library/aa372045(VS.85).aspx
    VARENUM varPropertyType;
    CComVariant varValue;
};

std::wstring FormatMessageFromHRW(HRESULT hr)
{
    std::wstring result;
	LPWSTR lpMsgBuf = NULL;
	DWORD rc = 0;

    rc = ::FormatMessageW(
		FORMAT_MESSAGE_ALLOCATE_BUFFER |
		FORMAT_MESSAGE_FROM_SYSTEM,
		NULL,
		hr,
		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), // Default language
		reinterpret_cast<LPWSTR>(& lpMsgBuf),
		0,
		NULL );

	if (rc != 0)
	{
		result = lpMsgBuf;
	}
	else
	{
        std::wstringstream result_s;
        result_s << L"0x" << std::hex << hr;
		result = result_s.str();
	}
    return result;
}

int _tmain(int argc, _TCHAR* argv[])
{
	::CoInitialize(NULL);

	// create an empty MSI

	MSIHANDLE hDB = NULL;

	wchar_t * lpszFilename = L"C:\\temp\\MsiOpenPackage.msi";

	DWORD dwError = 0;
    if (0 != (dwError = ::MsiOpenDatabase(lpszFilename, MSIDBOPEN_CREATEDIRECT, & hDB)))
	{
		std::wcerr << L"Error creating: " << lpszFilename << L": 0x" << std::hex << HRESULT_FROM_WIN32(dwError)
			<< L" " << FormatMessageFromHRW(HRESULT_FROM_WIN32(dwError));
		return dwError;
	}
	
	// set summary information

    MsiDatabaseSummaryEntry summary[] = 
    {
        { PID_TITLE, VT_LPSTR, L"MSI Shim Database" },
        { PID_SUBJECT, VT_LPSTR, L"MSI Shim Database" },
        { PID_AUTHOR, VT_LPSTR, L"dB." },
        { PID_TEMPLATE, VT_LPSTR, ";1033" },
        { PID_REVNUMBER, VT_LPSTR, "{00869AA3-A32E-4398-89B2-5C5DC7328C7C}" },
        { PID_PAGECOUNT, VT_I4, 100 },
        { PID_WORDCOUNT, VT_I4, 100 },
    };

	MSIHANDLE hSummary;

	int size = ARRAYSIZE(summary);

	if (0 != (dwError = ::MsiGetSummaryInformation(hDB, NULL, size, & hSummary)))
	{
		std::wcerr << L"MsiGetSummaryInformation : 0x" << std::hex << HRESULT_FROM_WIN32(dwError)
			<< L" " << FormatMessageFromHRW(HRESULT_FROM_WIN32(dwError));
		return dwError;
	}

    for (int i = 0; i < size; i++)
    {
        switch(summary[i].varValue.vt)
        {
        case VT_BSTR:
			if (0 != (dwError = MsiSummaryInfoSetProperty(hSummary, summary[i].dwPropertyId, summary[i].varPropertyType, 0, NULL, static_cast<LPCWSTR>(summary[i].varValue.bstrVal))))
			{
				std::wcerr << L"MsiSummaryInfoSetProperty : 0x" << std::hex << HRESULT_FROM_WIN32(dwError)
					<< L" " << FormatMessageFromHRW(HRESULT_FROM_WIN32(dwError));
				return dwError;
			}
            break;
        case VT_I2:
        case VT_I4:
			if (0 != (dwError = MsiSummaryInfoSetProperty(hSummary, summary[i].dwPropertyId, summary[i].varPropertyType, summary[i].varValue.iVal, NULL, NULL)))
			{
				std::wcerr << L"MsiSummaryInfoSetProperty : 0x" << std::hex << HRESULT_FROM_WIN32(dwError)
					<< L" " << FormatMessageFromHRW(HRESULT_FROM_WIN32(dwError));
				return dwError;
			}
            break;
        }
    }

	if (0 != (dwError = ::MsiSummaryInfoPersist(hSummary)))
	{
		std::wcerr << L"MsiSummaryInfoPersist : 0x" << std::hex << HRESULT_FROM_WIN32(dwError)
			<< L" " << FormatMessageFromHRW(HRESULT_FROM_WIN32(dwError));
		return dwError;
	}

	if (0 != (dwError = ::MsiDatabaseCommit(hDB)))
	{
		std::wcerr << L"MsiDatabaseCommit : 0x" << std::hex << HRESULT_FROM_WIN32(dwError)
			<< L" " << FormatMessageFromHRW(HRESULT_FROM_WIN32(dwError));
		return dwError;
	}

	::MsiCloseHandle(hSummary);
	::MsiCloseHandle(hDB);

	// open package

	MSIHANDLE hProduct = NULL;
	if (0 != (dwError = ::MsiOpenPackage(lpszFilename, & hProduct)))
	{
		std::wcerr << L"Error opening: " << lpszFilename << L": 0x" << std::hex << HRESULT_FROM_WIN32(dwError)
			<< L" " << FormatMessageFromHRW(HRESULT_FROM_WIN32(dwError));
		return dwError;
	}

	::MsiCloseHandle(hProduct);

	std::wcout << L"OK";
	return 0;
}

