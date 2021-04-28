
#include "EntropyLibrary.h"
#include <cmath>

#define PLATFORM_ANDROID 0
#define PLATFORM_IOS 1

char* getTemplateInfo()
{
#if PLATFORM == PLATFORM_IOS
	static char info[] = "Platform for iOS";
#elif PLATFORM == PLATFORM_ANDROID
	static char info[] = "Platform for Android";
#else
	static char info[] = "Undefined platform";
#endif

	return info;
}

EntropyLibrary::EntropyLibrary()
{
	TotalEntropy = 0;
}

EntropyLibrary* GetEntropyLibrary()
{
	return new EntropyLibrary();
}


Symbol::Symbol(char Character) {
	this->Character = Character;
	this->Count = 1;
}
Symbol* EntropyLibrary::FindSymbol(vector<Symbol*> Symbols, char character)
{
	for (unsigned long i = 0; i < Symbols.size(); i++)
	{
		Symbol* symbol = Symbols[i];
		if (symbol->Character == character) {
			return symbol;
		}
	}
	return NULL;
}


EntropyLibrary::~EntropyLibrary()
{
}



void ReleaseEntropyLibrary(EntropyLibrary* handle) {
	handle->Symbols.clear();
	handle->Symbols.shrink_to_fit();
}

void CalculateTotalEntropy(EntropyLibrary* handle)
{
	handle->TotalEntropy = 0;
	unsigned long TotalSymbols = 0;
	for (unsigned long i = 0; i < handle->Symbols.size(); i++)
	{
		Symbol* symbol = handle->Symbols[i];
		TotalSymbols += symbol->Count;
	}

	for (unsigned long i = 0; i < handle->Symbols.size(); i++)
	{
		Symbol* symbol = handle->Symbols[i];
		long double count = symbol->Count;
		symbol->Frecuency = count / TotalSymbols;

		handle->TotalEntropy += (symbol->Frecuency * log2(1 / symbol->Frecuency));
	}
}



double GetTotalEntropy(EntropyLibrary* handle) {
	return handle->TotalEntropy;
}
int ReadFromFile(EntropyLibrary* handle, const char* Filepath)
{
	string filename(Filepath);
	char character = 0;

	ifstream input_file(filename);
	if (!input_file.is_open()) {
		cerr << "Could not open the file - '"
			<< filename << "'" << endl;
		return EXIT_FAILURE;
	}

	while (input_file.get(character)) {
		Symbol* symbol = handle->FindSymbol(handle->Symbols, character);
		if (symbol != NULL) {
			symbol->Count++;
			continue;
		}
		handle->Symbols.push_back(new Symbol(character));
	}
	cout << endl;
	input_file.close();
	CalculateTotalEntropy(handle);
	return EXIT_SUCCESS;
}

unsigned long  GetSymbolsLenght(EntropyLibrary* handle)
{
	return handle->Symbols.size();
}
char GetSymbolChar(EntropyLibrary* handle, unsigned long index) {
	return handle->Symbols[index]->Character;
}
int GetSymbolCount(EntropyLibrary* handle, unsigned long index) {
	return handle->Symbols[index]->Count;
}
float GetSymbolFrecuency(EntropyLibrary* handle, unsigned long index) {
	return handle->Symbols[index]->Frecuency;
}


float Calculate(float* EventsProbability,int size)
{
	float totalEntropy = 0;
	for (int i = 0; i < size; i++) {
		float probability = EventsProbability[i];
		totalEntropy += (probability * log2(1 / probability));
	}
	return totalEntropy;
}