// Extract.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

int extractedFiles = 0;
long extractedSize = 0;

BOOL OnBeforeCopyFile(Cabinet::CExtract::kCabinetFileInfo * k_FI, void* p_Param)
{
	extractedFiles++;
	extractedSize += k_FI->s32_Size;
	// std::wcout << k_FI->u16_FullPath << std::endl;
	return TRUE;
}

void OnNextCabinet(Cabinet::CExtract::kCabinetInfo* pk_Info, int s32_Error, void* p_Param)
{
	std::cout << "--- Next cabinet: " << pk_Info->s8_NextCabinet << std::endl;
}

void OnAfterCopyFile(wchar_t * s8_File, Cabinet::CMemory *, void* /*p_Param*/)
{
	std::wcout << L" " << s8_File << std::endl;
}

int _tmain(int argc, _TCHAR* argv[])
{
	try
	{
		if (argc <= 1) 
		{
			throw std::exception("syntax: extract [cab]");
		}

		Cabinet::CExtract extract;
		Cabinet::CExtract::kCallbacks callbacks;
		callbacks.f_OnBeforeCopyFile = & OnBeforeCopyFile; 
		callbacks.f_OnAfterCopyFile = & OnAfterCopyFile;
		callbacks.f_OnNextCabinet = & OnNextCabinet;
		extract.SetCallbacks(& callbacks);

		wchar_t curdir[MAX_PATH];
		::GetCurrentDirectory(ARRAYSIZE(curdir), curdir);

		wchar_t cabdir[MAX_PATH];
		::GetFullPathName(argv[1], ARRAYSIZE(cabdir), cabdir, NULL);

		std::wcout << L"Extracting: " << cabdir << L" to " << curdir << std::endl;

		USES_CONVERSION;
		if (! extract.CreateFDIContext()) throw std::exception(W2A(extract.LastErrorW()));
		if (! extract.ExtractFileW(cabdir, curdir)) throw std::exception(W2A(extract.LastErrorW()));

		std::wcout << L"Extracted " << extractedFiles << L" file(s), " << extractedSize << L" byte(s)" << std::endl;
	}
	catch(std::exception& ex)
	{
		std::cerr << ex.what();
	}
	return 0;
}

